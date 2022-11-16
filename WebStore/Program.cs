
using System;
using System.Globalization;
using System.IO;

namespace WebStoreProject
{
    public class Program
    {
        static string tabs;
        static string? userInputString;
        static string[] userInput;
        static WebStore store = new WebStore();
        static Product? product;
        static string cartFilePath = @"..\..\..\Data\ProductsCart.txt";
        static string storeFilePath = @"..\..\..\Data\ProductsStore.txt";



        static void Main(string[] args)
        {
            LoadProducts(storeFilePath, store.Products);
            LoadProducts(cartFilePath, store.Cart.ProductsInCart);
            bool finished = false;
            do
            {
                userInputString = Console.ReadLine();

                if (userInputString is not null)
                {
                    userInput = Words(userInputString.ToUpper());

                    switch (userInput[0])
                    {
                        case "HELP":
                        case "INFO":
                            Console.WriteLine("Commands are: HELP/INFO, STORE, CART, SELECT, ADD, REMOVE, CREATE, DELETE, EDIT");
                            break;



                        case "STORE":
                            Console.WriteLine("\n\n" + "Product" + "\t\t\t\t" + "Price" + "\t\t\t" + "Amount" + "\t\t\t" + "Category");

                            foreach (Product p in store.Products)
                            {
                                tabs = AmountOfTabs(p.Name, 4);
                                Console.WriteLine(p.Name + tabs + p.Price + "\t\t\t" + p.Amount + "\t\t\t" + p.Category);
                            }
                            break;



                        case "SELECT": // Doesn't really do anything noteworthy. Maybe add descriptions to products that can be viewed here? And reviews?
                            Console.WriteLine();

                            product = store.FindProduct(userInput[1]);
                            if (product is not null) Console.WriteLine(WebStore.ViewProduct(product));
                            else Console.WriteLine("Product does not exist");
                            break;



                        case "ADD": // add producName (int amount opt.) 
                            Console.WriteLine();

                            int amount = 1;
                            product = store.FindProduct(userInput[1]);
                            if (product is not null)
                            {
                                if (userInput.Length > 2)
                                {
                                    int parseAttempt = 0;
                                    if (int.TryParse(userInput[2], out parseAttempt))
                                    {
                                        amount = parseAttempt;
                                    }
                                }
                                Product productInCart = store.Cart.FindProduct(userInput[1]);
                                if (productInCart is not null) productInCart.Amount += amount;
                                else store.Cart.AddProduct(new Product(product.Name, product.Price, amount, product.Category));
                                Console.WriteLine(product.Name + " is added to your shopping cart.");
                                SaveProducts(cartFilePath, store.Cart.ProductsInCart);
                            }
                            else Console.WriteLine("Product does not exist in the store.");
                            break;



                        case "REMOVE": // Need to add argument so that you can control how many you remove
                            Console.WriteLine();

                            product = store.Cart.FindProduct(userInput[1]);
                            if (product is not null)
                            {
                                store.Cart.RemoveProduct(product);
                                Console.WriteLine(product.Name + " has been removed from your shopping cart.");
                                SaveProducts(cartFilePath, store.Cart.ProductsInCart);
                            }
                            else Console.WriteLine("Product is not in your shopping cart.");
                            break;



                        case "CREATE":

                            if (userInput.Length > 2)
                            {
                                product = CreateProduct(WithoutFirstElement(userInput));
                                store.AddProductToStore(product);
                                Console.WriteLine(product.Name + " has been added for sale.");
                                SaveProducts(storeFilePath, store.Products);
                            }
                            else Console.WriteLine("Name and price arguments are required, additional amount and category arguments are optional)");
                            break;



                        case "DELETE":
                            product = store.FindProduct(userInput[1]);
                            if (product is not null)
                            {
                                store.RemoveProduct(product);
                                Console.WriteLine(product.Name + " has been removed from the store.");
                                SaveProducts(storeFilePath, store.Products);
                            }
                            else Console.WriteLine("Product not found");
                            break;



                        case "EDIT": // Maybe this should just be called edit, and let you edit the fields of product-objects?
                            break;



                        case "CART":
                            Console.WriteLine("\n\n" + "Product" + "\t\t\t\t" + "Price" + "\t\t\t" + "Amount" + "\t\t\t" + "Category");

                            foreach (Product p in store.Cart.ProductsInCart)
                            {
                                tabs = AmountOfTabs(p.Name, 4);
                                Console.WriteLine(p.Name + tabs + p.Price + "\t\t\t" + p.Amount + "\t\t\t" + p.Category);
                            }
                            Console.WriteLine("\n" + "Total: \t\t\t\t" + store.Cart.Total);
                            break;



                        case "":
                            Console.WriteLine();
                            break;



                        default:
                            Console.WriteLine("Commands are: HELP/INFO, STORE, CART, SELECT, ADD, REMOVE, CREATE, DELETE, EDIT");
                            break;
                    }
                }
                else Console.WriteLine();

            } while (!finished);

        }

