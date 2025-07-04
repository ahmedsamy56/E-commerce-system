namespace E_commerce_system.Entities.Abstracts.Classes
{
    public abstract class Product
    {
        #region Fields
        private string _name;
        private decimal _price;
        private int _quantity;

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Product Name cannot be null or empty.");

                if (value.Length > 100)
                    throw new ArgumentException("Product name cannot exceed 100 characters.");

                _name = value.Trim();
            }
        }
        public decimal Price
        {
            get => _price;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Price cannot be negative.");

                _price = value;
            }
        }
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Quantity cannot be negative.");

                _quantity = value;
            }
        }
        #endregion

        #region Constructors
        protected Product(string name, decimal price, int quantity)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
        }
        #endregion

        #region Functions

        public bool IsQuantityAvailable(int requiredQuantity)
        {
            if (requiredQuantity < 0)
                throw new ArgumentException("Required quantity cannot be negative.");

            return _quantity >= requiredQuantity;
        }
        #endregion
    }
}
