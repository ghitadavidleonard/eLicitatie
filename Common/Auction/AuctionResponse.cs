using Common.Authentication;
using Common.Offer;
using Common.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Auction
{
    public class AuctionResponse
    {
        public int Id { get; set; }
        public decimal StartingPrice { get; set; }
        public DateTime StartDate { get; set; }
        public int DaysActive { get; set; }
        public UserResponse Creator { get; set; }
        public ProductResponse Product { get; set; }

        public IEnumerable<OfferResponse> Offers { get; set; }

        public AuctionResponse()
        {
            Offers = new List<OfferResponse>();
        }
    }
}
