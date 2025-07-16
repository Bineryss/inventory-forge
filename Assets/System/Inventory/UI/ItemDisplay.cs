using UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace System.Inventory
{
    [UxmlElement]
    public partial class ItemDisplay : VisualElement, HorizontalListItem<string, ItemDisplayData>
    {
        static readonly VisualTreeAsset asset = UnityEngine.Resources.Load<VisualTreeAsset>("item");

        private Button Container => this.Q<Button>("container");
        private Label Icon => this.Q<Label>("icon");
        private Label Quantity => this.Q<Label>("quantity");

        public event Action<string> OnClick = delegate { };

        private string id;

        public ItemDisplay()
        {
            if (asset == null)
            {
                Debug.Log("⛔ Failed to load uxml file for Item!");
                return;
            }

            asset.CloneTree(this);

            Container.clicked += HandleClick;
        }

        public void Set(string id, string icon, Color bgColor, int quantity)
        {
            this.id = id;
            Icon.text = icon;
            Container.style.backgroundColor = bgColor;
            Quantity.text = quantity.ToString();
        }

        public void Set(ItemDisplayData data)
        {
            Set(data.Id, data.Icon, data.BgColor, data.Quantity);
        }

        private void HandleClick()
        {
            OnClick.Invoke(id);
        }

        public VisualElement CreateElement() => new();
    }
}