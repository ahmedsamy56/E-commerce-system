using E_commerce_system.Entities.Abstracts.Interfaces;

namespace E_commerce_system.Service
{
    public class ShippingService
    {
        public void ShipItems(List<IShippable> items, List<(string name, int quantity)> nameQuantities)
        {
            if (items == null || items.Count == 0)
                return;
            Console.WriteLine("\n\n** Shipment notice **");
            double totalWeight = 0;
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                var name = item.GetName();
                var quantity = nameQuantities.FirstOrDefault(nq => nq.name == name).quantity;
                Console.WriteLine($"{quantity}x {name,-12}{item.GetWeight(),6}g");
                totalWeight += item.GetWeight();
            }
            Console.WriteLine($"Total package weight {totalWeight / 1000.0}kg");
        }
    }
}
