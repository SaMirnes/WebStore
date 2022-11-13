using WebStoreProject;

namespace WebStoreProject
{
    public class WebStore
    {
        public List<Product> Products { get; set; }
        public ShoppingCart Cart { get; set; }

        public WebStore()
        {
            Products = new List<Product>();
            Cart = new ShoppingCart();
        }

        public void AddProductToStore(Product product)
        {
            Products.Add(product);
        }

        public void AddProductToCart(Product product)
        {
            Cart.AddProduct(product);
        }

        public static string ViewProduct(Product product)
        {
            return $"Name: {product.Name} \t\t Price: {product.Price} \t\t Amount in store: {product.Amount} \t\t Category: {product.Category}";
        }

        public Product FindProduct(String desiredProductName)
        {
            return Products.Find(x => ExtentionMethods.ContainsCaseInsensitive(desiredProductName, x.Name));
        }

        public void RemoveProduct(Product product)
        {
            Products.Remove(product);
        }
    }
}
