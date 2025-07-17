using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace System.Inventory
{
    public class InventoryModel
    {
        public readonly ObservableList<ItemInstance> items = new();
        public ItemInstance selectedItem;
        public event Action<IList<ItemInstance>> OnModelChanged
        {
            add => items.ValueChanged += value;
            remove => items.ValueChanged -= value;
        }

        public InventoryModel(ObservableList<ItemInstance> itemDetails)
        {
            items = itemDetails;
        }

        public ItemInstance Get(int index) => items[index];
        public void Add(ItemInstance data)
        {
            items.Add(data);
        }

        public void SelectItem(string id)
        {
            ItemInstance data = items
            .ToList()
            .Where(item =>
            {
                return id.Equals(item.Id);
            })
            .FirstOrDefault();
            selectedItem = data;
            items.Invoke(); //TODO find proper event mechanism for this here
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