using Common.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Offer
{
    public class OfferResponse
    {
        public int Id { get; set; }
        public int AuctionId { get; set; }
        public decimal Price { get; set; }
        public UserResponse User { get; set; }
    }
}
