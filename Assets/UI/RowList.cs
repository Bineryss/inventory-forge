using System;
using System.Collections.Generic;
using UnityEngine.Pool;
using UnityEngine.UIElements;

namespace UI
{
    public class HorizontalList<Element, DataType> : VisualElement where Element : VisualElement
    {
        public delegate Element CreateItem();
        public delegate void UpdateItem(Element element, DataType data);

        private readonly ObjectPool<Element> pool;
        private readonly List<Element> activeElements = new();
        private readonly UpdateItem updateItem;
        public HorizontalList(CreateItem makeItem, UpdateItem updateItem, int defaultCapacity = 5)
        {
            this.updateItem = updateItem;
            VisualElement row = new();
            row.style.flexDirection = FlexDirection.Row;
            row.style.marginBottom = 4;
            Add(row);

            pool = new(
            () =>
            {
                Element element = makeItem();
                row.Add(element);
                return element;
            },
            item => { item.style.display = DisplayStyle.Flex; activeElements.Add(item); },
            item => { item.style.display = DisplayStyle.None; activeElements.Remove(item); },
            item => row.Remove(item), true, defaultCapacity, 20
            );
        }

        public void Set(IList<DataType> data)
        {
            int minLength = Math.Min(activeElements.Count, data.Count);

            for (int i = 0; i < minLength; i++)
            {
                updateItem(activeElements[i], data[i]);
            }

            for (int i = minLength; i < activeElements.Count; i++)
            {
                var element = activeElements[i];
                pool.Release(element);
            }

            for (int i = minLength; i < data.Count; i++)
            {
                var element = pool.Get();
                updateItem(element, data[i]);
            }
        }

    }
}