using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace System.Inventory
{
    public class InventoryModel
    {
        public readonly ObservableList<ItemInstance> items = new();
        public event Action<IList<ItemInstance>> OnModelChanged
        {
            add => items.ValueChanged += value;
            remove => items.ValueChanged -= value;
        }

        public InventoryModel(IEnumerable<ItemInstance> itemDetails)
        {
            foreach (var detail in itemDetails)
            {
                items.Add(detail);
            }

        }

        public ItemInstance Get(int index) => items[index];
        public void Add(ItemInstance data)
        {
            items.Add(data);
        }

        public void ChangeQuantity(ItemInstance data, int quantityDelta)
        {
            data.quantity += quantityDelta;
            items.Invoke();
        }

        public void ChangeQuantity(string id, int quantityDelta)
        {
            ItemInstance data = items.ToList().Where(item =>
            {
                return id.Equals(item.Id);
            }).FirstOrDefault();
            if (data == null) return;

            data.quantity += quantityDelta;

            if (data.quantity == 0)
            {
                items.Remove(data);
            }
            else
            {
                items.Invoke();
            }

        }
    }
}