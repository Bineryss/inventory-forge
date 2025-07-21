using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace System.Inventory
{

    public interface IInventoryDataSource
    {
        ObservableList<ItemInstance> DataSource { get; }
    }

    public class InventoryDataSource : SerializedMonoBehaviour, IInventoryDataSource
    {
        private static ObservableList<ItemInstance> dataSource;

        [OdinSerialize] private List<ItemInstance> inventory = new();
        public ObservableList<ItemInstance> DataSource => dataSource ??= new ObservableList<ItemInstance>(inventory);

        [ContextMenu("Refresh Data")]
        void RefreshDataSource()
        {
            dataSource.Invoke();
        }
    }
}