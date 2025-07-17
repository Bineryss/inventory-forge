using UnityEngine;
using UnityEngine.UIElements;
namespace System.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private InventoryView view;
        [SerializeField] private Inventory items;
        [SerializeField] private UIDocument root;

        InventoryController controller;

        void Awake()
        {
            view.SetRoot(root.rootVisualElement);
            controller = new InventoryController.Builder(view).InventoryDataSource(items).Build();
        }
    }
}