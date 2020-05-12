using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eLicitatie.Web.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string ImageUrl { get; set; }
        public UserModel Owner { get; set; }
        public List<CategoryModel> Categories { get; set; }

        public ProductViewModel()
        {
            Categories = new List<CategoryModel>();
        }
    }
}
