using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eLicitatie.Web.Models
{
    public class ProductCreateViewModel
    {
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public IFormFile Image { get; set; }
        public List<int> Categories { get; set; }

        public ProductCreateViewModel()
        {
            Categories = new List<int>();
        }
    }
}
