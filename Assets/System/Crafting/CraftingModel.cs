using System.Collections.Generic;
using System.Inventory;
using System.Item;
using System.Linq;
using UnityEngine;

namespace System.Crafting
{
    public class CraftingModel
    {
        public event Action<IList<ItemDetail>> OnSelectionChanged
        {
            add => selection.ValueChanged += value;
            remove => selection.ValueChanged -= value;
        }

        public event Action<IList<ItemInstance>> OnInventoryChanged
        {
            add => inventory.ValueChanged += value;
            remove => inventory.ValueChanged -= value;
        }

        public readonly ObservableList<ItemInstance> inventory;
        public readonly ObservableList<ItemDetail> selection = new();

        public CraftingModel(ObservableList<ItemInstance> inventory)
        {
            this.inventory = inventory;
        }

        private ItemInstance ChangeQuantity(string id, int quantityDelta)
        {
            ItemInstance data = inventory.FirstOrDefault(item => id.Equals(item.Id));
            if (data == null) return null;

            data.quantity += quantityDelta;

            if (data.quantity == 0)
            {
                inventory.Remove(data);
            }
            else
            {
                inventory.Invoke();
            }

            return data;
        }

        public void Add(ItemDetail data)
        {
            inventory.Add(new ItemInstance(data));
        }

        public void SelectItem(string id) // ItemInstance Id
        {
            ItemInstance data = ChangeQuantity(id, -1);
            if (data == null)
            {
                Debug.Log($"Item with id {id} dosn't exist!");
                return;
            }
            selection.Add(data.detail);
        }

        public void DeselectItem(ItemDetail item)
        {
            ItemInstance data = inventory.FirstOrDefault(el => item.Equals(el.detail));

            if (data != null)
            {
                ChangeQuantity(data.Id, 1);
            }
            else
            {
                inventory.Add(new ItemInstance(item));
            }
            selection.Remove(item);
        }

        public void ClearSelection()
        {
            foreach (ItemDetail el in selection)
            {
                DeselectItem(el);
            }
        }
    }
}