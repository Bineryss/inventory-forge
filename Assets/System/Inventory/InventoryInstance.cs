using System.Collections.Generic;
using System.Item;
using UnityEngine;

namespace System.Inventory
{
    [Serializable]
    public class ItemInstance
    {
        private string id;
        public string Id => id == null || "".Equals(id) ? (id = Guid.NewGuid().ToString()) : id; // necessary because of unity
        [SerializeField] public int quantity = 1;
        [SerializeField] private IItemDetail detail;

        public IItemDetail Detail => detail;
        public int Quantity => quantity;

        public ItemInstance() { }
        public ItemInstance(IItemDetail detail, int quantity = 1)
        {
            this.detail = detail;
            this.quantity = quantity;
        }

        //future
        private List<ITrait> traits;
        private ItemInstance equipedWeapon; //only viable if type == SHIP
                                            //    private Type type => detail.Type
    }

}