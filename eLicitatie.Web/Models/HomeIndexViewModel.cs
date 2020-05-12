using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eLicitatie.Web.Models
{
    public class HomeIndexViewModel
    {
        public IEnumerable<CategoryModel> Categories { get; set; }
        public IEnumerable<AuctionViewModel> Auctions { get; set; }
    }
}