        public static void SaveProducts(string pathToFile, List<Product> products)
        {
            using (StreamWriter writer = new StreamWriter(pathToFile))
            {
                foreach (Product p in products)
                {
                    writer.WriteLine($"{p.Name};{p.Price};{p.Amount};{p.Category}" + "~");
                }
            }
        }

        public static void LoadProducts(string path, List<Product> products) // Bruh do I need out here? Arents list reference types anyway?
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string? fileString = reader.ReadToEnd();
                string[] textFileString = fileString.Split("~"); // Decide split char
                string[] productInfo;

                if (textFileString[0] != "")
                {
                    foreach (string productString in textFileString)
                    {
                        productInfo = productString.Split(";"); // Decide split char
                        products.Add(new Product(productInfo[0], Decimal.Parse(productInfo[1]), int.Parse(productInfo[2]),
                            (Enums.Categories)Enum.Parse(typeof(Enums.Categories), productInfo[3], true)));

                    }
                }
            }
        }

        /// <summary>
        /// Returns amount of tabs based on length of string. Longer strings need fewer tabs. Shorter string need more.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="tabsAmount"></param>
        /// <returns></returns>
        public static string AmountOfTabs(string s, int tabsAmount)
        {
            int eidghts = s.Length / 8;

            string tabs = "";
            int targetTabsAmount = tabsAmount - eidghts;

            for (int i = 0; i < targetTabsAmount; i++)
            {
                tabs += "\t";
            }

            return tabs;
        }

        public static Product CreateProduct(string[] args)
        {
            switch (args.Length)
            {
                case 0:
                case 1:
                    return new Product("Error", 1m, 1, Enums.Categories.Other);
                case 2:
                    return new Product(args[0], Decimal.Parse(args[1]), 1, Enums.Categories.Other);
                case 3:
                    return new Product(args[0], Decimal.Parse(args[1]), Int32.Parse(args[2]), Enums.Categories.Other); // How to check if args[2] can be properly parsed?
                default:
                    return new Product(args[0], Decimal.Parse(args[1]), Int32.Parse(args[2]),
                        (Enums.Categories)Enum.Parse(typeof(Enums.Categories), args[3], true));// How to Parse enum properly? // How to check if args[2] can be properly parsed?
            }
        }

        public static string[] WithoutFirstElement(string[] array)
        {
            string[] newArray = new string[array.Length - 1];

            for (int i = 0; i < newArray.Length; i++)
            {
                newArray[i] = array[i + 1];
            }
            return newArray;
        }

        /// <summary>
        /// Returns array of contents of paramater string split by space " " but kept by ()
        /// </summary>
        /// <param name="text"></param>
        public static string[] Words(string text)
        {
            List<string> sequences = new List<string>();
            string startingSplitChars = " ('\"";
            string endCharSplit = " )'\"";
            char startChar = ' ';
            char endChar = ' ';

            for (int i = 0; i < text.Length; i++)
            {
                switch (text[i])
                {
                    case '(':
                        startChar = '(';
                        endChar = ')';
                        break;

                    case '"':
                        startChar = '"';
                        endChar = '"';
                        break;

                    case '\'':
                        startChar = '\'';
                        endChar = '\'';
                        break;

                    case ' ':
                        if (startingSplitChars.Contains(text[i + 1]))
                        {
                            continue;
                        }
                        else
                        {
                            startChar = ' ';
                            endChar = ' ';
                            break;
                        }

                    default:
                        break;
                }

                if (text[i] == startChar || i is 0)
                {
                    int indexStart = i;
                    int indexEnd = 0;

                    for (int j = i + 1; j < text.Length; j++) // Search for ending split-char
                    {
                        if (text[j] == endChar)
                        {
                            indexEnd = j;
                            i = j - 1; // Don't need to search for next start split-char until after this word
                            break;
                        }
                        else if (j == text.Length - 1)
                        {
                            indexEnd = j;
                        }
                    }
                    if (indexEnd is not 0)
                    {
                        if (indexEnd != (text.Length - 1))
                        {
                            sequences.Add(text.Substring(indexStart + 1, indexEnd - indexStart - 1));
                        }
                        else
                        {
                            if (indexStart is 0) // if first and last word
                            {
                                sequences.Add(text.Substring(indexStart, indexEnd - indexStart));
                            }
                            else if(endCharSplit.Contains(text[indexEnd])) // if last but not first word
                            {
                                sequences.Add(text.Substring(indexStart + 1, indexEnd - indexStart - 1));
                            }
                            else
                            {
                                sequences.Add(text.Substring(indexStart + 1, indexEnd - indexStart));
                            }
                        }
                    }
                }
            }
            return sequences.ToArray();
        }
    }
}