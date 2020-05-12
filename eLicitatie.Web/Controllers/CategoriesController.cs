using Common;
using Common.ProductType;
using eLicitatie.Web.Extensions;
using eLicitatie.Web.Extensions.Mappers;
using eLicitatie.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace eLicitatie.Web.Controllers
{
    [Authorize(Roles = "0")]
    public class CategoriesController : BaseController
    {
        public CategoriesController(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await GetAsync("categories");

            if (!response.IsSuccessStatusCode) TempData["CategErrorMessage"] = Constants.ErrorMessages.DataCouldntBeRetrieved;

            var categories = await response.ContentAsTypeAsync<IEnumerable<CategoryResponse>>();

            var viewModel = new CategoryViewModel
            {
                Categories = categories.MapToViewModel()
            };

            return View(viewModel);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryModel model)
        {
            HttpResponseMessage response = await PostAsync("categories", model);

            if (!response.IsSuccessStatusCode) TempData["CategErrorMessage"] = await response.Content.ReadAsStringAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            HttpResponseMessage response = await DeleteAsync($"categories/{id}");

            if (!response.IsSuccessStatusCode) TempData["CategErrorMessage"] = await response.Content.ReadAsStringAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Update(CategoryModel model)
        {
            var requst = model.MapToUpdateRequest();

            HttpResponseMessage response = await PutAsync($"categories", model);

            if (!response.IsSuccessStatusCode) TempData["CategErrorMessage"] = await response.Content.ReadAsStringAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
