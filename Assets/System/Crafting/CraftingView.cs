using System.Collections;
using System.Collections.Generic;
using System.Inventory;
using System.Linq;
using System.UI;
using UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace System.Crafting
{
    public class CraftingView : MonoBehaviour, IView
    {
        private VisualElement Overlay => root.Q<VisualElement>("crafted-item-overlay");
        private VisualElement DetailViewContainer => root.Q<VisualElement>("detail-view");
        private Button OKButton => root.Q<Button>("ok");
        private VisualElement InventoryList => root.Q<VisualElement>("inventory-list");
        private VisualElement SelectedList => root.Q<VisualElement>("selected-items");
        private Button CraftingButton => root.Q<Button>("craft");


        [SerializeField] private VisualTreeAsset asset;
        [SerializeField] private StyleSheet styleSheet;

        public event Action<List<string>> OnCraftingClick = delegate { };

        private VisualElement root;
        private InventoryListView listView;
        private ItemDetailElement detailView;
        private HorizontalListElement<ItemDisplay, SelectedItemDisplayData> selectedList;

        private List<SelectedItemDisplayData> selectedItems = new();
        private List<ItemDisplayData> internalInventoryList = new();
        public IEnumerator InitializeView()
        {
            root ??= new();
            root.Clear();
            if (styleSheet != null)
            {
                root.styleSheets.Add(styleSheet);
            }
            asset.CloneTree(root);

            listView = new();
            listView.OnElementClick += (value) => SelectItem(value);
            listView.style.flexGrow = 1;
            InventoryList.Add(listView);

            Overlay.style.display = DisplayStyle.None;
            detailView = new();
            detailView.style.display = DisplayStyle.None;
            DetailViewContainer.Add(detailView);
            OKButton.clicked += () => Overlay.style.display = DisplayStyle.None;

            selectedList = new(
                () =>
                {
                    ItemDisplay element = new();
                    element.OnClick += (value) => Deselect(value);
                    element.style.marginRight = 4;
                    return element;
                },
                (element, data) =>
                {
                    element.Set(new ItemDisplayData()
                    {
                        Id = data.Id,
                        Icon = data.Icon,
                        BgColor = data.BgColor
                    });
                }
            );
            SelectedList.Add(selectedList);

            CraftingButton.clicked += () => OnCraftingClick.Invoke(selectedItems.Select(el => el.Id).ToList());

            yield return null;
        }

        public void SelectItem(string id)
        {
            ItemDisplayData item = internalInventoryList.FirstOrDefault((el) => id.Equals(el.Id));
            if (item == null) return;

            selectedItems.Add(new SelectedItemDisplayData()
            {
                Id = item.Id,
                Icon = item.Icon,
                BgColor = item.BgColor
            });
            selectedList.Set(selectedItems);
        }

        public void Deselect(string id)
        {
            Debug.Log("deselcted item");
        }

        public void UpdateInventory(List<ItemDisplayData> data)
        {
            listView.Data = data;
            internalInventoryList = data;
        }

        public void UpdateDetailData(ItemDisplayDetailData data)
        {
            detailView.Data = data;
            Overlay.style.display = DisplayStyle.Flex;
        }

        public void Hide()
        {
            root.style.display = DisplayStyle.None;
        }

        public void Mount(VisualElement root)
        {
            this.root ??= new();
            root.Add(this.root);
        }

        public void Show()
        {
            root.style.display = DisplayStyle.Flex;
        }
    }

    public class SelectedItemDisplayData
    {
        public string Id;
        public string Icon;
        public Color BgColor;
    }
}