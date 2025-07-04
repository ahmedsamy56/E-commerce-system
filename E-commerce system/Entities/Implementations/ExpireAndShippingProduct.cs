using E_commerce_system.Entities.Abstracts.Classes;
using E_commerce_system.Entities.Abstracts.Interfaces;

namespace E_commerce_system.Entities.Implementations
{
    //can expire and require shipping
    public class ExpireAndShippingProduct : Product, IExpirable, IShippable
    {
        #region Fields
        private DateTime _expirationDate;
        private double _weight;

        public DateTime ExpirationDate
        {
            get => _expirationDate;
            set
            {
                if (value < DateTime.Now)
                    throw new ArgumentException("Expiration date cannot be in the past");

                _expirationDate = value;
            }
        }
        public double Weight
        {
            get => _weight;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Weight must be greater than zero.", nameof(value));

                _weight = value;
            }
        }

        #endregion

        #region Constructors
        public ExpireAndShippingProduct(string name, decimal price, int quantity, double weight, DateTime expirationDate) : base(name, price, quantity)
        {
            ExpirationDate = expirationDate;
            Weight = weight;

        }

        #endregion

        #region Functions
        public bool IsExpired()
        {
            return DateTime.Now > ExpirationDate;
        }

        public string GetName()
        {
            return Name;
        }
        public double GetWeight()
        {
            return Weight;
        }
        #endregion

    }
}
