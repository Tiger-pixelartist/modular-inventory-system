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
        private InventoryInteractionController controller;
        [SerializeField] private Image background;
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text quantityText;

        [Header("Highlight Settings")]
        [SerializeField] private Sprite normalSprite;
        [SerializeField] private Sprite selectedSprite;

        private InventorySlot slot;

        public void OnPointerClick(PointerEventData eventData)
        {

            if (controller == null)
            {
                return;
            }

            if (eventData.button == PointerEventData.InputButton.Left)
                controller.OnLeftClick(slot);

            if (eventData.button == PointerEventData.InputButton.Right)
                controller.OnRightClick(slot);
        }


        // Call this to visually highlight or unhighlight the slot
        public void SetHighlight(bool highlighted)
        {
            if (normalSprite == null || selectedSprite == null) return;
            background.sprite = highlighted ? selectedSprite : normalSprite;
        }


        public void SetController(InventoryInteractionController controller)
        {
            this.controller = controller;
        } 
        // BIND


        public void Bind(InventorySlot slot)
        {
            this.slot = slot;
            Refresh();
        }

        // REFRESH
        public void Refresh()
        {
            if (slot == null || slot.IsEmpty)
            {
                icon.enabled = false;
                quantityText.text = "";
            }
            else
            {
                icon.enabled = true;
                icon.sprite = slot.Item.Icon;
                quantityText.text = slot.Item.IsStackable && slot.Quantity > 1
                    ? slot.Quantity.ToString()
                    : "";
            }

            // Update highlight based on whether this slot is the held item origin
            bool isSelected = controller != null && controller.HeldFromSlot == slot;
            SetHighlight(isSelected);
        }
    }
}