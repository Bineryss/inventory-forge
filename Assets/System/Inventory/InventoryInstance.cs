using System.Collections.Generic;
using System.Item;
using System.Linq;
using UnityEngine;

namespace System.Inventory
{
    [Serializable]
    public class ItemInstance
    {
        private string id;
        public string Id => id == null || "".Equals(id) ? (id = Guid.NewGuid().ToString()) : id; // necessary because of unity
        [SerializeField] private IItemDetail detail;

        public IItemDetail Detail => detail;
        [field: SerializeField] public int Quantity { get; set; } = 1;
        [SerializeField] private List<ITrait> traits = new();
        public List<ITrait> Traits
        {
            get
            {
                if (detail is ResourceDetail) return (detail as ResourceDetail).Traits.ToList();

                return traits;
            }
            set
            {
                if (detail is ResourceDetail) return;

                traits = value;
            }
        }

        public ItemInstance() { }
        public ItemInstance(IItemDetail detail)
        {
            this.detail = detail;
        }

    }

}