using Common.ProductType;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Product
{
    public class ProductFullRequest
    {
        public ICollection<CategoryFullRequest> Categories { get; set; }
        public string LongDescription { get; set; }
        public string ShortDescription { get; set; }
        public byte[] Image { get; set; }
        public int Id { get; set; }
        public ProductFullRequest()
        {
            Categories = new List<CategoryFullRequest>();
        }
    }
}
