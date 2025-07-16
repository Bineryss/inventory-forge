using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace System.Inventory
{
    public abstract class StorageView : MonoBehaviour
    {
        public List<ItemDisplay> Slots;

        [SerializeField] protected UIDocument document;
        [SerializeField] protected StyleSheet styleSheet;

        protected VisualElement Root => document.rootVisualElement;

        public abstract IEnumerator InitializeView();
    }
}