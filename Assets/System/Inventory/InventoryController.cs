using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace System.Inventory
{
    public class InventoryController
    {
        private readonly InventoryModel model;
        private readonly InventoryView view;

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
                displayData.Icon = selected.Detail.Icon;
                displayData.BgColor = selected.Detail.Quality.Color;
                displayData.Name = selected.Detail.Name;
                displayData.Description = selected.Detail.Description;
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
                Quantity = item.Quantity,
                Icon = item.Detail.Icon,
                BgColor = item.Detail.Quality.Color,
                Order = item.Detail.Quality.Score
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