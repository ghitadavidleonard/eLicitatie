using Common.Auction;
using Common.ProductType;
using eLicitatie.Web.Extensions;
using eLicitatie.Web.Extensions.Mappers;
using eLicitatie.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eLicitatie.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<IActionResult> Index()
        {
            HttpResponseMessage auctionsResponse = await GetAsync("auctions");
            HttpResponseMessage categoriesResponse = await GetAsync("categories");

            if (!auctionsResponse.IsSuccessStatusCode) TempData["AuctionErrorMessage"] = await auctionsResponse.Content.ReadAsStringAsync();
            if (!categoriesResponse.IsSuccessStatusCode) TempData["CategoriesErrorMessage"] = await categoriesResponse.Content.ReadAsStringAsync();

            var auctions = await auctionsResponse.ContentAsTypeAsync<IEnumerable<AuctionResponse>>();
            var categories = await categoriesResponse.ContentAsTypeAsync<IEnumerable<CategoryResponse>>();

            var vm = new HomeIndexViewModel
            {
                Auctions = auctions.Where(a => a.Creator.Id == Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value)).MapToViewModel(),
                Categories = categories.MapToViewModel()
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Auctions(int[] productCategories)
        {
            HttpResponseMessage auctionsResponse = await GetAsync($"auctions?categories={string.Join(",", productCategories)}");
            HttpResponseMessage categoriesResponse = await GetAsync("categories");

            if (!auctionsResponse.IsSuccessStatusCode)
            {
                TempData["AuctionErrorMessage"] = await auctionsResponse.Content.ReadAsStringAsync();
                return RedirectToAction(nameof(Index));
            }

            if (!categoriesResponse.IsSuccessStatusCode)
            {
                TempData["CategoriesErrorMessage"] = await categoriesResponse.Content.ReadAsStringAsync();
                return RedirectToAction(nameof(Index));
            }

            var auctions = await auctionsResponse.ContentAsTypeAsync<IEnumerable<AuctionResponse>>();
            var categories = await categoriesResponse.ContentAsTypeAsync<IEnumerable<CategoryResponse>>();

            var vm = new HomeIndexViewModel
            {
                Auctions = auctions.MapToViewModel(),
                Categories = categories.MapToViewModel()
            };
            return View(nameof(Index), vm);
        }
    }
}