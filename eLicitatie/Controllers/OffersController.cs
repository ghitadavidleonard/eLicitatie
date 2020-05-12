using Common;
using Common.Offer;
using eLicitatie.Api.Entities;
using eLicitatie.Api.Extensions;
using eLicitatie.Api.Extensions.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eLicitatie.Api.Controllers
{
    [Authorize]
    [ApiController]
    public class OffersController : ControllerBase
    {
        private readonly eLicitatieDbContext _context;

        public OffersController(eLicitatieDbContext context) 
        {
            _context = context;
        }

        [HttpPost("api/offers")]
        public async Task<IActionResult> CreateOffer(OfferRequest request)
        {
            if (request is null) return UnprocessableEntity(Constants.ErrorMessages.UnprocessableEntity);

            if (!_context.Auctions.Any(a => a.Id == request.AuctionId))
                return BadRequest(Constants.ErrorMessages.AuctionDoesntExist);

            if (_context.Auctions.Where(a => a.Id == request.AuctionId).Select(a => a.StartDate).FirstOrDefault().Date > DateTime.UtcNow)
                return BadRequest(Constants.ErrorMessages.ToSoon);

            var auctionPrice = 0M;
            if (_context.Offers.Any(a => a.AuctionId == request.AuctionId))
            {
                auctionPrice = _context.Offers.Where(a => a.AuctionId == request.AuctionId)
                    .OrderByDescending(a => a.Price)
                    .Select(a => a.Price)
                    .FirstOrDefault();

                if (request.Price <= auctionPrice)
                    return BadRequest(Constants.ErrorMessages.InvalidPriceOffer);
            }
            else
            {
                auctionPrice = _context.Auctions.Where(a => a.Id == request.AuctionId)
                        .Select(a => a.StartingPrice).FirstOrDefault();

                if (request.Price < auctionPrice)
                    return BadRequest(Constants.ErrorMessages.InvalidPriceOffer);
            }

            var offer = request.MapToEntity();

            bool parsed = int.TryParse(User.Identity.Name, out int userId);
            if (!parsed) { throw new ApplicationException(Constants.ErrorMessages.UnreachableUserId); }

            offer.UserId = userId;

            var entity = _context.Offers.Add(offer);

            try { await _context.SaveChangesAsync().ConfigureAwait(false); }
            catch (Exception) { return this.InternalServerError(); }

            return Ok(entity.Entity);
        }

        [HttpGet("api/offers/forAuction/{auctionId:int}")]
        public IEnumerable<OfferResponse> GetOffers(int auctionId)
        {
            var offers = new List<OfferResponse>();

            foreach (var item in _context.Offers.Include(c => c.User).Where(p => p.AuctionId == auctionId).ToList())
            {
                offers.Add(item.MapToResponse());
            }

            return offers;
        }
    }
}
