using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eLicitatie.Web.Models
{
    public class CategoryViewModel
    {
        public IEnumerable<CategoryModel> Categories { get; set; }
        public CategoryModel Category { get; set; }
    }
}
