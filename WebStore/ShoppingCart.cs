using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStoreProject
{
    public  class ShoppingCart
    {
        decimal _total;
        public List<Product> ProductsInCart = new List<Product>();

        public decimal Total
        {
            get
            {
                foreach (Product p in ProductsInCart)
                {
                    _total += p.Price;
                }
                return _total;
            }
        }

        public void AddProduct(Product product)
        {
            ProductsInCart.Add(product);
        }

        public void RemoveProduct(Product product)
        {
            ProductsInCart.Remove(product);
        }

        public Product FindProduct(String desiredProductName)
        {
            return ProductsInCart.Find(x => ExtentionMethods.ContainsCaseInsensitive(desiredProductName, x.Name));
        }
    }
}
