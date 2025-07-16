using System.Collections.Generic;
using UnityEngine;
namespace System.Inventory
{
    public class Inventory: MonoBehaviour
    {
        [SerializeField] InventoryView view;
        [SerializeField] List<ItemInstance> startingItems = new();

        InventoryController controller;

        void Awake()
        {
            controller = new InventoryController.Builder(view).Items(startingItems).Build();
        }
    }
}