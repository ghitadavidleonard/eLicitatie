using Common.Auction;
using eLicitatie.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eLicitatie.Web.Extensions.Mappers
{
    public static class Auctions
    {
        public static AuctionViewModel MapToViewModel(this AuctionResponse response)
        {
            if (response is null) throw new ArgumentNullException(nameof(response));

            return new AuctionViewModel
            {
                Id = response.Id,
                StartingPrice = response.StartingPrice,
                StartDate = response.StartDate,
                DaysActive = response.DaysActive,
                Product = response.Product?.MapToViewModel(),
                Creator = response.Creator?.MapToViewModel()
            };
        }

        public static IEnumerable<AuctionViewModel> MapToViewModel(this IEnumerable<AuctionResponse> responses)
        {
            if (responses is null) throw new ArgumentNullException(nameof(responses));

            var auctions = new List<AuctionViewModel>();

            foreach ( var item in responses)
            {
                auctions.Add(item.MapToViewModel());
            }

            return auctions;
        }

        public static AuctionRequest MapToRequest(this AuctionCreateViewModel model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));

            return new AuctionRequest
            {
                DaysActive = model.DaysActive,
                ProductId = model.ExistentProductId,
                StartDate = model.StartDate,
                StartingPrice = model.StartingPrice
            };
        }
    }
}
