using IS.Data;

namespace IS.Core
{
    public class HeldItem
    {
        public ItemData Item { get; private set; }
        public int Quantity { get; private set; }

        public bool IsEmpty => Item == null;

        public void Pick(ItemData item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }

        public void Clear()
        {
            Item = null;
            Quantity = 0;
        }

        public int AddQuantity(int amount)
        {
            Quantity += amount;
            return Quantity;
        }

        public int RemoveQuantity(int amount)
        {
            int removed = UnityEngine.Mathf.Min(amount, Quantity);
            Quantity -= removed;
            if (Quantity <= 0)
                Clear();
            return removed;
        }
    }
}