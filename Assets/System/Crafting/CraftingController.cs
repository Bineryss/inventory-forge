
using System.Collections;
using System.Collections.Generic;
using System.Inventory;
using System.Linq;
using UnityEngine;

namespace System.Crafting
{

    public class CraftingController
    {
        // private readonly CraftingModel model;
        private readonly CraftingView view;

        public CraftingController(CraftingView view)
        {
            Debug.Assert(view != null, "View is null");
            // Debug.Assert(model != null, "Model is null");
            this.view = view;

            view.StartCoroutine(Initialize());
        }

        IEnumerator Initialize()
        {
            yield return view.InitializeView();

            view.UpdateInventory(new List<ItemDisplayData>()
            {
                new() {
                    Id = "abc",
                    Icon = "T",
                    Quantity = 10,
                },
                new() {
                    Id = "cde",
                    Icon = "S",
                    Quantity = 5,
                }
            });

            view.OnCraftingClick += (value) =>
            {
                Debug.Log($"selected lenght: {value.Count}");
                foreach (string v in value)
                {
                    Debug.Log($"selected: {v}");
                }
            };
        }
    }
}