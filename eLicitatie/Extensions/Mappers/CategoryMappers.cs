using Common.ProductType;
using eLicitatie.Api.Entities;
using System;

namespace eLicitatie.Api.Extensions.Mappers
{
    public static class CategoryMappers
    {
        public static Category MapToEntity(this CategoryRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            return new Category
            {
                Name = request.Name
            };
        }

        public static Category MapToEntity(this CategoryFullRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            return new Category
            {
                Id = request.Id,
                Name = request.Name
            };
        }

        public static CategoryResponse MapToResponse(this Category productType)
        {
            if (productType is null) throw new ArgumentNullException(nameof(productType));

            return new CategoryResponse
            {
                Id = productType.Id,
                Name = productType.Name
            };
        }
    }
}