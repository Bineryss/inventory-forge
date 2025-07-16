using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

namespace System.Inventory
{
    public class InventoryRow : VisualElement
    {
        public event Action<string> OnElementClick = delegate { };

        private readonly ObjectPool<ItemDisplay> pool;
        private readonly List<ItemDisplay> activeElements = new();
        public InventoryRow(int defaultCapacity = 5)
        {
            VisualElement row = new();
            row.style.flexDirection = FlexDirection.Row;
            row.style.marginBottom = 4;
            Add(row);

            pool = new(
            () =>
            {
                ItemDisplay element = new();
                row.Add(element);
                element.style.marginRight = 4;
                element.OnClick += (value) => OnElementClick.Invoke(value);
                return element;
            },
            item => item.style.visibility = Visibility.Visible,
            item => item.style.visibility = Visibility.Hidden,
            item => row.Remove(item), true, defaultCapacity, 20
            );
        }

        public void Set(IList<ItemDisplayData> data)
        {
            //pooling refactorn!
            int minLength = Math.Min(activeElements.Count, data.Count);
            Debug.Log($"set minLength: {minLength}, activeElements: {activeElements.Count}, data: {data.Count}");

            for (int i = 0; i < minLength; i++)
            {
                Debug.Log($"no new {i}");
                activeElements[i].Set(data[i]);
            }

            for (int i = minLength; i < activeElements.Count; i++)
            {
                Debug.Log($"remove {i}");
                ItemDisplay element = activeElements[i];
                pool.Release(element);
                activeElements.Remove(element);
            }

            for (int i = minLength; i < data.Count; i++)
            {
                Debug.Log($"add {i}");
                ItemDisplay element = pool.Get();
                element.Set(data[i]);
                activeElements.Add(element);
            }
        }

    }
}