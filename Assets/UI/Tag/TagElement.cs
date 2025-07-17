using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class TagElement : VisualElement
    {
        private static readonly VisualTreeAsset asset = Resources.Load<VisualTreeAsset>("tag");

        private Label Label => this.Q<Label>("label");
        private VisualElement Background => this.Q<VisualElement>("background");

        public TagElement()
        {
            if (asset == null)
            {
                Debug.Log("⛔ Failed to load uxml file for TagElement!");
            }
            asset.CloneTree(this);
        }

        public void Set(TagDisplayData data)
        {
            Label.text = data.Label;
            Background.style.backgroundColor = new Color(data.Color.r, data.Color.g, data.Color.b, 0.4f);
            Color borderColor = new(data.Color.r, data.Color.g, data.Color.b, 1f);
            Background.style.borderLeftColor = borderColor;
            Background.style.borderTopColor = borderColor;
            Background.style.borderRightColor = borderColor;
            Background.style.borderBottomColor = borderColor;
        }
    }

    public class TagDisplayData
    {
        public string Label;
        public Color Color = Color.pink;
    }
}