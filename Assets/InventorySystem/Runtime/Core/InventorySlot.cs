using IS.Data;

namespace IS.Core
{
    public class InventorySlot
    {
        public ItemData Item { get; private set; }
        public int Quantity { get; private set; }

        public bool IsEmpty => Item == null;

        public bool CanStack(ItemData item)
        {
            if (IsEmpty)
                return false;

            if (!Item.IsStackable)
                return false;

            return Item.ItemID == item.ItemID && Quantity < Item.MaxStackSize;
        }

        public int Add(ItemData item, int amount)
        {
            if (item == null || amount <= 0)
                return amount;

            if (IsEmpty)
            {
                Item = item;

                int added = item.IsStackable
                    ? UnityEngine.Mathf.Min(amount, item.MaxStackSize)
                    : 1;

                Quantity = added;
                return amount - added;
            }

            if (!CanStack(item))
                return amount;
            Item = item;
            int spaceLeft = Item.MaxStackSize - Quantity;
            int toAdd = UnityEngine.Mathf.Min(spaceLeft, amount);

            Quantity += toAdd;
            return amount - toAdd;
        }

        public int Remove(int amount)
        {
            if (IsEmpty || amount <= 0)
                return amount;

            int removed = UnityEngine.Mathf.Min(amount, Quantity);
            Quantity -= removed;

            if (Quantity <= 0)
                Clear();

            return amount - removed;
        }


        public void Clear()
        {
            Item = null;
            Quantity = 0;
        }
    }
}