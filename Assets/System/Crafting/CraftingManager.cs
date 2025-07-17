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
            CraftingModel model = new(inventory.DataSource);
            new CraftingController(view, model, itemDetailDictionary);
        }
    }
}