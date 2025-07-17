using System.Collections.Generic;
using UnityEngine;

namespace System.Inventory
{

    public interface IInventoryDataSource
    {
        ObservableList<ItemInstance> DataSource { get; }
    }

    public class InventoryDataSource : MonoBehaviour, IInventoryDataSource
    {
        private static ObservableList<ItemInstance> dataSource;

        [SerializeField] private List<ItemInstance> inventory = new();
        public ObservableList<ItemInstance> DataSource => dataSource ??= new ObservableList<ItemInstance>(inventory);

        [ContextMenu("Refresh Data")]
        void RefreshDataSource()
        {
            dataSource.Invoke();
        }
    }


}