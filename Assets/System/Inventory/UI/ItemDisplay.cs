using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace System.Inventory
{
    [UxmlElement]
    public partial class ItemDisplay : VisualElement
    {
        private readonly Button container;
        private readonly Label icon;
        private readonly Label quantity;

        public event Action<string> OnClick = delegate { };

        private string id;

        public ItemDisplay()
        {
            container = new();
            container.style.backgroundColor = Color.white;
            container.clicked += HandleClick;
            Add(container);

            icon = new() { text = "#" };
            container.Add(icon);

            quantity = new() { text = "#000" };
            container.Add(quantity);
        }

        public void Set(string id, string icon, Color bgColor, int quantity)
        {
            this.id = id;
            this.icon.text = icon;
            container.style.backgroundColor = bgColor;
            this.quantity.text = quantity.ToString();
        }

        public void Set(ItemDisplayData data)
        {
            id = data.Id;
            icon.text = data.Icon;
            container.style.backgroundColor = data.BgColor;
            quantity.text = data.Quantity.ToString();
        }

        private void HandleClick()
        {
            Debug.Log($"element clicked {id}");
            OnClick.Invoke(id);
        }
    }
}