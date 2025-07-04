using E_commerce_system.Entities.Abstracts.Interfaces;
using E_commerce_system.Entities.Implementations;

namespace E_commerce_system.Service
{
    public class CheckoutService
    {
        #region Fields
        private const decimal ShippingFee = 30;  //It changes according to the needs of the business.
        private readonly ShippingService _shippingService = new ShippingService();
        #endregion


        #region Functions
        public void Checkout(Customer customer)
        {
            var cart = customer.Cart;
            if (cart.IsEmpty())
            {
                Console.WriteLine("Cart is empty!!");
                return;
            }

            // Validate products
            foreach (var item in cart.Items)
            {
                if (item.Quantity > item.Product.Quantity)
                {
                    Console.WriteLine($"{item.Product.Name} is out of stock");
                    return;
                }
                if (item.Product is IExpirable expirable && expirable.IsExpired())
                {
                    Console.WriteLine($"{item.Product.Name} is expired");
                    return;
                }
            }

            decimal subtotal = 0;
            var shippableItems = new List<IShippable>();
            var nameQuantities = new List<(string name, int quantity)>();
            foreach (var item in cart.Items)
            {
                subtotal += item.Product.Price * item.Quantity;
                if (item.Product is IShippable shippable)
                {
                    shippableItems.Add(shippable);
                    nameQuantities.Add((shippable.GetName(), item.Quantity));
                }
            }
            decimal shipping = shippableItems.Count > 0 ? ShippingFee : 0;
            decimal total = subtotal + shipping;

            if (customer.Balance < total)
            {
                Console.WriteLine("Customer balance is insufficient");
                return;
            }


            if (shippableItems.Count > 0)
                _shippingService.ShipItems(shippableItems, nameQuantities);


            Console.WriteLine("\n** Checkout receipt **");
            foreach (var item in cart.Items)
            {
                Console.WriteLine($"{item.Quantity}x {item.Product.Name,-12}{(item.Product.Price * item.Quantity),6}");
            }
            Console.WriteLine("----------------------");
            Console.WriteLine($"{"Subtotal",-15}{subtotal,6}");
            Console.WriteLine($"{"Shipping",-15}{shipping,6}");
            Console.WriteLine($"{"Amount",-15}{total,6}");


            customer.Balance -= total;
            foreach (var item in cart.Items)
            {
                item.Product.Quantity -= item.Quantity;
            }
            cart.Clear();
        }
        #endregion
    }
}
