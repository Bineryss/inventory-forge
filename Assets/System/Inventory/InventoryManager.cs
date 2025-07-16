using System.Collections.Generic;
using UnityEngine;
namespace System.Inventory
{
    public class InventoryManager: MonoBehaviour
    {
        [SerializeField] private InventoryView view;
        [SerializeField] private Inventory items;

        InventoryController controller;

        void Awake()
        {
            controller = new InventoryController.Builder(view).InventoryDataSource(items).Build();
        }
    }
}