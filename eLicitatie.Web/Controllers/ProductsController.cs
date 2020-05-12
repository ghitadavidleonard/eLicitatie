using Common;
using Common.Product;
using Common.ProductType;
using eLicitatie.Web.Extensions;
using eLicitatie.Web.Extensions.Mappers;
using eLicitatie.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eLicitatie.Web.Controllers
{
    public class ProductsController : BaseController
    {
        public ProductsController(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await GetAsync($"products/forUser/{User.FindFirst(ClaimTypes.NameIdentifier)?.Value}");

            if (!response.IsSuccessStatusCode) TempData["ProductErrorMessage"] = Constants.ErrorMessages.DataCouldntBeRetrieved;

            HttpResponseMessage categoriesResponse = await GetAsync("categories");

            if (!categoriesResponse.IsSuccessStatusCode) TempData["CategoriesErrorMessage"] = await categoriesResponse.Content.ReadAsStringAsync();

            var products = await response.ContentAsTypeAsync<IEnumerable<ProductResponse>>();
            var categories = await categoriesResponse.ContentAsTypeAsync<IEnumerable<CategoryResponse>>();

            var vm = new ProductIndexViewModel
            {
                Products = products.MapToViewModel(),
                Categories = categories.MapToViewModel()
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            HttpResponseMessage response = await GetAsync("categories");

            if (!response.IsSuccessStatusCode) TempData["ProductErrorMessage"] = await response.Content.ReadAsStringAsync();

            var categories = await response.ContentAsTypeAsync<IEnumerable<CategoryResponse>>();

            var addProduct = new AddProductViewModel
            {
                Categories = categories.MapToViewModel()
            };

            return View(addProduct);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductCreateViewModel model)
        {
            var productRequest = model.MapToRequest();

            HttpResponseMessage response = await PostAsync("products", productRequest);

            if (!response.IsSuccessStatusCode) TempData["ProductErrorMessage"] = await response.Content.ReadAsStringAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            HttpResponseMessage response = await DeleteAsync($"products/delete/{id}");

            if (!response.IsSuccessStatusCode) TempData["ProductErrorMessage"] = await response.Content.ReadAsStringAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProduct(int productId)
        {
            HttpResponseMessage responseMessage = await GetAsync($"products/{productId}");

            if (!responseMessage.IsSuccessStatusCode)
            {
                TempData["ProductErrorMessage"] = await responseMessage.Content.ReadAsStringAsync();
                return RedirectToAction(nameof(Index));
            }
            
            var product = await responseMessage.ContentAsTypeAsync<ProductViewModel>();

            HttpResponseMessage response = await GetAsync("categories");

            if (!response.IsSuccessStatusCode)
            {
                TempData["ProductErrorMessage"] = await response.Content.ReadAsStringAsync();
                return RedirectToAction(nameof(Index));
            }

            var categories = await response.ContentAsTypeAsync<IEnumerable<CategoryResponse>>();

            var updateProduct = new AddProductViewModel
            {
                Product = product,
                Categories = categories.MapToViewModel()
            };

            return View(updateProduct);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(UpdateProductModel model)
        {
            var productRequest = model.MapToFullRequest();

            HttpResponseMessage response = await PutAsync("products", productRequest);

            if (!response.IsSuccessStatusCode) TempData["ProductErrorMessage"] = await response.Content.ReadAsStringAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int productId)
        {
            HttpResponseMessage response = await GetAsync($"products/{productId}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["ProductErrorMessage"] = await response.Content.ReadAsStringAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(await response.ContentAsTypeAsync<ProductViewModel>());
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsByCategory(int[] productCategories)
        {
            HttpResponseMessage productResponse = await GetAsync($"products?categories={string.Join(",", productCategories)}");

            if (!productResponse.IsSuccessStatusCode)
            {
                TempData["ProductErrorMessage"] = await productResponse.Content.ReadAsStringAsync();
                return RedirectToAction(nameof(Index));
            }

            HttpResponseMessage categoriesResponse = await GetAsync("categories");

            if (!categoriesResponse.IsSuccessStatusCode)
            {
                TempData["CategoriesErrorMessage"] = await productResponse.Content.ReadAsStringAsync();
                return RedirectToAction(nameof(Index));
            }

            var products = await productResponse.ContentAsTypeAsync<IEnumerable<ProductResponse>>();
            var categories = await categoriesResponse.ContentAsTypeAsync<IEnumerable<CategoryResponse>>();

            var vm = new ProductIndexViewModel
            {
                Products = products.MapToViewModel(),
                Categories = categories.MapToViewModel()
            };

            return View(nameof(Index), vm);
        }

        [HttpGet]
        public async Task<IActionResult> Image(int productId)
        {
            HttpResponseMessage response = await GetAsync($"products/image/{productId}");
            
            if (!response.IsSuccessStatusCode) TempData["ProductErrorMessage"] = await response.Content.ReadAsStringAsync();

            using var ms = new MemoryStream();
            var file = await response.Content.ReadAsStreamAsync();
            await file.CopyToAsync(ms);

            return File(ms.ToArray(), "application/octet-stream");
        }
    }
}
