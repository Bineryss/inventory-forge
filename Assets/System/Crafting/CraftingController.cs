
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
        private readonly ItemDetailDictionary itemDetailDictionary;

        public CraftingController(CraftingView view, CraftingModel model, ItemDetailDictionary itemDetailDictionary)
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
            itemDetailDictionary.DataSource.TryGetValue(id, out ItemDetail item);

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
                Id = item.Id,
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
    }
}