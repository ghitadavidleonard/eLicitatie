using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eLicitatie.Web.Models
{
    public class ProductIndexViewModel
    {
        public IEnumerable<ProductViewModel> Products { get; set; }
        public IEnumerable<CategoryModel> Categories { get; set; }
    }
}
