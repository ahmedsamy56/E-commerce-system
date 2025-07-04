using E_commerce_system.Entities.Abstracts.Classes;

namespace E_commerce_system.Entities.Implementations
{
    //cannot expire and does not require shipping
    public class NotShippingNotExpireProduct : Product
    {
        #region Fields
        #endregion

        #region Constructors
        public NotShippingNotExpireProduct(string name, decimal price, int quantity) : base(name, price, quantity)
        {
        }
        #endregion

        #region Functions
        #endregion

    }
}
