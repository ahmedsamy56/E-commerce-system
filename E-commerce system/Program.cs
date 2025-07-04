using E_commerce_system.Entities.Abstracts.Classes;
using E_commerce_system.Entities.Abstracts.Interfaces;
using E_commerce_system.Entities.Implementations;
using E_commerce_system.Service;

namespace E_commerce_system
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var products = new List<Product>
            {
                new ExpireAndShippingProduct("Cheese", 100, 10, 400, DateTime.Now.AddDays(5)),
                new ExpireAndShippingProduct("Biscuits", 150, 5,  700, DateTime.Now.AddDays(2)),
                new OnlyExpireProduct("game card", 150, 50, DateTime.Now.AddDays(2)),
                new OnlyShippingProduct("TV", 5000, 3, 700),
                new NotShippingNotExpireProduct("scratch card", 50, 20)
            };

            Console.WriteLine("Login as:");
            Console.WriteLine("1. Admin");
            Console.WriteLine("2. Customer");
            Console.Write("Enter choice: ");
            var loginChoice = Console.ReadLine();

            #region Admin mode
            if (loginChoice == "1")
            {

                while (true)
                {
                    try
                    {
                        Console.WriteLine("\nAdd a new product:");
                        Console.Write("Can this product expire? (y/n): ");
                        var canExpire = Console.ReadLine()?.Trim().ToLower() == "y";
                        Console.Write("Does this product require shipping? (y/n): ");
                        var requiresShipping = Console.ReadLine()?.Trim().ToLower() == "y";

                        Console.Write("Enter product name: ");
                        var name = Console.ReadLine();
                        Console.Write("Enter price: ");
                        if (!decimal.TryParse(Console.ReadLine(), out decimal price))
                        {
                            Console.WriteLine("Invalid price Try again.");
                            continue;
                        }
                        Console.Write("Enter quantity: ");
                        if (!int.TryParse(Console.ReadLine(), out int quantity))
                        {
                            Console.WriteLine("Invalid quantity Try again.");
                            continue;
                        }

                        DateTime? expiry = null;
                        if (canExpire)
                        {
                            Console.Write("Enter expiry date (yyyy-mm-dd): ");
                            if (!DateTime.TryParse(Console.ReadLine(), out DateTime expDate))
                            {
                                Console.WriteLine("Invalid date Try again.");
                                continue;
                            }
                            expiry = expDate;
                        }

                        double? weight = null;
                        if (requiresShipping)
                        {
                            Console.Write("Enter weight by grams: ");
                            if (!double.TryParse(Console.ReadLine(), out double w) || w <= 0)
                            {
                                Console.WriteLine("Invalid weight. Try again.");
                                continue;
                            }
                            weight = w;
                        }

                        Product newProduct;
                        if (canExpire && requiresShipping)
                            newProduct = new ExpireAndShippingProduct(name, price, quantity, weight.Value, expiry.Value);
                        else if (canExpire)
                            newProduct = new OnlyExpireProduct(name, price, quantity, expiry.Value);
                        else if (requiresShipping)
                            newProduct = new OnlyShippingProduct(name, price, quantity, weight.Value);
                        else
                            newProduct = new NotShippingNotExpireProduct(name, price, quantity);

                        products.Add(newProduct);
                        Console.WriteLine($"Product '{name}' added successfully!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        Console.WriteLine("Please try again.");
                        continue;
                    }

                    Console.Write("Add another product? (y/n): ");
                    var again = Console.ReadLine();
                    if (again == null || again.Trim().ToLower() != "y")
                        break;
                }
                Console.WriteLine("Admin session ended. Restart the program to shop as a customer.");
            }
            #endregion

            #region Customer mode
            else if (loginChoice == "2")
            {
                Console.Write("Enter your name: ");
                var customerName = Console.ReadLine();
                decimal customerBalance;
                while (true)
                {
                    Console.Write("Enter your starting balance: ");
                    if (decimal.TryParse(Console.ReadLine(), out customerBalance) && customerBalance >= 0)
                        break;
                    Console.WriteLine("Invalid balance Please enter a non-negative number.");
                }

                var customer = new Customer(customerName, customerBalance);
                var checkoutService = new CheckoutService();
                while (true)
                {
                    Console.WriteLine("\n==========================");
                    Console.WriteLine("      Products");
                    Console.WriteLine("==========================");
                    Console.WriteLine($"{"No.",-4}{"Name",-15}{"Price",8}{"Stock",8}{"Expiry",14}{"Weight",8}");
                    int displayIndex = 1;
                    var availableProducts = new List<dynamic>();
                    for (int i = 0; i < products.Count; i++)
                    {
                        var p = products[i];
                        bool isExpired = p is IExpirable expirable && expirable.IsExpired();
                        if (isExpired)
                            continue;
                        string expiry = p is IExpirable exp ? exp.ExpirationDate.ToString("yyyy-MM-dd") : "";
                        string weight = p is IShippable ship ? ship.Weight + "g" : "";
                        Console.WriteLine($"{displayIndex,-4}{p.Name,-15}{p.Price,8}{p.Quantity,8}{expiry,14}{weight,8}");
                        availableProducts.Add(p);
                        displayIndex++;
                    }

                    Console.WriteLine("\n--------------------------");
                    Console.WriteLine("        Your Card");
                    Console.WriteLine("--------------------------");
                    if (customer.Cart.Items.Count == 0)
                    {
                        Console.WriteLine("(empty)");
                    }
                    else
                    {
                        Console.WriteLine($"{"No.",-4}{"Name",-15}{"Qty",6}{"Price",8}{"Subtotal",10}");
                        decimal cartSubtotal = 0;
                        for (int i = 0; i < customer.Cart.Items.Count; i++)
                        {
                            var item = customer.Cart.Items[i];
                            decimal itemSubtotal = item.Product.Price * item.Quantity;
                            cartSubtotal += itemSubtotal;
                            Console.WriteLine($"{i + 1,-4}{item.Product.Name,-15}{item.Quantity,6}{item.Product.Price,8}{itemSubtotal,10}");
                        }
                        Console.WriteLine($"\nCart subtotal: {cartSubtotal}");
                    }

                    Console.WriteLine("\nChoose an action:");
                    Console.WriteLine("A - Add product to cart");
                    Console.WriteLine("R - Remove product from cart");
                    Console.WriteLine("C - Checkout");
                    Console.Write("Enter choice: ");
                    var action = Console.ReadLine()?.Trim().ToUpper();

                    if (action == "A")
                    {
                        Console.Write("Enter product number to add: ");
                        if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > availableProducts.Count)
                        {
                            Console.WriteLine("Invalid choice Try again");
                            continue;
                        }
                        var selectedProduct = availableProducts[choice - 1];
                        Console.Write($"Enter quantity for {selectedProduct.Name}: ");
                        if (!int.TryParse(Console.ReadLine(), out int qty) || qty <= 0)
                        {
                            Console.WriteLine("Invalid quantity Try again");
                            continue;
                        }
                        try
                        {
                            customer.Cart.AddProduct(selectedProduct, qty);
                            Console.WriteLine($"Added {qty}x {selectedProduct.Name} to cart");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                    else if (action == "R")
                    {
                        if (customer.Cart.Items.Count == 0)
                        {
                            Console.WriteLine("Cart is empty Nothing to remove");
                            continue;
                        }
                        Console.Write("Enter cart item number to remove: ");
                        if (!int.TryParse(Console.ReadLine(), out int removeIndex) || removeIndex < 1 || removeIndex > customer.Cart.Items.Count)
                        {
                            Console.WriteLine("Invalid choice Try again");
                            continue;
                        }
                        var toRemove = customer.Cart.Items[removeIndex - 1].Product;
                        customer.Cart.RemoveProduct(toRemove);
                        Console.WriteLine($"Removed {toRemove.Name} from cart");
                    }
                    else if (action == "C")
                    {
                        checkoutService.Checkout(customer);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid action Try again");
                    }
                }
            }
            #endregion
            else
            {
                Console.WriteLine("Invalid login choice");
            }

        }
    }
}
