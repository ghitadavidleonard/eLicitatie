using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eLicitatie.Web.Models
{
    public class OfferViewModel
    {
        public int Id { get; set; }
        public UserModel User { get; set; }
        public int AuctionId { get; set; }
        public decimal Price { get; set; }
    }
}
