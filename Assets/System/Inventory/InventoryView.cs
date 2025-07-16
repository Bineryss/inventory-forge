using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace System.Inventory
{

    public class InventoryView : StorageView
    {
        public event Action<string> OnItemClick = delegate { };

        private List<ItemDisplayData> data = new();
        public void UpdateData(List<ItemDisplayData> data)
        {
            this.data = data;
            listView.itemsSource = data;
        }

        private ListView listView;
        public override IEnumerator InitializeView()
        {
            Root.Clear();
            Root.styleSheets.Add(styleSheet);

            VisualElement container = new VisualElement();
            Root.Add(container);

            listView = new();
            listView.makeItem = () =>
            {
                ItemDisplay element = new();
                element.OnClick += OnItemClick;
                return element;
            };
            listView.bindItem = (element, index) =>
            {
                ItemDisplay convEl = element as ItemDisplay;
                convEl.Set(listView.itemsSource[index] as ItemDisplayData);
            };
            listView.itemsSource = data;
            listView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            listView.fixedItemHeight = 0;
            container.Add(listView);

            yield return null;
        }
    }

    public class ItemDisplayData
    {
        public string Id;
        public string Icon;
        public Color BgColor;
        public int Quantity;
    }
}