using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace eLicitatie.Web.Models
{
    public class UpdateProductModel
    {
        public int Id { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public IFormFile Image { get; set; }
        public List<int> Categories { get; set; }

        public UpdateProductModel()
        {
            Categories = new List<int>();
        }
    }
}