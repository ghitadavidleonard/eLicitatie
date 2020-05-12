using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eLicitatie.Api.Entities
{
    public class Auction
    {
        public int Id { get; set; }
        public decimal StartingPrice { get; set; }
        public DateTime StartDate { get; set; }
        public int DaysActive { get; set; }
        public int CreatorId { get; set; }
        public User Creator { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public ICollection<Offer> Offers { get; set; }

        public Auction()
        {
            Offers = new List<Offer>();
        }
    }
}
