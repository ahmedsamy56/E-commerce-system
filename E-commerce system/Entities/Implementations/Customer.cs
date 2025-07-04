namespace E_commerce_system.Entities.Implementations
{
    public class Customer
    {
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public Cart Cart { get; set; }

        public Customer(string name, decimal balance)
        {
            Name = name;
            Balance = balance;
            Cart = new Cart();
        }
    }
}
