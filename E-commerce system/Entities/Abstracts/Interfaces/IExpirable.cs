namespace E_commerce_system.Entities.Abstracts.Interfaces
{
    public interface IExpirable
    {
        public DateTime ExpirationDate { get; set; }
        bool IsExpired();
    }
}
