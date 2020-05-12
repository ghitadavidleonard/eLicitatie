using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eLicitatie.Web.Models
{
    public class AuctionViewModel
    {
        public int Id { get; set; }
        public decimal StartingPrice { get; set; }
        public DateTime StartDate { get; set; }
        public int DaysActive { get; set; }
        public UserModel Creator { get; set; }
        public ProductViewModel Product { get; set; }
        public OfferViewModel Offer { get; set; }
        public IEnumerable<CategoryModel> ProductCategories { get; set; }
        public IEnumerable<OfferViewModel> Offers { get; set; }
        public int RemainingDays => (StartDate.AddDays(DaysActive) - DateTime.UtcNow).Days;
        public bool LastDay { get => (RemainingDays == 0); }
        public AuctionViewModel()
        {
            ProductCategories = new List<CategoryModel>();
            Offers = new List<OfferViewModel>();
        }
    }
}
