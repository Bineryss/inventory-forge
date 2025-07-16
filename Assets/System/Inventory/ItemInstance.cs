using System.Item;

namespace System.Inventory
{
    [Serializable]
    public class ItemInstance
    {
        private string id;
        public string Id => id == null || "".Equals(id) ? (id = Guid.NewGuid().ToString()) : id; // necessary because auf unity
        public int quantity;
        public ItemDetail detail;

        public ItemInstance(ItemDetail detail) => this.detail = detail;
    }
}