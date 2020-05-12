using Common.ProductType;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Product
{
    public class ProductResponse
    {
        public ICollection<CategoryResponse> Categories { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string ImageName { get; set; }

        public ProductResponse()
        {
            Categories = new List<CategoryResponse>();
        }
    }
}
