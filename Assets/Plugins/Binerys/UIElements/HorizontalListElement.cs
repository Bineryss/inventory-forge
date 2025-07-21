using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

namespace UIElements
{
    public class HorizontalListElement<Element, DataType> : VisualElement where Element : VisualElement
    {
        public delegate void UpdateItem(Element element, DataType data);

        private readonly ObjectPool<Element> pool;
        private readonly List<Element> activeElements = new();
        private readonly UpdateItem updateItem;
        public HorizontalListElement(Func<Element> createItem, UpdateItem updateItem, Action<Element> destroyItem, int defaultCapacity = 5, bool wrap = false)
        {
            this.updateItem = updateItem;
            VisualElement row = new();
            row.style.flexDirection = FlexDirection.Row;
            row.style.flexWrap = wrap ? Wrap.Wrap : Wrap.NoWrap;
            row.style.marginBottom = 4;
            Add(row);

            pool = new(
            createItem,
            item => { row.Add(item); item.style.display = DisplayStyle.None; activeElements.Add(item); },
            item => { row.Remove(item); activeElements.Remove(item); },
            destroyItem,
            true,
            defaultCapacity,
            2
            );
        }

        public void Set(IList<DataType> data)
        {
            if (data == null)
            {
                activeElements.ForEach(element => pool.Release(element));
                return;
            }

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
                element.style.display = DisplayStyle.Flex;
            }
        }
    }
}