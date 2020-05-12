using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Auction
{
    public class AuctionRequest
    {
        public decimal StartingPrice { get; set; }
        public DateTime StartDate { get; set; }
        public int DaysActive { get; set; }
        public int ProductId { get; set; }
    }
}
