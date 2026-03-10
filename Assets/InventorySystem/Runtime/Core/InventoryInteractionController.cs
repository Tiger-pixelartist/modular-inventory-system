using UnityEngine;
using IS.Core;

namespace IS.UI
{
    public class InventoryInteractionController : MonoBehaviour
    {
        private Inventory inventory;
        private HeldItem heldItem = new HeldItem();
        private InventorySlot heldFromSlot;
        public InventorySlot HeldFromSlot => heldFromSlot;

        public HeldItem HeldItem => heldItem;

        private void Awake()
        {
            inventory = GetComponent<Inventory>();
            if (inventory == null)
                Debug.LogError("InventoryInteractionController: No Inventory found on this GameObject.");
        }

        public void OnLeftClick(InventorySlot clickedSlot)
        {

            if (heldItem.IsEmpty)
            {
                if (clickedSlot.IsEmpty) return;


                heldFromSlot = clickedSlot;
                heldItem.Pick(clickedSlot.Item, clickedSlot.Quantity);
                clickedSlot.Clear();


                inventory.NotifyChanged();
                return;
            }


            if (clickedSlot.IsEmpty)
            {
                clickedSlot.Add(heldItem.Item, heldItem.Quantity);
                heldItem.Clear();

                heldFromSlot = null;
                inventory.NotifyChanged();
                return;
            }

            // Merge stackable items
            if (clickedSlot.CanStack(heldItem.Item))
            {
                int remaining = clickedSlot.Add(heldItem.Item, heldItem.Quantity);
                if (remaining <= 0)
                {

                    heldItem.Clear();
                    heldFromSlot = null;
                }
                else
                {

                    heldItem.Pick(heldItem.Item, remaining);
                }

                inventory.NotifyChanged();
                return;
            }

            // Swap items between hand and clicked slot
            var tempItem = clickedSlot.Item;
            var tempQuantity = clickedSlot.Quantity;
            clickedSlot.Clear();
            clickedSlot.Add(heldItem.Item, heldItem.Quantity);
            heldItem.Pick(tempItem, tempQuantity);

            // After swapping, the item now in hand came from the clicked slot — update origin
            heldFromSlot = clickedSlot;

            inventory.NotifyChanged();
        }

        public void OnRightClick(InventorySlot clickedSlot)
        {
            if (heldItem.IsEmpty)
            {
                if (clickedSlot.IsEmpty || clickedSlot.Quantity <= 1) return;
                int half = clickedSlot.Quantity / 2;
                clickedSlot.Remove(half);

                // Origin is this slot for the split pick
                heldFromSlot = clickedSlot;
                heldItem.Pick(clickedSlot.Item, half);
                inventory.NotifyChanged();
                return;
            }

            if (clickedSlot.IsEmpty || clickedSlot.CanStack(heldItem.Item))
            {
                clickedSlot.Add(heldItem.Item, 1);
                heldItem.RemoveQuantity(1);

                if (heldItem.IsEmpty)
                    heldFromSlot = null;

                inventory.NotifyChanged();
            }
        }
    }
}