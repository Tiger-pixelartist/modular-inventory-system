using IS.Core;
using UnityEngine;
using UnityEngine.UI;

namespace IS.UI
{
    public class InventoryView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Inventory inventory;
        [SerializeField] private InventorySlotView slotPrefab;
        [SerializeField] private Transform slotsParent;
        [SerializeField] private RectTransform panelRect;
        [SerializeField] private InventoryInteractionController controller;

        private void Awake()
        {
            if (inventory == null)
                inventory = GetComponent<Inventory>();

            if (controller == null)
                controller = GetComponent<InventoryInteractionController>();
        }

        private void Start()
        {
            if (inventory == null)
            {
                Debug.LogError("InventoryView: Inventory reference missing.");
                return;
            }

            inventory.OnInventoryChanged += Refresh;

            Build();
            Refresh();
        }
        private void Build()
        {
            for (int i = slotsParent.childCount - 1; i >= 0; i--)
                DestroyImmediate(slotsParent.GetChild(i).gameObject);

            foreach (var slot in inventory.Slots)
            {
                

                    var instance = Instantiate(slotPrefab, slotsParent);
                    instance.SetController(controller);
                    instance.Bind(slot);
                

            }

            AdjustSize();
        }

        private void AdjustSize()
        {
            var grid = slotsParent.GetComponent<GridLayoutGroup>();
            if (grid == null) return;

            int total = inventory.Slots.Count;
            int columns = grid.constraintCount;
            int rows = Mathf.CeilToInt((float)total / columns);

            float width = (columns * grid.cellSize.x)
                        + ((columns - 1) * grid.spacing.x)
                        + grid.padding.left + grid.padding.right;

            float height = (rows * grid.cellSize.y)
                         + ((rows - 1) * grid.spacing.y)
                         + grid.padding.top + grid.padding.bottom;

            slotsParent.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);

            if (panelRect != null)
                panelRect.sizeDelta = new Vector2(width + 40, height + 40);
        }

        private void Refresh()
        {
            foreach (Transform child in slotsParent)
            {
                var slotView = child.GetComponent<InventorySlotView>();
                if (slotView != null)
                    slotView.Refresh();
            }
        }

        private void OnDestroy()
        {
            if (inventory != null)
                inventory.OnInventoryChanged -= Refresh;
        }
    }
}