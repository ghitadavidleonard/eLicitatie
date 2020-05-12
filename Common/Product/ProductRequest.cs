using Common.ProductType;
using System.Collections.Generic;

namespace Common.Product
{
    public class ProductRequest
    {
        public List<CategoryFullRequest> Categories { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public byte[] Image { get; set; }

        public ProductRequest()
        {
            Categories = new List<CategoryFullRequest>();
        }
    }
}