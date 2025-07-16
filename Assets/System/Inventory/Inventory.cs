using System.Collections.Generic;
using UnityEngine;

namespace System.Inventory
{

    public interface IInventoryDataSource
    {
        ObservableList<ItemInstance> DataSource { get; }
    }

    public class Inventory : MonoBehaviour, IInventoryDataSource
    {
        private static ObservableList<ItemInstance> dataSource;

        [SerializeField] private List<ItemInstance> inventory = new();
        public ObservableList<ItemInstance> DataSource => dataSource == null ? dataSource = new ObservableList<ItemInstance>(inventory) : dataSource;
    }


}