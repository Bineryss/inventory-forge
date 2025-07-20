using UnityEngine;

namespace System.Item
{
    [CreateAssetMenu(fileName = "Quality", menuName = "Item/Quality")]
    public class Quality : ScriptableObject, IQuality
    {
        [SerializeField] private string label;
        [SerializeField] private int score;
        [SerializeField] private Color color;

        public int Score => score;

        public Color Color => color;
    }

    public interface IQuality
    {
        int Score { get; }
        Color Color { get; }
    }
}