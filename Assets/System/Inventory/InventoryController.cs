using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
            model.SelectItem(id);
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
            }).ToList());

            var selected = model.selectedItem;
            if (selected == null)
            {
                view.UpdateDetailData(new ItemDisplayDetailData());
            }
            else
            {
                view.UpdateDetailData(new ItemDisplayDetailData()
                {
                    Id = selected.Id,
                    Icon = selected.detail.Icon,
                    BgColor = selected.detail.Rarity.color,
                    Name = selected.detail.Name,
                    Description = selected.detail.Description
                });
            }
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