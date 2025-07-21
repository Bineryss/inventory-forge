using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Utility
{
    public class DataRegistry<T> : IDataRegistry<T>
    {
        public event Action OnInitialized = delegate { };
        [SerializeField] private readonly Dictionary<string, T> registry = new();
        public Dictionary<string, T> Registry => registry;

        [ContextMenu("Refresh Data")]
        public void Initialize(AssetLabelReference reference, IDataRegistry<T>.ConvertToId converter)
        {
            registry.Clear();
            Debug.Log("🚚 Loading assets...");
            Addressables.LoadAssetsAsync<T>(reference, detail =>
            {
                Debug.Log($"adding {converter(detail)}-{detail}");
                registry.Add(converter(detail), detail);
            }).Completed += HandleInitialization;
        }

        private void HandleInitialization(AsyncOperationHandle<IList<T>> handler)
        {
            if (handler.Status == AsyncOperationStatus.Failed)
            {
                Debug.Log("⛔ Failed to load assets");
                return;
            }
            OnInitialized.Invoke();

            Debug.Log($"✅ Initialized assets registry for {typeof(T)}");
        }

        public bool TryGetValue(string key, out T detail)
        {
            if (registry.TryGetValue(key, out T value))
            {
                detail = value;
                return true;
            }
            else
            {
                detail = default;
                return false;
            }
        }

        public bool TryGetValue<K>(string key, out K detail) where K : T
        {
            TryGetValue(key, out T detailInterface);

            if (detailInterface != null && detailInterface is K k)
            {
                detail = k;
                return true;
            }
            else
            {
                detail = default;
                return false;
            }

        }
    }

    public interface IDataRegistry<T>
    {
        delegate string ConvertToId(T element);
        Dictionary<string, T> Registry { get; }
        event Action OnInitialized;
        void Initialize(AssetLabelReference reference, ConvertToId converter);
        bool TryGetValue(string key, out T detail);
        bool TryGetValue<O>(string key, out O detail) where O : T;
    }
}