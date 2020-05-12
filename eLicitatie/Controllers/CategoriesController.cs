using Common;
using Common.ProductType;
using eLicitatie.Api.Entities;
using eLicitatie.Api.Extensions;
using eLicitatie.Api.Extensions.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class Categories : ControllerBase
    {
        private readonly eLicitatieDbContext _context;

        public Categories(eLicitatieDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "0")]
        [HttpPost("api/categories")]
        public async Task<IActionResult> AddCategory([FromBody]CategoryRequest request)
        {
            if (request == null) return UnprocessableEntity(Constants.ErrorMessages.UnprocessableEntity);

            if (string.IsNullOrWhiteSpace(request.Name)) return BadRequest(Constants.ErrorMessages.EmptyProductTypeName);

            if (_context.Categories.Any(pt => pt.Name == request.Name)) return BadRequest(Constants.ErrorMessages.ProductTypeAlreadyExists);

            Category productType = request.MapToEntity();

            var entry = _context.Categories.Add(productType);

            try { await _context.SaveChangesAsync(); }
            catch (Exception) { return this.InternalServerError(); }

            var response = entry.Entity.MapToResponse();

            return Ok(response);
        }

        [Authorize(Roles = "0")]
        [HttpPut("api/categories")]
        public async Task<IActionResult> UpdateProductType([FromBody]CategoryFullRequest request)
        {
            if (request is null) return UnprocessableEntity(Constants.ErrorMessages.UnprocessableEntity);

            if (string.IsNullOrEmpty(request.Name)) return BadRequest(Constants.ErrorMessages.EmptyProductTypeName);

            if (_context.Categories.Any(pt => pt.Name == request.Name)) return BadRequest(Constants.ErrorMessages.ProductTypeAlreadyExists);

            Category productType = null;
            try { productType = await _context.Categories.SingleOrDefaultAsync(pt => pt.Id == request.Id); }
            catch (Exception) { return this.InternalServerError(); }

            if (productType is null) return NotFound(Constants.ErrorMessages.NotFound);

            productType.Name = request.Name;
            var entry = _context.Categories.Update(productType);

            try { await _context.SaveChangesAsync(); }
            catch (Exception) { return this.InternalServerError(); }

            var response = entry.Entity.MapToResponse();

            return Ok(response);
        }

        [Authorize(Roles = "0")]
        [HttpDelete("api/categories/{id:int}")]
        public async Task<IActionResult> DeleteProductType(int id)
        {
            if (id <= 0)
                return BadRequest(Constants.ErrorMessages.UnhandledRequest);

            Category productType = await _context.Categories.SingleOrDefaultAsync(pt => pt.Id == id);

            if (productType == null)
                return NotFound(Constants.ErrorMessages.NotFound);

            _context.Categories.Remove(productType);

            try { await _context.SaveChangesAsync(); }
            catch (Exception) { return this.InternalServerError(); }

            return Ok(productType);
        }

        [HttpGet("api/categories")]
        public IEnumerable<CategoryResponse> GetProductTypes()
        {
            var productTypes = new List<CategoryResponse>();

            foreach (var item in _context.Categories.ToList())
            {
                productTypes.Add(item.MapToResponse());
            }

            return productTypes;
        }
    }
}