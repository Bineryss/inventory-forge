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
            ItemInstance selected = model.items.FirstOrDefault(item => id.Equals(item.Id));
            ItemDisplayDetailData displayData = new();

            if (selected != null)
            {
                displayData.Id = selected.Id;
                displayData.Icon = selected.detail.Icon;
                displayData.BgColor = selected.detail.Rarity.color;
                displayData.Name = selected.detail.Name;
                displayData.Description = selected.detail.Description;
            }

            view.UpdateDetailData(displayData);
        }

        private void HandleModelChanged(IList<ItemInstance> data)
        {
            RefreshView();
        }

        private void RefreshView()
        {
            view.UpdateData(model.items.Select(item => new ItemDisplayData()
            {
                Id = item.Id,
                Quantity = item.quantity,
                Icon = item.detail.Icon,
                BgColor = item.detail.Rarity.color,
                Order = item.detail.Rarity.score
            }).ToList());
        }

        #region builder
        public class Builder
        {
            InventoryView view;
            IInventoryDataSource itemDetails;

            public Builder(InventoryView view)
            {
                this.view = view;
            }

            public Builder InventoryDataSource(IInventoryDataSource itemDetails)
            {
                this.itemDetails = itemDetails;
                return this;
            }

            public InventoryController Build()
            {
                InventoryModel model = itemDetails != null
                ? new InventoryModel(itemDetails.DataSource)
                : new InventoryModel(new ObservableList<ItemInstance>());

                return new InventoryController(view, model);
            }

        }
        #endregion
    }
}