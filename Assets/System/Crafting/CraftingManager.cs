using System.Inventory;
using System.Item;
using UnityEngine;

namespace System.Crafting
{
    public class CraftingManager : MonoBehaviour
    {
        [SerializeField] private CraftingView view;
        [SerializeField] private InventoryDataSource inventory;
        [SerializeField] private ItemDetailDictionary itemDetailDictionary;

        void Awake()
        {
            new CraftingController.Builder()
            .WithView(view)
            .WithInventoryDataSource(inventory)
            .WithItemDetailService(itemDetailDictionary)
            .Build();
        }
    }
}