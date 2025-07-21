
using System.Collections;
using System.Collections.Generic;
using System.Inventory;
using System.Item;
using System.Linq;
using UIElements;
using UnityEngine;
using Utility;

namespace System.Crafting
{

    public class CraftingController
    {
        private readonly CraftingModel model;
        private readonly CraftingView view;
        private readonly IDataRegistry<IItemDetail> itemDetailDictionary;
        private readonly CraftingService craftingService;

        private CraftingController(CraftingView view, CraftingModel model, IDataRegistry<IItemDetail> itemDetailDictionary, CraftingService craftingService)
        {
            Debug.Assert(view != null, "View is null");
            Debug.Assert(model != null, "Model is null");
            Debug.Assert(itemDetailDictionary != null, "ItemDetailDict is null");
            Debug.Assert(craftingService != null, "CraftingService is null");
            this.view = view;
            this.model = model;
            this.itemDetailDictionary = itemDetailDictionary;
            this.craftingService = craftingService;

            view.StartCoroutine(Initialize());
        }

        IEnumerator Initialize()
        {
            yield return view.InitializeView();

            model.OnSelectionChanged += (value) => HandleSelectionChanged(value);
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
            itemDetailDictionary.TryGetValue(id, out IItemDetail item);

            if (item == null)
            {
                Debug.Log($"ItemDetail with Id {id} is missing");
                return;
            }

            model.DeselectItem(item);
        }

        private void HandleSelectionChanged(Dictionary<IItemDetail, int> data)
        {
            view.UpdateSelectedList(
                data
                .SelectMany(pair => Enumerable.Repeat(pair.Key, pair.Value))
                .OrderByDescending(item => item.Quality.Score)
                .Select(item => new SelectedItemDisplayData()
                {
                    Id = item.Id,
                    Icon = item.Icon,
                    BgColor = item.Quality.Color
                })
                .ToList()
            );
        }

        private void HandleInventoryChanged(IList<ItemInstance> data)
        {
            view.UpdateInventory(data.Where(item => item.Detail is ICraftingResource).OrderByDescending(item => item.Detail.Quality.Score).Select(item => new ItemDisplayData()
            {
                Id = item.Id,
                Icon = item.Detail.Icon,
                BgColor = item.Detail.Quality.Color,
                Quantity = item.Quantity
            }).ToList());
        }

        private void HandleCraftingClick()
        {
            ICraftedItem result = craftingService.Evaluate(model.selection.ToDictionary(entry => entry.Key as ICraftingResource, entry => entry.Value));
            model.ConsumeSelection();
            if (result == null) return;

            ItemInstance instance = new(result.Data);
            model.Add(instance);
            view.UpdateDetailData(new ItemDisplayDetailData()
            {
                Icon = instance.Detail.Icon,
                BgColor = instance.Detail.Quality.Color,
                Name = instance.Detail.Name,
                Description = instance.Detail.Description,
                Effects = new List<TagDisplayData>()
                {
                    new TagDisplayData() {
                        Label = "some label",
                        Color = Color.royalBlue
                    }
                }
            });
        }

        #region builder
        public class Builder
        {
            private CraftingView view;
            private IDataRegistry<IItemDetail> itemDetailDictionary;
            private IInventoryDataSource inventoryDS;
            private CraftingService cs;

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

            public Builder WithItemDetailService(IDataRegistry<IItemDetail> dictionary)
            {
                itemDetailDictionary = dictionary;
                return this;
            }

            public Builder WithCraftingService(CraftingService service)
            {
                cs = service;
                return this;
            }

            public CraftingController Build()
            {
                CraftingModel model = inventoryDS != null
                ? new CraftingModel(inventoryDS.DataSource)
                : new CraftingModel(new ObservableList<ItemInstance>());

                return new CraftingController(view, model, itemDetailDictionary, cs);
            }


        }
        #endregion
    }
}