    using UnityEngine;

    namespace IS.Data
    {
        [CreateAssetMenu(
            fileName = "NewItem",
            menuName = "Modular Inventory System/Item Data",
            order = 0)]
        public class ItemData : ScriptableObject
        {

            [Header("Visual")]
            [SerializeField] private Sprite icon;
            public Sprite Icon => icon;

            [Header("Identification")]
            [SerializeField] private string itemID;

            [Header("Display")]
            [SerializeField] private string displayName;

            [Header("Stacking")]
            [SerializeField] private bool isStackable = true;

            [SerializeField] private int maxStackSize = 99;

            // ===== Public Read-Only API =====

            public string ItemID => itemID;
            public string DisplayName => displayName;
            public bool IsStackable => isStackable;
            public int MaxStackSize => isStackable ? Mathf.Max(1, maxStackSize) : 1;

    #if UNITY_EDITOR
            private void OnValidate()
            {
                if (string.IsNullOrWhiteSpace(itemID))
                {
                    itemID = name;
                }

                if (maxStackSize < 1)
                {
                    maxStackSize = 1;
                }
            }
    #endif
        }
    }