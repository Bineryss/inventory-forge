using System.Collections.Generic;
using System.Inventory;
using System.Item;
using System.Linq;
using UnityEngine;

namespace System.Crafting
{
    public class CraftingModel
    {
        public event Action<Dictionary<IItemDetail, int>> OnSelectionChanged = delegate { };

        public event Action<IList<ItemInstance>> OnInventoryChanged
        {
            add => inventory.ValueChanged += value;
            remove => inventory.ValueChanged -= value;
        }

        public readonly ObservableList<ItemInstance> inventory;
        public readonly Dictionary<IItemDetail, int> selection = new();

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

        public void Add(IItemDetail data)
        {
            inventory.Add(new ItemInstance(data));
        }

        public void SelectItem(string id, int quantity = 1) // ItemInstance Id
        {
            ItemInstance data = ChangeQuantity(id, -quantity);
            if (data == null)
            {
                Debug.Log($"Item with id {id} dosn't exist!");
                return;
            }


            if (selection.TryGetValue(data.Detail, out int selectedQuantity))
            {
                selection[data.Detail] = selectedQuantity + quantity;
            }
            else
            {
                selection[data.Detail] = quantity;
            }
            OnSelectionChanged.Invoke(selection);
        }

        public void DeselectItem(IItemDetail item, int quantity = 1)
        {
            if (!selection.TryGetValue(item, out int selectedQuantity))
            {
                Debug.Log($"Item with id {item.Id} dosnt exists in selection!");
                return;
            }
            int newQuantity = selectedQuantity - quantity;
            if (newQuantity < 0)
            {
                Debug.Log($"You cant deselct more items than are selected!");
                return;
            }

            if (newQuantity == 0)
            {
                selection.Remove(item);
            }
            else
            {
                selection[item] = newQuantity;
            }
            OnSelectionChanged.Invoke(selection);


            ItemInstance data = inventory.FirstOrDefault(el => item.Equals(el.Detail));

            if (data != null)
            {
                ChangeQuantity(data.Id, quantity);
            }
            else
            {
                inventory.Add(new ItemInstance(item) { quantity = quantity });
            }
        }

    }
}