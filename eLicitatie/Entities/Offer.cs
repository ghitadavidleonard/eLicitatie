using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eLicitatie.Api.Entities
{
    public class Offer
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int AuctionId { get; set; }
        public Auction Auction { get; set; }
        public decimal Price { get; set; }
    }
}
