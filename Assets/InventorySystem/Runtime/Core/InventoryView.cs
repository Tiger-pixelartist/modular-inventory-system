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
        private void Awake()
        {
            if (inventory == null)
                inventory = GetComponent<Inventory>();
        }

        private void Build()
        {
            // Limpiar hijos
            foreach (Transform child in slotsParent)
            {
                Destroy(child.gameObject);
            }

            // Crear slots visuales según capacidad
            foreach (var slot in inventory.Slots)
            {
                var instance = Instantiate(slotPrefab, slotsParent);
                instance.Bind(slot);
            }

            AdjustPanelSize();
        }

        private void AdjustPanelSize()
        {
            var grid = slotsParent.GetComponent<GridLayoutGroup>();

            int totalSlots = inventory.Slots.Count;
            int columns = grid.constraintCount;
            int rows = Mathf.CeilToInt((float)totalSlots / columns);

            float width = (columns * grid.cellSize.x) +
                          ((columns - 1) * grid.spacing.x) +
                          grid.padding.left + grid.padding.right;

            float height = (rows * grid.cellSize.y) +
                           ((rows - 1) * grid.spacing.y) +
                           grid.padding.top + grid.padding.bottom;

            var rect = slotsParent.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(width, height);

            // Si quieres que el panel también se adapte:
            panelRect.sizeDelta = new Vector2(width + 40, height + 40);
        }


        private void Refresh()
        {
            foreach (Transform child in slotsParent)
            {
                var slotView = child.GetComponent<InventorySlotView>();
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