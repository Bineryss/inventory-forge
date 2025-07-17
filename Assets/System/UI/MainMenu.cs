using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace System.UI
{
    public class MainMenu : MonoBehaviour, IView
    {
        public delegate void TransitionMethod(UIState newState);
        [SerializeField] private VisualTreeAsset asset;

        private VisualElement root;

        public IEnumerator InitializeView(TransitionMethod transitionMethod)
        {
            root ??= new();
            root.Clear();

            asset.CloneTree(root);

            root.Q<Button>("inventory").clicked += () => transitionMethod(UIState.INVENTORY);
            root.Q<Button>("crafting").clicked += () => transitionMethod(UIState.CRAFTING);

            yield return null;
        }


        public void Hide()
        {
            root.style.display = DisplayStyle.None;
        }

        public void Mount(VisualElement root)
        {
            this.root ??= new();
            root.Add(this.root);
        }

        public void Show()
        {
            root.style.display = DisplayStyle.Flex;
        }
    }
}