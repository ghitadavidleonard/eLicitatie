using Common;
using Common.Auction;
using Common.Enums;
using eLicitatie.Api.Entities;
using eLicitatie.Api.Extensions;
using eLicitatie.Api.Extensions.Mappers;
using eLicitatie.Api.ModelBinders;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace eLicitatie.Api.Controllers
{
    [Authorize]
    [ApiController]
    public class AuctionsController : ControllerBase
    {
        private readonly eLicitatieDbContext _context;

        public AuctionsController(eLicitatieDbContext context)
        {
            _context = context;
        }

        [HttpPost("api/auctions")]
        public async Task<IActionResult> Create([FromBody]AuctionRequest auctionRequest)
        {
            if (auctionRequest is null) return UnprocessableEntity(Constants.ErrorMessages.UnprocessableEntity);
            if (auctionRequest.StartDate < DateTime.UtcNow.Date) return BadRequest(Constants.ErrorMessages.StartDateBeforeToday);
            if (auctionRequest.StartingPrice < (int)Metrics.MinimumAllowedValue) return BadRequest(Constants.ErrorMessages.MinimumStartPrice);
            if (auctionRequest.DaysActive < (int)Metrics.MinimumAllowedValue) return BadRequest(Constants.ErrorMessages.MinimumDaysActive);
            if (_context.Auctions.Any(a => a.ProductId == auctionRequest.ProductId)) return BadRequest(Constants.ErrorMessages.AuctionAllreadyExists);

            var auction = auctionRequest.MapToEntity();

            bool parsed = int.TryParse(User.Identity.Name, out int userId);
            if (!parsed) { throw new ApplicationException(Constants.ErrorMessages.UnreachableUserId); }

            auction.CreatorId = userId;

            var entity = _context.Auctions.Add(auction);

            try { await _context.SaveChangesAsync().ConfigureAwait(false); }
            catch (Exception) { return this.InternalServerError(); }
            
            BackgroundJob.Schedule(() => CloseAuction(auction.Id), TimeSpan.FromDays(auction.DaysActive));

            return Ok(entity.Entity);
        }

        [HttpGet("api/auctions")]
        public async Task<IEnumerable<AuctionResponse>> GetAuctions([ModelBinder(typeof(CommaDelimitedArrayModelBinder))]int[] categories)
        {
            IQueryable<Auction> query = null;

            if (categories != null && categories.Any())
                query = _context.Auctions
                    .Include(p => p.Creator)
                    .Include(p => p.Product)
                    .ThenInclude(c => c.ProductCategories)
                    .Where(a => a.Product.ProductCategories.Any(pc => categories.Any(c => c == pc.CategoryId)));
            else
                query = _context.Auctions.Include(p => p.Product).Include(a => a.Creator);

            var auctions = new List<AuctionResponse>();

            foreach (var item in await query.ToListAsync())
            {
                auctions.Add(item.MapToResponse());
            }

            return auctions;
        }

        [HttpGet("api/auctions/{auctionId}")]
        public async Task<IActionResult> GetAuction(int auctionId)
        {
            if (auctionId == default) return BadRequest(Constants.ErrorMessages.UnhandledRequest);

            var auction = await _context.Auctions
                .Include(p => p.Offers)
                    .ThenInclude(ofi => ofi.User)
                .Include(p => p.Creator)
                .Include(p => p.Product)
                    .ThenInclude(p => p.ProductCategories)
                    .ThenInclude(p => p.Category)
                .SingleOrDefaultAsync(a => a.Id == auctionId);

            if (auction == null)
                return NotFound(Constants.ErrorMessages.NotFound);

            return Ok(auction.MapToResponse());
        }

        [HttpDelete("api/auctions/delete/{auctionId:int}")]
        public async Task<IActionResult> Delete(int auctionId)
        {
            if (auctionId == default)
                return BadRequest(Constants.ErrorMessages.UnprocessableEntity);

            var entity = _context.Auctions.Where(a => a.Id == auctionId).FirstOrDefault();

            if (entity == null)
                return NotFound(Constants.ErrorMessages.NotFound);

            if (!_context.Offers.Any(o => o.AuctionId == auctionId) || DateTime.UtcNow.Date > entity.StartDate.AddDays(entity.DaysActive))
            {
                _context.Offers.RemoveRange(entity.Offers);
                _context.Auctions.Remove(entity);

                try { await _context.SaveChangesAsync().ConfigureAwait(false); }
                catch (Exception) { Debug.WriteLine(Constants.ErrorMessages.SmthIsWrong); }
            }
            else
                return BadRequest(Constants.ErrorMessages.AuctionHasOffers);   

            return Ok();
        }

        [HttpGet("api/auctions/close/{auctionId:int}")]
        public async Task<IActionResult> CloseAuction(int auctionId)
        {
            var entity = _context.Auctions.Include(a => a.Product).Where(a => a.Id == auctionId).FirstOrDefault();

            if (entity == null) return UnprocessableEntity(Constants.ErrorMessages.UnprocessableEntity);

            if (DateTime.UtcNow.Date >= entity.StartDate.AddDays(entity.DaysActive))
            {
                // if final date passed and contract has at least one offer change the owner to the one with the biggest offer
                if (_context.Offers.Any(o => o.AuctionId == auctionId))
                {
                    entity.Product.OwnerId = _context.Offers.Where(o => o.AuctionId == auctionId)
                        .OrderByDescending(a => a.Price)
                        .Select(b => b.UserId).FirstOrDefault();
                    _context.Products.Update(entity.Product);
                }
                _context.Offers.RemoveRange(entity.Offers);
                _context.Auctions.Remove(entity);

                try { await _context.SaveChangesAsync().ConfigureAwait(false); }
                catch (Exception) { Debug.WriteLine(Constants.ErrorMessages.SmthIsWrong); }
            }

            return Ok();
        }
    }
}