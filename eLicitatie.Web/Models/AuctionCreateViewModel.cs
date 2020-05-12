using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eLicitatie.Web.Models
{
    public class AuctionCreateViewModel
    {
        public decimal StartingPrice { get; set; }
        public DateTime StartDate { get; set; }
        public int DaysActive { get; set; }
        public int CreatorId { get; set; }
        public int ProductId { get; set; }

        public ProductCreateViewModel NewProduct { get; set; }
        public IEnumerable<CategoryModel> Categories { get; set; }
        public int ExistentProductId { get; set; }
        public IEnumerable<ProductViewModel> ExistentProducts { get; set; }
        
        public AuctionCreateViewModel()
        {
            ExistentProducts = new List<ProductViewModel>();
            Categories = new List<CategoryModel>();
            StartDate = DateTime.Now;
        }
    }
}
