using IS.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace IS.UI
{
    public class InventorySlotView : MonoBehaviour, IPointerClickHandler
    {
        [Header("UI References")]
        [SerializeField] private InventoryInteractionController controller;
        [SerializeField] private Image background;
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text quantityText;

        private InventorySlot slot;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                controller.OnLeftClick(slot);

            if (eventData.button == PointerEventData.InputButton.Right)
                controller.OnRightClick(slot);
        }

        // ==============================
        // BIND
        // ==============================

        public void Bind(InventorySlot slot)
        {
            this.slot = slot;
            Refresh();
        }

        // ==============================
        // REFRESH
        // ==============================

        public void Refresh()
        {
            if (slot == null || slot.IsEmpty)
            {
                icon.enabled = false;
                quantityText.text = slot.Quantity > 1 ? slot.Quantity.ToString() : "";

                return;
            }

            icon.enabled = true;
            icon.sprite = slot.Item.Icon;

            if (slot.Item.IsStackable && slot.Quantity > 1)
            {
                quantityText.text = slot.Quantity.ToString();
            }
            else
            {
                quantityText.text = "";
            }
        }
    }
}