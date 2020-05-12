using eLicitatie.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace eLicitatie.Web.Controllers
{
    public class OffersController : BaseController
    {
        public OffersController(IConfiguration configuration) : base(configuration)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Create(OfferViewModel model)
        {
            HttpResponseMessage response = await PostAsync("offers", model);

            if (!response.IsSuccessStatusCode) TempData["OfferErrorMessage"] = await response.Content.ReadAsStringAsync();

            return RedirectToAction("Details", "Auctions", new { id = model.AuctionId });
        }
    }
}
