using E_commerce_system.Entities.Abstracts.Classes;
using E_commerce_system.Entities.Abstracts.Interfaces;

namespace E_commerce_system.Entities.Implementations
{
    //can expire and does not require shipping
    public class OnlyExpireProduct : Product, IExpirable
    {
        #region Fields
        private DateTime _expirationDate;

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
        #endregion

        #region Constructors
        public OnlyExpireProduct(string name, decimal price, int quantity, DateTime expirationDate) : base(name, price, quantity)
        {
            ExpirationDate = expirationDate;
        }
        #endregion


        #region Functions
        public bool IsExpired()
        {
            return DateTime.Now > ExpirationDate;
        }
        #endregion
    }
}
