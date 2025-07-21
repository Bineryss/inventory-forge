using System.Item;
using UnityEngine;
using Utility;
namespace System.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private InventoryView view;
        [SerializeField] private InventoryDataSource items;

        InventoryController controller;

        public void Initialize()
        {
            controller = new InventoryController.Builder(view).InventoryDataSource(items).Build();
        }
    }
}