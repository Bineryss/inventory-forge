using System;
using UnityEngine.UIElements;

namespace UI
{
    public interface HorizontalListItem<EventType, DataType>
    {
        VisualElement CreateElement();
        event Action<EventType> OnClick;
        void Set(DataType data);
    }
}