using System.Collections.Generic;
using UnityEngine.UIElements;

namespace System.Inventory
{
    [UxmlElement]
    public partial class InventoryListView : VisualElement
    {
        public event Action<string> OnElementClick = delegate { };

        private readonly ListView listView;
        private List<ItemDisplayData> internalDataSource = new();
        public List<ItemDisplayData> Data
        {
            set
            {
                if (value == null) return;
                internalDataSource = value;
                listView.itemsSource = value;
                // listView.Rebuild();
            }
        }
        public InventoryListView()
        {
            listView = new()
            {
                makeItem = () =>
                {
                    ItemDisplay element = new();
                    element.OnClick += (value) => OnElementClick.Invoke(value);
                    return element;
                },
                bindItem = (element, index) =>
                    {
                        ((ItemDisplay)element).Set(internalDataSource[index]);
                    },
                itemsSource = internalDataSource,
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
            };
            Add(listView);
        }
    }
}