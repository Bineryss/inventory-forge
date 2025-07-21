using System.Collections.Generic;
using System.Inventory;
using System.Item;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utility;

namespace System.Crafting
{
    public class CraftingManager : SerializedMonoBehaviour
    {
        [SerializeField] private CraftingView view;
        [SerializeField] private InventoryDataSource inventory;
        [SerializeField] private ItemDetailDictionary itemDetailDictionary;
        [SerializeField] private AssetLabelReference recipeRef;
        [OdinSerialize] private ICraftedItem failedItem;
        [OdinSerialize, ReadOnly] private IDataRegistry<ICraftingRecipe> recipeRegistry;

        void Awake()
        {
            recipeRegistry = new DataRegistry<ICraftingRecipe>();
            recipeRegistry.Initialize(recipeRef, (value) => value.GetHashCode().ToString());

            recipeRegistry.OnInitialized += () => new CraftingController.Builder()
            .WithView(view)
            .WithInventoryDataSource(inventory)
            .WithItemDetailService(itemDetailDictionary)
            .WithCraftingService(new CraftingService(recipeRegistry.Registry.Values.ToList(), failedItem))
            .Build();
        }
    }
}