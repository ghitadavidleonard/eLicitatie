using Common.ProductType;
using eLicitatie.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eLicitatie.Web.Extensions.Mappers
{
    public static class Categories
    {
        public static CategoryModel MapToViewModel(this CategoryResponse response)
        {
            if (response is null) throw new ArgumentNullException(nameof(response));

            return new CategoryModel
            {
                Id = response.Id,
                Name = response.Name
            };
        }

        public static IEnumerable<CategoryModel> MapToViewModel(this IEnumerable<CategoryResponse> responses)
        {
            if (responses is null) throw new ArgumentNullException(nameof(responses));

            var viewModels = new List<CategoryModel>();

            foreach (var item in responses)
            {
                viewModels.Add(item.MapToViewModel());
            }

            return viewModels;
        }

        public static CategoryFullRequest MapToUpdateRequest(this CategoryModel model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));

            return new CategoryFullRequest
            {
                Name = model.Name,
                Id = model.Id
            };
        }
    }
}
