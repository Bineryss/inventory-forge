using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.UI;
using UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace System.Inventory
{

    public class InventoryView : MonoBehaviour, IView
    {
        [SerializeField] private VisualTreeAsset asset;
        [SerializeField] private StyleSheet styleSheet;
        public event Action<string> OnItemClick = delegate { };

        private VisualElement root;
        private InventoryListView listView;
        private ItemDetailElement detailView;
        public IEnumerator InitializeView()
        {
            root ??= new();
            root.Clear();
            root.styleSheets.Add(styleSheet);
            asset.CloneTree(root);

            VisualElement list = root.Q<VisualElement>("list");
            VisualElement detail = root.Q<VisualElement>("detail");

            listView = new();
            listView.OnElementClick += (value) => OnItemClick.Invoke(value);
            listView.style.flexGrow = 1;
            listView.style.marginRight = 32;
            list.Add(listView);

            detailView = new();
            detailView.style.display = DisplayStyle.None;
            detail.Add(detailView);

            yield return null;
        }
        public void Mount(VisualElement root)
        {
            this.root ??= new();
            root.Add(this.root);
        }

        public void UpdateData(List<ItemDisplayData> data)
        {
            listView.Data = data.OrderByDescending(el => el.Order).ToList();
        }

        public void UpdateDetailData(ItemDisplayDetailData data)
        {
            if (detailView.style.display == DisplayStyle.None)
            {
                detailView.style.display = DisplayStyle.Flex;
            }
            detailView.Data = data;
        }

        public void Show()
        {
            root.style.display = DisplayStyle.Flex;
        }

        public void Hide()
        {
            root.style.display = DisplayStyle.None;
        }
    }

    public class ItemDisplayData
    {
        public string Id;
        public string Icon;
        public Color BgColor;
        public int Quantity;
        public int Order;
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