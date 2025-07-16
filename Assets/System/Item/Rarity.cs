using UnityEngine;

namespace System.Item
{
    [CreateAssetMenu(fileName = "Rarity", menuName = "Items/Rarity")]
    public class Rarity : ScriptableObject
    {
        public int score;
        public Color color;
    }
}