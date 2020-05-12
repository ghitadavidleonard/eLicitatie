using Common.Auction;
using Common.Product;
using Common.ProductType;
using eLicitatie.Web.Extensions;
using eLicitatie.Web.Extensions.Mappers;
using eLicitatie.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eLicitatie.Web.Controllers
{
    public class AuctionsController : BaseController
    {
        public AuctionsController(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<IActionResult> Index()
        {
            HttpResponseMessage auctionsResponse = await GetAsync("auctions");

            if (!auctionsResponse.IsSuccessStatusCode) TempData["AuctionErrorMessage"] = await auctionsResponse.Content.ReadAsStringAsync();

            var auctions = await auctionsResponse.ContentAsTypeAsync<IEnumerable<AuctionResponse>>();

            return View(auctions.MapToViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            HttpResponseMessage existentProductsResponse = await GetAsync($"products/forUser/{User.FindFirst(ClaimTypes.NameIdentifier)?.Value}");
            HttpResponseMessage categoriesResponse = await GetAsync("categories");


            if (!existentProductsResponse.IsSuccessStatusCode)
            {
                TempData["AuctionErrorMessage"] = await existentProductsResponse.Content.ReadAsStringAsync();
                return RedirectToAction(nameof(Index));
            }
            if (!categoriesResponse.IsSuccessStatusCode)
            {
                TempData["CategoriesErrorMessage"] = await categoriesResponse.Content.ReadAsStringAsync();
                return RedirectToAction(nameof(Index));
            }

            var existentProducts = await existentProductsResponse.ContentAsTypeAsync<IEnumerable<ProductResponse>>();
            var categories = await categoriesResponse.ContentAsTypeAsync<IEnumerable<CategoryResponse>>();

            var vm = new AuctionCreateViewModel {
                Categories = categories.MapToViewModel(),
                ExistentProducts = existentProducts.MapToViewModel()
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AuctionCreateViewModel model)
        {
            if (model.ExistentProductId == 0)
            {
                HttpResponseMessage newProductResponse = await PostAsync("products", model.NewProduct.MapToRequest());
                if (!newProductResponse.IsSuccessStatusCode) 
                {
                    TempData["AuctionErrorMessage"] = await newProductResponse.Content.ReadAsStringAsync();
                    return RedirectToAction(nameof(Create));
                }

                var existentProduct = await newProductResponse.ContentAsTypeAsync<ProductResponse>();
                model.ExistentProductId = existentProduct.Id;
            }
            
            HttpResponseMessage reponse = await PostAsync("auctions", model.MapToRequest());

            if (!reponse.IsSuccessStatusCode)
            {
                TempData["AuctionErrorMessage"] = await reponse.Content.ReadAsStringAsync();
                return RedirectToAction(nameof(Create));
            }

            var auction = await reponse.ContentAsTypeAsync<AuctionResponse>();

            return RedirectToAction(nameof(Details), new { id = auction.Id });
        }

        public async Task<IActionResult> Delete(int id)
        {
            HttpResponseMessage responseMessage = await DeleteAsync($"auctions/delete/{id}");

            if (!responseMessage.IsSuccessStatusCode) 
                TempData["AuctionErrorMessage"] = await responseMessage.Content.ReadAsStringAsync();

            return RedirectToAction(nameof(Index), "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            HttpResponseMessage response = await GetAsync($"auctions/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["AuctionErrorMessage"] = await response.Content.ReadAsStringAsync();
                return RedirectToAction("Index", "Home");
            }

            var vm = await response.ContentAsTypeAsync<AuctionViewModel>();
            return View(vm);
        }
    }
}