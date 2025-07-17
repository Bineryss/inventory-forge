using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace System.Inventory
{
    [UxmlElement]
    public partial class ItemDetailElement : VisualElement
    {
        static readonly VisualTreeAsset asset = UnityEngine.Resources.Load<VisualTreeAsset>("item-detail");
        private VisualElement EffectListContainer => this.Q<VisualElement>("effect-list");
        private Label Icon => this.Q<Label>("icon");
        private Label Name => this.Q<Label>("name");
        private Label Description => this.Q<Label>("description");
        private VisualElement Background => this.Q<VisualElement>("background");

        private readonly HorizontalListElement<TagElement, TagDisplayData> horizontalList;

        public ItemDisplayDetailData Data
        {
            set
            {
                Icon.text = value.Icon;
                Name.text = value.Name;
                Description.text = value.Description;
                Background.style.backgroundColor = value.BgColor;
                horizontalList.Set(value.Effects);
            }
        }

        public ItemDetailElement()
        {
            if (asset == null)
            {
                Debug.Log("⛔ Failed to load uxml file for Item!");
                return;
            }

            asset.CloneTree(this);

            horizontalList = new(() =>
            {
                TagElement element = new();
                element.style.marginRight = 4;
                return element;
            },
            (element, data) =>
            {
                element.Set(data);
            }
            );

            EffectListContainer.Add(horizontalList);
        }
    }
}