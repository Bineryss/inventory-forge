using System;

namespace UI
{
    public interface IClickableElement<EventType, DataType>
    {
        event Action<EventType> OnClick;
        void Set(DataType data);
    }
}