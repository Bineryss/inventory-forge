using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace System.Inventory
{

    public class InventoryView : StorageView
    {
        public event Action<string> OnItemClick = delegate { };

        public void UpdateData(List<ItemDisplayData> data)
        {
            listView.Data = data;
        }

        private InventoryListView listView;
        public override IEnumerator InitializeView()
        {
            Root.Clear();
            Root.styleSheets.Add(styleSheet);

            VisualElement container = new VisualElement();
            Root.Add(container);

            listView = new();
            listView.OnElementClick += (value) => OnItemClick.Invoke(value);
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