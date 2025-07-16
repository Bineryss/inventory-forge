using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace System.Inventory
{
    public class InventoryListView : VisualElement
    {
        public event Action<string> OnElementClick = delegate { };
        public int Cols { get; set; } = 5;

        private readonly ListView listView;
        private List<List<ItemDisplayData>> internalDataSource = new();
        public List<ItemDisplayData> Data
        {
            set
            {
                if (value == null) return;
                int row = 0;
                List<List<ItemDisplayData>> listList = new();

                while (row < value.Count)
                {
                    listList.Add(value.Skip(row).Take(Cols).ToList());
                    row += Cols;
                }

                internalDataSource = listList;
                listView.itemsSource = listList;
                // listView.Rebuild();
            }
        }
        public InventoryListView()
        {
            listView = new()
            {
                makeItem = () =>
                {
                    InventoryRow element = new();
                    element.OnElementClick += (value) => OnElementClick.Invoke(value);
                    return element;
                },
                bindItem = (element, index) =>
                    {
                        ((InventoryRow)element).Set(internalDataSource[index]);
                    },
                itemsSource = internalDataSource,
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
            };
            Add(listView);
        }
    }
}