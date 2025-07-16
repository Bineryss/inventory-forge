using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace System.Inventory
{
    public class InventoryController
    {
        readonly InventoryModel model;
        readonly InventoryView view;

        private InventoryController(InventoryView view, InventoryModel model)
        {
            Debug.Assert(view != null, "View is null");
            Debug.Assert(model != null, "Model is null");
            this.view = view;
            this.model = model;

            view.StartCoroutine(Initialize());
        }

        IEnumerator Initialize()
        {
            yield return view.InitializeView();

            view.OnItemClick += HandleItemClick;
            model.OnModelChanged += HandleModelChanged;

            RefreshView();
        }

        private void HandleItemClick(string id)
        {
            Debug.Log($"handle click for {id}");
            model.ChangeQuantity(id, -1);
        }

        private void HandleModelChanged(IList<ItemInstance> data)
        {
            RefreshView();
        }

        private void RefreshView()
        {
            foreach (var item in model.items)
            {
                Debug.Log($"item: {item.detail.icon}-{item.quantity}");
            }
            view.UpdateData(model.items.Select(item => new ItemDisplayData()
            {
                Id = item.Id,
                Quantity = item.quantity,
                Icon = item.detail.icon,
                BgColor = item.detail.rarity.color,
            }).ToList());
        }

        #region builder
        public class Builder
        {
            InventoryView view;
            IEnumerable<ItemInstance> itemDetails;

            public Builder(InventoryView view)
            {
                this.view = view;
            }

            public Builder Items(IEnumerable<ItemInstance> itemDetails)
            {
                this.itemDetails = itemDetails;
                return this;
            }

            public InventoryController Build()
            {
                InventoryModel model = itemDetails != null
                ? new InventoryModel(itemDetails)
                : new InventoryModel(new List<ItemInstance>());

                return new InventoryController(view, model);
            }

        }
        #endregion
    }
}