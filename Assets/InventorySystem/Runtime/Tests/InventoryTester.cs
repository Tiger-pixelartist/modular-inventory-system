using UnityEngine;
using IS.Core;
using IS.Data;

namespace IS.Tester
{
    public class InventoryTester : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Inventory inventory;

        [Header("Test Items")]
        [SerializeField] private ItemData itemA;
        [SerializeField] private ItemData itemB;

        private void Awake()
        {
            if (inventory == null)
                inventory = GetComponent<Inventory>();
        }

        private void Start()
        {
            inventory.Add(itemA, 3);
            inventory.Add(itemB, 1);
        }
    }
}