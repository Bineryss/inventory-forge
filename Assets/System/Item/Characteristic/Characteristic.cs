using Unity.Properties;
using UnityEngine;
using Utility;

namespace System.Item
{

    [CreateAssetMenu(fileName = "Characteristic", menuName = "Item/Characteristic")]
    public class Characteristic : ScriptableObjectWithId, ICharacteristic
    {

        [SerializeField, TextArea] private string description;
        [SerializeField] private bool ignoreInRecipe;

        public string Id => id;

        public bool Ignore => ignoreInRecipe;
    }

    public interface ICharacteristic
    {
        string Id { get; }
        bool Ignore { get; }
    }
}
