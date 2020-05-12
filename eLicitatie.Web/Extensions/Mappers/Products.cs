using Common;
using Common.Product;
using Common.ProductType;
using eLicitatie.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLicitatie.Web.Extensions.Mappers
{
    public static class Products
    {
        public static ProductViewModel MapToViewModel(this ProductResponse response)
        {
            if (response is null) throw new ArgumentNullException(nameof(response));

            var viewModel = new ProductViewModel
            {
                Id = response.Id,
                ShortDescription = response.ShortDescription,
                LongDescription = response.LongDescription,
                Owner = new UserModel
                {
                    Id = response.Id
                },
                ImageUrl = response.ImageName
            };

            foreach (var categ in response.Categories)
            {
                viewModel.Categories.Add(categ.MapToViewModel());
            }

            return viewModel;
        }

        public static IEnumerable<ProductViewModel> MapToViewModel(this IEnumerable<ProductResponse> responses)
        {
            if (responses is null) throw new ArgumentNullException(nameof(responses));

            var viewModels = new List<ProductViewModel>();

            foreach (var item in responses)
            {
                viewModels.Add(item.MapToViewModel());
            }

            return viewModels;
        }

        public static ProductRequest MapToRequest(this ProductCreateViewModel model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));

            var productRequest = new ProductRequest
            {
                ShortDescription = model.ShortDescription,
                LongDescription = model.LongDescription
            };

            if (model.Image != null)
            {
                using var ms = new MemoryStream();
                model.Image.CopyTo(ms);
                var fileBytes = ms.ToArray();
                productRequest.Image = fileBytes;
            }
            else
            {
                productRequest.Image = Convert.FromBase64String(Constants.Defaults.Image);
            }

            foreach (var item in model.Categories)
            {
                productRequest.Categories.Add(new CategoryFullRequest { 
                   Id = item 
                });
            }

            return productRequest;
        }

        public static ProductFullRequest MapToFullRequest(this UpdateProductModel model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));

            var productRequest = new ProductFullRequest
            {
                Id = model.Id,
                ShortDescription = model.ShortDescription,
                LongDescription = model.LongDescription
            };

            if (model.Image != null)
            {
                using var ms = new MemoryStream();
                model.Image.CopyTo(ms);
                var fileBytes = ms.ToArray();
                productRequest.Image = fileBytes;
            }
            else
            {
                productRequest.Image = Convert.FromBase64String(Constants.Defaults.Image);
            }

            foreach (var item in model.Categories)
            {
                productRequest.Categories.Add(new CategoryFullRequest
                {
                    Id = item
                });
            }

            return productRequest;
        }
    }
}
