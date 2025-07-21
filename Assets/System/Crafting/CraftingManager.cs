using System.Inventory;
using System.Item;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Utility;

namespace System.Crafting
{
    public class CraftingManager : SerializedMonoBehaviour
    {
        [SerializeField] private CraftingView view;
        [SerializeField] private InventoryDataSource inventory;
        [OdinSerialize] private ICraftedItem failedItem;

        private bool itemDetailRegInit;
        private bool recipeRegInit;

        public void Initialize(IDataRegistry<IItemDetail> itemDetailRegistry, IDataRegistry<ICraftingRecipe> recipeRegistry)
        {
            recipeRegistry.OnInitialized += () => { recipeRegInit = true; HandleInit(itemDetailRegistry, recipeRegistry); };
            itemDetailRegistry.OnInitialized += () => { itemDetailRegInit = true; HandleInit(itemDetailRegistry, recipeRegistry); };
        }

        private void HandleInit(IDataRegistry<IItemDetail> itemDetailRegistry, IDataRegistry<ICraftingRecipe> recipeRegistry)
        {
            if (!(itemDetailRegInit && recipeRegInit)) return;

            new CraftingController.Builder()
                        .WithView(view)
                        .WithInventoryDataSource(inventory)
                        .WithItemDetailService(itemDetailRegistry)
                        .WithCraftingService(new CraftingService(recipeRegistry.Registry.Values.ToList(), failedItem))
                        .Build();
        }
    }
}