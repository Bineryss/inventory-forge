using UnityEngine;
namespace System.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private InventoryView view;
        [SerializeField] private InventoryDataSource items;

        InventoryController controller;

        void Awake()
        {
            controller = new InventoryController.Builder(view).InventoryDataSource(items).Build();
        }
    }
}