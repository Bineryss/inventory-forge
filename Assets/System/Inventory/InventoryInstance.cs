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
        public int quantity = 1;
        [SerializeField] private ItemDetail detail;

        public IItemDetail Detail => detail;

        public ItemInstance(IItemDetail detail) => this.detail = detail as ItemDetail;
        public ItemInstance(ItemInstance copy)
        {
            id = copy.Id;
            quantity = copy.quantity;
            detail = copy.detail;
        }

        //future
        private List<ITrait> traits;
        private ItemInstance equipedWeapon; //only viable if type == SHIP
                                            //    private Type type => detail.Type
    }

}