using System.Crafting;
using System.Inventory;
using System.Item;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utility;

public class Bootstraper : MonoBehaviour
{
    [Header("Inventory")]
    [OdinSerialize, ReadOnly] private DataRegistry<IItemDetail> itemDetails = new();
    [SerializeField] private AssetLabelReference itemDetailRef;
    [SerializeField] private InventoryManager inventoryManager;

    [Header("Crafting")]
    [OdinSerialize, ReadOnly] private DataRegistry<ICraftingRecipe> recipes = new();
    [SerializeField] private AssetLabelReference recipeRef;
    [SerializeField] private CraftingManager craftingManager;

    void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        recipes.Initialize(recipeRef, value => value.GetHashCode().ToString());
        itemDetails.Initialize(itemDetailRef, value => value.Id);
        inventoryManager.Initialize();
        craftingManager.Initialize(itemDetails, recipes);
    }

}