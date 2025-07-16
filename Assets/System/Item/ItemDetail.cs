using UnityEngine;

namespace System.Item
{
    [CreateAssetMenu(fileName = "ItemDetail", menuName = "Items/ItemDetail")]
    public class ItemDetail : ScriptableObject
    {
        public string icon;
        public Rarity rarity;
    }
}
