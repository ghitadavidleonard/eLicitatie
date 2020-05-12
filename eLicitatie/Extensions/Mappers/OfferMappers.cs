using Common.Offer;
using eLicitatie.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eLicitatie.Api.Extensions.Mappers
{
    public static class OfferMappers
    {
        public static OfferResponse MapToResponse(this Offer offer)
        {
            if (offer is null) throw new ArgumentNullException(nameof(offer));

            return new OfferResponse
            {
                Id = offer.Id,
                AuctionId = offer.AuctionId,
                Price = offer.Price,
                User = offer.User.MapToUserResponse()
            };
        }
        public static IEnumerable<OfferResponse> MapToResponse(this IEnumerable<Offer> offers)
        {
            if (offers is null) throw new ArgumentNullException(nameof(offers));

            var offersList = new List<OfferResponse>();

            foreach (var item in offers)
            {
                offersList.Add(item.MapToResponse());
            }

            return offersList;
        }

        public static Offer MapToEntity(this OfferRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            return new Offer
            {
                AuctionId = request.AuctionId,
                Price = request.Price
            };
        }
    }
}
