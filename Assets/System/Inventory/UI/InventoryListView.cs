using System.Collections.Generic;
using System.Linq;
using UIElements;
using UnityEngine.UIElements;

namespace System.Inventory
{
    public enum Order
    {
        NONE,
        ASC,
        DESC
    }
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
                    listList.Add(value.OrderByDescending(el => el.Order).Skip(row).Take(Cols).ToList());
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
                makeItem = MakeItem,
                bindItem = (element, index) =>
                    {
                        if (element is not HorizontalListElement<ItemDisplay, ItemDisplayData> itemEl) return;

                        itemEl.Set(internalDataSource[index]);
                    },
                itemsSource = internalDataSource,
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
            };
            Add(listView);
        }

        private VisualElement MakeItem()
        {
            void handleOnClick(string value) => OnElementClick.Invoke(value);
            HorizontalListElement<ItemDisplay, ItemDisplayData> element = new(
            () =>
            {
                ItemDisplay element = new();
                element.OnClick += handleOnClick;
                element.style.marginRight = 4;
                return element;
            },
            (element, data) =>
            {
                element.Set(data);
            },
            (element) => element.OnClick -= handleOnClick);
            return element;
        }
    }
}