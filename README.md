# E-commerce System

## What it does
This project is a simple C# e-commerce system. It lets you create different types of products, add them to a shopping cart, and perform a checkout. Some products can expire, some can be shipped, and some can do both or neither. The system uses Object-Oriented Programming (OOP) concepts like classes, interfaces, and inheritance.

## Product Types
There are four main types of products in this system:
- **Expirable and Shippable Product**: Can expire and be shipped.
- **Only Expirable Product**: Can expire but cannot be shipped.
- **Only Shippable Product**: Can be shipped but does not expire.
- **Neither Expirable nor Shippable Product**: Cannot expire and cannot be shipped.

## Example
Here is a simple example of how you might use the system in code:

```csharp
// Create products
var Cheese = new ExpireAndShippingProduct("Cheese", 100, 10, 400, DateTime.Now.AddDays(5));
var game card = new OnlyExpireProduct("game card", 150, 50, DateTime.Now.AddDays(30));
var TV = new OnlyShippingProduct("TV", 5000, 3, 700);
var scratchCard = new NotShippingNotExpireProduct("scratch card", 50, 20);

// Add products to cart
var customer = new Customer(customerName, customerBalance);
customer.Cart.AddProduct(selectedProduct, qty);

// Checkout
var checkoutService = new CheckoutService();
checkoutService.Checkout(customer);
```

## How to Run
1. Open the solution in Visual Studio.
2. Build the project.
3. Run the application.

---
This project is for learning and fawry challenge purposes. 
