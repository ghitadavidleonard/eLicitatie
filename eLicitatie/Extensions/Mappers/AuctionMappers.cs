using Common.Auction;
using Common.Offer;
using eLicitatie.Api.Entities;
using System;
using System.Collections.Generic;

namespace eLicitatie.Api.Extensions.Mappers
{
    public static class AuctionMappers
    {
        public static Auction MapToEntity(this AuctionRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            return new Auction
            {
                StartingPrice = request.StartingPrice,
                StartDate = request.StartDate,
                DaysActive = request.DaysActive,
                ProductId = request.ProductId
            };
        }

        public static AuctionResponse MapToResponse(this Auction auction)
        {
            if (auction is null) throw new ArgumentNullException(nameof(auction));

            return new AuctionResponse
            {
                Id = auction.Id,
                DaysActive = auction.DaysActive,
                StartDate = auction.StartDate,
                StartingPrice = auction.StartingPrice,
                Product = auction.Product.MapToResponse(),
                Creator = auction.Creator.MapToUserResponse(),
                Offers = auction.Offers.MapToResponse()
            };
        }
    }
}