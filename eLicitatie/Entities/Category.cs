using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace eLicitatie.Api.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<ProductCategory> ProductCategories { get; set; }

        public Category()
        {
            ProductCategories = new List<ProductCategory>();
        }
    }
}
