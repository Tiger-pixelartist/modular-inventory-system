using UnityEngine;
using IS.Core;

namespace IS.UI
{
    public class InventoryInteractionController : MonoBehaviour
    {
        private InventorySlot selectedSlot;

        public void OnLeftClick(InventorySlot clickedSlot)
        {
            if (selectedSlot == null)
            {
                if (!clickedSlot.IsEmpty)
                    selectedSlot = clickedSlot;

                return;
            }

            if (selectedSlot == clickedSlot)
            {
                selectedSlot = null;
                return;
            }

            HandleMove(selectedSlot, clickedSlot);
            selectedSlot = null;
        }

        public void OnRightClick(InventorySlot clickedSlot)
        {
            if (clickedSlot.IsEmpty || clickedSlot.Quantity <= 1)
                return;

            int half = clickedSlot.Quantity / 2;
            clickedSlot.Remove(half);
        }

        private void HandleMove(InventorySlot from, InventorySlot to)
        {
            if (from.IsEmpty)
                return;

            // Si destino vacío
            if (to.IsEmpty)
            {
                to.Add(from.Item, from.Quantity);
                from.Clear();
                return;
            }

            // Si mismo item y stackeable
            if (to.CanStack(from.Item))
            {
                int remaining = to.Add(from.Item, from.Quantity);
                from.Remove(from.Quantity - remaining);
                return;
            }

            // Intercambio manual
            var tempItem = to.Item;
            var tempQuantity = to.Quantity;

            to.Clear();
            to.Add(from.Item, from.Quantity);

            from.Clear();
            from.Add(tempItem, tempQuantity);
        }
    }
}