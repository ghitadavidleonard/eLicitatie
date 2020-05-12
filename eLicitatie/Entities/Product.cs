using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace eLicitatie.Api.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public byte[] Image { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }

        public ICollection<ProductCategory> ProductCategories { get; set; }

        public Product()
        {
            ProductCategories = new List<ProductCategory>();
        }
    }
}
