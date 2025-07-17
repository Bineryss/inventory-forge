using System.Collections.Generic;
using System.Linq;

namespace System.Inventory
{
    public class InventoryModel
    {
        public readonly ObservableList<ItemInstance> items;
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
    }
}