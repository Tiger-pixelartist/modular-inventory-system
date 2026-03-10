using System;
using System.Collections.Generic;
using UnityEngine;
using IS.Data;

namespace IS.Core
{
    public class Inventory : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] public int capacity = 20;

        private List<InventorySlot> slots = new List<InventorySlot>();

        public IReadOnlyList<InventorySlot> Slots => slots;


        public event Action OnInventoryChanged;

        public InventorySlot GetFirstEmptySlot()
        {
            foreach (var slot in slots)
            {
                if (slot.IsEmpty)
                    return slot;
            }
            return null;
        }

        private void Awake()
        {
            for (int i = 0; i < capacity; i++)
            {
                slots.Add(new InventorySlot());
            }
        }
        public void NotifyChanged()
        {
            OnInventoryChanged?.Invoke();
        }


        private void Initialize()
        {
            capacity = Mathf.Max(1, capacity);

            slots = new List<InventorySlot>(capacity);

            for (int i = 0; i < capacity; i++)
            {
                slots.Add(new InventorySlot());
            }
        }

        public int Add(ItemData item, int amount)
        {
            if (item == null || amount <= 0)
                return amount;

            int remaining = amount;

            // Stacks existentes
            if (item.IsStackable)
            {
                foreach (var slot in slots)
                {
                    if (slot.CanStack(item))
                    {
                        remaining = slot.Add(item, remaining);
                        if (remaining <= 0)
                            break;
                    }
                }
            }

            // Slots vacíos
            if (remaining > 0)
            {
                foreach (var slot in slots)
                {
                    if (slot.IsEmpty)
                    {
                        remaining = slot.Add(item, remaining);
                        if (remaining <= 0)
                            break;
                    }
                }
            }

            if (remaining != amount)
                OnInventoryChanged?.Invoke();

            return remaining;
        }

        public int Remove(ItemData item, int amount)
        {
            if (item == null || amount <= 0)
                return amount;

            int remaining = amount;

            foreach (var slot in slots)
            {
                if (!slot.IsEmpty && slot.Item.ItemID == item.ItemID)
                {
                    remaining = slot.Remove(remaining);
                    if (remaining <= 0)
                        break;
                }
            }

            if (remaining != amount)
                OnInventoryChanged?.Invoke();

            return remaining;
        }
    }
}