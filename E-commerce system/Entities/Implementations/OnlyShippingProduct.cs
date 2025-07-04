using E_commerce_system.Entities.Abstracts.Classes;
using E_commerce_system.Entities.Abstracts.Interfaces;

namespace E_commerce_system.Entities.Implementations
{
    //require shipping and cannot expire 
    public class OnlyShippingProduct : Product, IShippable
    {
        #region Fields
        private double _weight;

        public double Weight
        {
            get => _weight;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Weight must be greater than zero.");

                _weight = value;
            }
        }
        #endregion

        #region Constructors
        public OnlyShippingProduct(string name, decimal price, int quantity, double weight) : base(name, price, quantity)
        {
            Weight = weight;
        }

        #endregion

        #region Functions
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
