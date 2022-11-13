using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStoreProject
{
    public class Product
    {
        public decimal Price { get; set; }

        public string Name { get; set; }

        public int Amount { get; set; }

        public Enums.Categories Category { get; set; }

        public Product(string name, decimal price, int amount, Enums.Categories category)
        {
            Name = name;
            Price = price;
            Amount = amount;
            Category = category;
        }
    }
}
