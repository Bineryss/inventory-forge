using System.Item;
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

        public ItemInstance() { }
        public ItemInstance(IItemDetail detail)
        {
            this.detail = detail;
        }

        //future
        // private List<ITrait> traits;
        // private ItemInstance equipedWeapon; //only viable if type == SHIP
        //    private Type type => detail.Type
    }

}