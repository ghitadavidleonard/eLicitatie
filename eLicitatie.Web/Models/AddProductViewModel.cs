using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eLicitatie.Web.Models
{
    public class AddProductViewModel
    {
        public ProductViewModel Product { get; set; }
        public IEnumerable<CategoryModel> Categories { get; set; }
    }
}
