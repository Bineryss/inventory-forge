using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace System.Inventory
{

    public class InventoryView : StorageView
    {
        private static VisualTreeAsset asset;
        public event Action<string> OnItemClick = delegate { };

        private InventoryListView listView;
        private ItemDetailElement detailView;
        public override IEnumerator InitializeView()
        {
            Root.Clear();
            Root.styleSheets.Add(styleSheet);

            if (asset == null)
            {
                asset = UnityEngine.Resources.Load<VisualTreeAsset>("inventory-view");
            }

            asset.CloneTree(Root);

            VisualElement list = Root.Q<VisualElement>("list");
            VisualElement detail = Root.Q<VisualElement>("detail");

            listView = new();
            listView.OnElementClick += (value) => OnItemClick.Invoke(value);
            listView.style.flexGrow = 1;
            listView.style.marginRight = 32;
            list.Add(listView);

            detailView = new();
            detail.Add(detailView);

            yield return null;
        }


        public void UpdateData(List<ItemDisplayData> data)
        {
            listView.Data = data;
        }

        public void UpdateDetailData(ItemDisplayDetailData data)
        {
            detailView.Data = data;
        }
    }

    public class ItemDisplayData
    {
        public string Id;
        public string Icon;
        public Color BgColor;
        public int Quantity;
    }

    public class ItemDisplayDetailData
    {
        public string Id;
        public string Icon;
        public Color BgColor;
        public string Name;
        public string Description;
        public List<TagDisplayData> Effects;
    }
}