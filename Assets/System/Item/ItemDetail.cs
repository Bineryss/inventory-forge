using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility;

namespace System.Item
{
    [CreateAssetMenu(fileName = "ItemDetail", menuName = "Item/ItemDetail")]
    public class ItemDetail : ScriptableObjectWithId, IItemDetail
    {
        [Header("UI")]
        [SerializeField] private string icon;
        [TextArea, SerializeField]
        public string description;

        [Header("Gameplay")]
        [SerializeField] private Quality rarity;

        public string Id => id;
        public string Name => name;

        public string Icon => icon;
        public string Description => description;
        public IQuality Quality => rarity;
    }

    public interface IItemDetail
    {
        string Id { get; }
        string Name { get; }
        string Icon { get; }
        string Description { get; }
        IQuality Quality { get; }
    }
}