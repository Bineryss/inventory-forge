using UnityEngine;
using Utility;

namespace System.Item
{

    [CreateAssetMenu(fileName = "Trait", menuName = "Item/Trait")]
    public class Trait : ScriptableObjectWithId, ITrait
    {

        [Header("UI")]
        [SerializeField] private string label;
        [SerializeField, TextArea] private string description;
        [SerializeField] private Color color;

        public string Id => id;
        public string Label => label;
        public Color Color => color;
        public string Description => description;
    }

    public interface ITrait
    {
        string Id { get; }
        string Label { get; }
        Color Color { get; }
        string Description { get; }
    }
}
