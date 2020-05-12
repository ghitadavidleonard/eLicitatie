using Common.Product;
using Common.ProductType;
using eLicitatie.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eLicitatie.Api.Extensions.Mappers
{
    public static class ProductMappers
    {
        public static Product MapToEntity(this ProductRequest productRequest)
        {
            if (productRequest is null) throw new ArgumentNullException(nameof(productRequest));

            var product = new Product
            {
                ShortDescription = productRequest.ShortDescription,
                LongDescription = productRequest.LongDescription,
                Image = productRequest.Image != null ? productRequest.Image.ToArray() : null
            };

            if (productRequest.Categories != null && productRequest.Categories.Any())
            {
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

            return product;
        }

        public static ProductResponse MapToResponse(this Product product)
        {
            if (product is null) throw new ArgumentNullException(nameof(product));

            var response = new ProductResponse
            {
                Id = product.Id,
                LongDescription = product.LongDescription,
                ShortDescription = product.ShortDescription,
                OwnerId = product.OwnerId,
                ImageName = product.Id.ToString()
            };

            if (product.ProductCategories != null && product.ProductCategories.Any())
            {
                foreach (var productCateg in product.ProductCategories)
                {
                    var categoryResponse = new CategoryResponse
                    {
                        Id = productCateg.CategoryId,
                        Name = productCateg.Category?.Name
                    };
                    response.Categories.Add(categoryResponse);
                }
            }

            return response;
        }
    }
}
