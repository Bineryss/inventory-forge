using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace System.Item
{
    public class ItemDetailDictionary : MonoBehaviour
    {
        private Dictionary<string, ItemDetail> dataSource;
        public Dictionary<string, ItemDetail> DataSource => dataSource ??= itemDetails.ToDictionary(el => el.Id);

        [SerializeField] private List<ItemDetail> itemDetails;

        [ContextMenu("Refresh Data")]
        void RefreshDataSource()
        {
            dataSource = itemDetails.ToDictionary(el => el.Id);
        }
    }
}