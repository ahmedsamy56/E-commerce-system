namespace E_commerce_system.Entities.Abstracts.Interfaces
{
    public interface IShippable
    {
        public double Weight { get; set; }
        public string GetName();
        public double GetWeight();
    }
}
