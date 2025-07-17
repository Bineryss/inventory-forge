using UnityEngine;

namespace System.Item
{
    [CreateAssetMenu(fileName = "ItemDetail", menuName = "Items/ItemDetail")]
    public class ItemDetail : ScriptableObject
    {
        [SerializeField] private string id = Guid.NewGuid().ToString();
        public string Id => id;
        public string Name => name;

        [Header("UI")]
        public string Icon;
        [TextArea]
        public string Description;

        [Header("Gameplay")]
        public Rarity Rarity;


        private void OnValidate()
        {
            if (string.IsNullOrEmpty(Id))
            {
                id = Guid.NewGuid().ToString();
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }
    }
}