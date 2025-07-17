using System.Collections.Generic;
using System.Inventory;
using UnityEngine;
using UnityEngine.UIElements;

namespace System.UI
{

    public class UIManager : MonoBehaviour
    {
        [Header("Document")]
        [SerializeField] private UIDocument document;
        [SerializeField] private StyleSheet styleSheet;

        [Header("Inventory")]
        [SerializeField] private InventoryView inventoryView;


        private Dictionary<UIState, IView> views;
        private UIState current = UIState.MENU;
        private Button BackButton => document.rootVisualElement.Q<Button>("to-menu");

        void Start()
        {
            VisualElement rootContainer = document.rootVisualElement.Q("container");

            views = new()
            {
                {UIState.INVENTORY, inventoryView},
            };

            foreach (IView view in views.Values)
            {
                view.Mount(rootContainer);
                view.Hide();
            }

            BackButton.clicked += () => TransitionTo(UIState.MENU);

            TransitionTo(UIState.INVENTORY);
        }

        public void TransitionTo(UIState newState)
        {
            if (current == newState) return;

            views.TryGetValue(current, out IView currentView);
            currentView?.Hide();
            views.TryGetValue(newState, out IView nextView);
            nextView?.Show();
            current = newState;
        }

    }

    public interface IView
    {
        void Mount(VisualElement root);
        // void Unmount();
        void Show();
        void Hide();
    }

    public enum UIState
    {
        MENU,
        INVENTORY,
        CRAFTING
    }
}