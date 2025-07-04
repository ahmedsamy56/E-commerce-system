using E_commerce_system.Entities.Abstracts.Classes;

namespace E_commerce_system.Entities.Implementations
{
    public class Cart
    {
        #region Fields
        private List<CartItem> _items = new List<CartItem>();
        public IReadOnlyList<CartItem> Items => _items.AsReadOnly();
        #endregion

        #region Functions
        public void AddProduct(Product product, int quantity)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            if (quantity <= 0) throw new ArgumentException("Quantity must be positive.");
            if (!product.IsQuantityAvailable(quantity)) throw new InvalidOperationException($"Cannot add more than available stock for {product.Name}.");


            //Handle if the product is added more than once
            var existing = _items.Find(i => i.Product == product);
            if (existing != null)
            {
                if (existing.Quantity + quantity > product.Quantity)
                    throw new InvalidOperationException($"Cannot add more than available stock for {product.Name}.");
                existing.Quantity += quantity;
            }
            else
            {
                _items.Add(new CartItem { Product = product, Quantity = quantity });
            }
        }

        public void RemoveProduct(Product product)
        {
            _items.RemoveAll(i => i.Product == product);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool IsEmpty() => _items.Count == 0;

        #endregion


    }


    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }

}
