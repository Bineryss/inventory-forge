using System.Collections.Generic;
using System.Item;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace System.Crafting
{

    [CreateAssetMenu(fileName = "Recipe", menuName = "Scriptable Objects/Recipe")]
    public class Recipe : SerializedScriptableObject, ICraftingRecipe
    {
        [Header("Requirements")]
        [OdinSerialize] private readonly Dictionary<ICharacteristic, int> requiredCharacteristics = new();
        [SerializeField] private int minimumQuality;
        [Header("Result")]
        [OdinSerialize] private IItemDetail outputData;



        public Dictionary<ICharacteristic, int> RequiredCharacteristics => requiredCharacteristics;
        public int MinimumQuality => minimumQuality;
        public IItemDetail OutputData => outputData;
        public CraftedItemType OutputType
        {
            get
            {
                return CraftedItemType.SHIP;
            }
        }
    }

    public interface ICraftingRecipe
    {
        Dictionary<ICharacteristic, int> RequiredCharacteristics { get; }
        int MinimumQuality { get; }
        IItemDetail OutputData { get; }
        CraftedItemType OutputType { get; }
    }
}
