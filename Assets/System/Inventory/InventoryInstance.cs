using System.Collections.Generic;
using System.Crafting;
using System.Item;
using System.Linq;
using UnityEngine;

namespace System.Inventory
{
    [Serializable]
    public class ItemInstance: ICraftedItem
    {
        private string id;
        public string Id => id == null || "".Equals(id) ? (id = Guid.NewGuid().ToString()) : id; // necessary because of unity
        [SerializeField] private IItemDetail detail;

        public IItemDetail Detail => detail;
        [field: SerializeField] public int Quantity { get; set; } = 1;
        [SerializeField] private IEnumerable<ITrait> traits = default;
        public IEnumerable<ITrait> Traits
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

        public IItemDetail Data => throw new NotImplementedException();

        public CraftingOutcomeTier Tier => throw new NotImplementedException();

        public CraftedItemType Type => throw new NotImplementedException();

        public ItemInstance() { }
        public ItemInstance(IItemDetail detail)
        {
            this.detail = detail;
        }

    }

}