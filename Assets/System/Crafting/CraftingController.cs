
using System.Collections;
using System.Collections.Generic;
using System.Inventory;
using System.Item;
using System.Linq;
using UnityEngine;

namespace System.Crafting
{

    public class CraftingController
    {
        private readonly CraftingModel model;
        private readonly CraftingView view;
        private readonly Dictionary<string, ItemDetail> itemDetailDictionary;

        private CraftingController(CraftingView view, CraftingModel model, Dictionary<string, ItemDetail> itemDetailDictionary)
        {
            Debug.Assert(view != null, "View is null");
            Debug.Assert(model != null, "Model is null");
            Debug.Assert(itemDetailDictionary != null, "ItemDetailDict is null");
            this.view = view;
            this.model = model;
            this.itemDetailDictionary = itemDetailDictionary;

            view.StartCoroutine(Initialize());
        }

        IEnumerator Initialize()
        {
            yield return view.InitializeView();

            model.OnSelectionChanged += HandleSelectionChanged;
            model.OnInventoryChanged += HandleInventoryChanged;

            view.OnItemSelect += (value) => model.SelectItem(value);
            view.OnItemDeselect += HandleItemDeselect;
            view.OnCraftingClick += HandleCraftingClick;
            view.OnClearSelectionClick += HandleCraftingClick;

            RefreshView();
        }

        private void RefreshView()
        {
            HandleSelectionChanged(model.selection);
            HandleInventoryChanged(model.inventory);
        }

        private void HandleItemDeselect(string id)
        {
            itemDetailDictionary.TryGetValue(id, out ItemDetail item);

            if (item == null)
            {
                Debug.Log($"ItemDetail with Id {id} is missing");
                return;
            }

            model.DeselectItem(item);
        }

        private void HandleSelectionChanged(IList<ItemDetail> data)
        {
            view.UpdateSelectedList(data.Select(item => new SelectedItemDisplayData()
            {
                Id = item.Id, //FIXEM ID isnt uniqe wich leads to strange behaviour in the selcted list. Not the clicked element gets removed but the first of its kind
                Icon = item.Icon,
                BgColor = item.Rarity.color
            }).ToList());
        }

        private void HandleInventoryChanged(IList<ItemInstance> data)
        {
            view.UpdateInventory(data.Select(item => new ItemDisplayData()
            {
                Id = item.Id,
                Icon = item.detail.Icon,
                BgColor = item.detail.Rarity.color,
                Quantity = item.quantity
            }).ToList());
        }

        private void HandleCraftingClick()
        {
            Debug.Log("start crafting");
        }

        #region builder
        public class Builder
        {
            private CraftingView view;
            private ItemDetailDictionary itemDetailDictionary;
            private IInventoryDataSource inventoryDS;

            public Builder WithView(CraftingView view)
            {
                this.view = view;
                return this;
            }

            public Builder WithInventoryDataSource(IInventoryDataSource source)
            {
                inventoryDS = source;
                return this;
            }

            public Builder WithItemDetailService(ItemDetailDictionary dictionary)
            {
                itemDetailDictionary = dictionary;
                return this;
            }

            public CraftingController Build()
            {
                CraftingModel model = inventoryDS != null
                ? new CraftingModel(inventoryDS.DataSource)
                : new CraftingModel(new ObservableList<ItemInstance>());

                return new CraftingController(view, model, itemDetailDictionary.DataSource);
            }


        }
        #endregion
    }
}