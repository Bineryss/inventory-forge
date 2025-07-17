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

        [Header("Views")]
        [SerializeField] private InventoryView inventoryView;
        [SerializeField] private MainMenu menuView;


        private Dictionary<UIState, IView> views;
        private UIState current;
        private Button BackButton => document.rootVisualElement.Q<Button>("to-menu");

        void Start()
        {
            StartCoroutine(menuView.InitializeView(TransitionTo));
            VisualElement rootContainer = document.rootVisualElement.Q("container");

            views = new()
            {
                {UIState.INVENTORY, inventoryView},
                {UIState.MENU, menuView},
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

            if (!views.ContainsKey(newState))
            {
                Debug.Log("This view dosn't exist!");
                return;
            }

            views[current].Hide();
            views[newState].Show();
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