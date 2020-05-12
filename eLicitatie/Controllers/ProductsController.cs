using Common;
using Common.Enums;
using Common.Product;
using Common.ProductType;
using eLicitatie.Api.Entities;
using eLicitatie.Api.Extensions;
using eLicitatie.Api.Extensions.Mappers;
using eLicitatie.Api.ModelBinders;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.ResponseCaching;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eLicitatie.Api.Controllers
{
    [Authorize]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        public readonly eLicitatieDbContext _context;

        public ProductsController(eLicitatieDbContext context)
        {
            _context = context;
        }

        [HttpGet("api/products/image/{productId:int}")]
        public async Task<IActionResult> Image(int productId)
        {
            var item = await _context.Products.SingleOrDefaultAsync(it => it.Id == productId);

            if (item is null) return NotFound();

            return File(item.Image, "application/octet-stream");
        }

        [HttpGet("api/products")]
        public async Task<IEnumerable<ProductResponse>> GetProducts([ModelBinder(typeof(CommaDelimitedArrayModelBinder))]int[] categories)
        {
            IQueryable<Product> query = null;

            bool parsed = int.TryParse(User.Identity.Name, out int userId);
            if (!parsed) { throw new ApplicationException(Constants.ErrorMessages.UnreachableUserId); }

            if (categories != null && categories.Any())
            {
                query = _context.Products
                    .Include(p => p.ProductCategories)
                        .ThenInclude(pc => pc.Category)
                    .Where(p => (p.OwnerId == userId) && (p.ProductCategories.Any(pc => categories.Any(c => c == pc.CategoryId))));
            }
            else
                query = _context.Products
                    .Include(p => p.ProductCategories)
                        .ThenInclude(pc => pc.Category)
                    .Where(p => p.OwnerId == userId);

            var products = new List<ProductResponse>();

            foreach (var item in await query.ToListAsync())
            {
                products.Add(item.MapToResponse());
            }

            return products;
        }

        [HttpGet("api/products/{productId:int}")]
        public async Task<IActionResult> GetProduct(int productId)
        {
            if (productId == default)
                return UnprocessableEntity(Constants.ErrorMessages.UnprocessableEntity);

            var product = await _context.Products.Include(c => c.ProductCategories).ThenInclude(mr => mr.Category).Where(p => p.Id == productId).FirstOrDefaultAsync();

            return Ok(product.MapToResponse());
        }

        [HttpGet("api/products/forUser/{id:int}")]
        public IEnumerable<ProductResponse> GetProductsFor(int id)
        {
            var products = new List<ProductResponse>();

            foreach (var prod in _context.Products.Include(c => c.ProductCategories).ThenInclude(mr => mr.Category).Where(p => p.OwnerId == id).ToList())
            {
                products.Add(prod.MapToResponse());
            }

            return products;
        }

        [HttpPost("api/products")]
        public async Task<IActionResult> AddProduct([FromBody]ProductRequest productRequest)
        {
            if (productRequest is null) return UnprocessableEntity(Constants.ErrorMessages.UnprocessableEntity);

            var descriptionErrorMessage = VerifyProductDescription(productRequest.ShortDescription, productRequest.LongDescription);

            if (!string.IsNullOrWhiteSpace(descriptionErrorMessage))
                return BadRequest(descriptionErrorMessage);


            Product product = productRequest.MapToEntity();

            bool parsed = int.TryParse(User.Identity.Name, out int userId);
            if (!parsed) { throw new ApplicationException(Constants.ErrorMessages.UnreachableUserId); }

            product.OwnerId = userId;

            var entry = _context.Products.Add(product);

            try { await _context.SaveChangesAsync().ConfigureAwait(false); }
            catch (Exception) { return this.InternalServerError(); }

            var response = entry.Entity.MapToResponse();

            return Ok(response);
        }

        [HttpDelete("api/products/delete/{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (id <= 0)
                return BadRequest(Constants.ErrorMessages.UnhandledRequest);

            Product product = await _context.Products.SingleOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound(Constants.ErrorMessages.NotFound);

            if (_context.Auctions.Any(a => a.ProductId == id))
                return BadRequest(Constants.ErrorMessages.ProductIsLinkedToAnAuction);

            _context.Products.Remove(product);

            try { await _context.SaveChangesAsync(); }
            catch (Exception) { return this.InternalServerError(); }

            return Ok(product);
        }

        [HttpPut("api/products")]
        public async Task<IActionResult> UpdateProduct([FromBody]ProductFullRequest productRequest)
        {
            if (productRequest is null) return UnprocessableEntity(Constants.ErrorMessages.UnprocessableEntity);

            var descriptionErrorMessage = VerifyProductDescription(productRequest.ShortDescription, productRequest.LongDescription);

            if (!string.IsNullOrWhiteSpace(descriptionErrorMessage))
                return BadRequest(descriptionErrorMessage);

            Product product = await _context.Products.SingleOrDefaultAsync(p => p.Id == productRequest.Id);

            if (product is null) return NotFound(Constants.ErrorMessages.NotFound);

            bool parsed = int.TryParse(User.Identity.Name, out int userId);
            if (!parsed) { throw new ApplicationException(Constants.ErrorMessages.UnreachableUserId); }

            if (product.OwnerId != userId || product.OwnerId == default) return Forbid(Constants.ErrorMessages.ThisUserIsNotAllowed);

            product.ShortDescription = productRequest.ShortDescription;
            product.LongDescription = productRequest.LongDescription;
            product.Image = productRequest.Image.ToArray();

            if (productRequest.Categories != null && productRequest.Categories.Any())
            {
                var categories = _context.ProductCategories.Where(p => p.ProductId == product.Id).ToList();
                _context.ProductCategories.RemoveRange(categories);
                foreach (var categ in productRequest.Categories)
                {
                    var pc = new ProductCategory
                    {
                        Product = product,
                        CategoryId = categ.Id
                    };
                    product.ProductCategories.Add(pc);
                }
            }

            var entry = _context.Products.Update(product);

            try { await _context.SaveChangesAsync().ConfigureAwait(false); }
            catch (Exception) { return this.InternalServerError(); }

            var response = entry.Entity.MapToResponse();

            return Ok(response);
        }

        private string VerifyProductDescription(string shortDescription, string longDescription)
        {
            if (string.IsNullOrWhiteSpace(shortDescription) || string.IsNullOrWhiteSpace(longDescription))
                return Constants.ErrorMessages.EmptyProductNameAndDescription;

            if (shortDescription.Length > (int)Metrics.MaxDescriptionLenght)
                return Constants.ErrorMessages.MaxDescription;

            if (longDescription.Length > (int)Metrics.MaxLongDescriptionLenght)
                return Constants.ErrorMessages.MaxLongDescription;
            
            return "";
        }
    }
}