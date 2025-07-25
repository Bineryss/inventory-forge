using System.Collections;
using System.Collections.Generic;
using System.Inventory;
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

        public event Action<string> OnItemSelect = delegate { };
        public event Action<string> OnItemDeselect = delegate { };
        public event Action OnCraftingClick = delegate { };
        public event Action OnClearSelectionClick = delegate { };

        private VisualElement root;
        private InventoryListView inventoryList;
        private ItemDetailElement detailView;
        private HorizontalListElement<ItemDisplay, SelectedItemDisplayData> selectedList;

        public IEnumerator InitializeView()
        {
            root ??= new();
            root.style.flexGrow = 1;
            root.Clear();
            if (styleSheet != null)
            {
                root.styleSheets.Add(styleSheet);
            }
            asset.CloneTree(root);
            CreateInventoryList();
            CreateOverlay();
            CreateSelectionList();

            CraftingButton.clicked += () => OnCraftingClick.Invoke();

            yield return null;
        }
        public void UpdateInventory(List<ItemDisplayData> data)
        {
            inventoryList.Data = data;
        }

        public void UpdateSelectedList(List<SelectedItemDisplayData> data)
        {
            selectedList.Set(data);
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

        private void CreateSelectionList()
        {
            void handleOnSelectedListClick(string value) => OnItemDeselect.Invoke(value);
            selectedList = new(
                () =>
                {
                    ItemDisplay element = new();
                    element.OnClick += handleOnSelectedListClick;
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
                },
                (element) => element.OnClick -= handleOnSelectedListClick,
                5, true
            );
            selectedList.Style.marginBottom = 0;
            selectedList.Style.justifyContent = Justify.Center;
            SelectedList.Add(selectedList);
        }

        private void CreateOverlay()
        {
            Overlay.style.display = DisplayStyle.None;
            detailView = new();
            DetailViewContainer.Add(detailView);
            OKButton.clicked += () => Overlay.style.display = DisplayStyle.None;
        }

        private void CreateInventoryList()
        {
            inventoryList = new();
            inventoryList.OnElementClick += (value) => OnItemSelect.Invoke(value);
            inventoryList.style.flexGrow = 1;
            InventoryList.Add(inventoryList);
        }

    }

    public class SelectedItemDisplayData
    {
        public string Id;
        public string Icon;
        public Color BgColor;
    }
}