using System.Inventory;
using UnityEngine;

namespace System.Crafting
{
    public class CraftingManager : MonoBehaviour
    {
        [SerializeField] private CraftingView view;
        [SerializeField] private InventoryDataSource inventory;

        void Awake()
        {
            CraftingModel model = new(inventory.DataSource);
            new CraftingController(view, model);
        }
    }
}