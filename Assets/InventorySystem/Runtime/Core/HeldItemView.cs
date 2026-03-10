using UnityEngine;
using UnityEngine.UI;
using TMPro;
using IS.UI;

namespace IS.UI
{
    public class HeldItemView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InventoryInteractionController controller;
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text quantityText;

        private RectTransform rectTransform;
        private Canvas canvas;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
        }

        private void Update()
        {
            if (controller.HeldItem.IsEmpty)
            {
                icon.enabled = false;
                quantityText.text = "";
                return;
            }

            icon.enabled = true;
            FollowCursor();
            Refresh();
            
        }

        private void FollowCursor()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.GetComponent<RectTransform>(),
                Input.mousePosition,
                canvas.worldCamera,
                out Vector2 localPoint
            );

            rectTransform.localPosition = localPoint;
        }

        private void Refresh()
        {
            var held = controller.HeldItem;
            icon.sprite = held.Item.Icon;
            icon.enabled = true;

            quantityText.text = held.Quantity > 1
                ? held.Quantity.ToString()
                : "";
        }
    }
}