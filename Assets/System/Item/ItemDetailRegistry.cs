using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


namespace System.Item
{
    public class ItemDetailDictionary : SerializedMonoBehaviour, IItemDetailService
    {
        [OdinSerialize] private Dictionary<string, IItemDetail> dataSource;
        public Dictionary<string, IItemDetail> DataSource => dataSource ??= loadedItems.ToDictionary(el => el.Id);
        private readonly List<IItemDetail> loadedItems = new();

        [Header("Data")]
        [SerializeField] private AssetLabelReference itemDetailsRef;

        [ContextMenu("Refresh Data")]
        void RefreshDataSource()
        {
            dataSource = loadedItems.ToDictionary(el => el.Id);
        }

        void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            Debug.Log("🚚 Loading item details...");
            Addressables.LoadAssetsAsync<IItemDetail>(itemDetailsRef, detail =>
            {
                loadedItems.Add(detail);
            }).Completed += HandleInitialization;
        }

        private void HandleInitialization(AsyncOperationHandle<IList<IItemDetail>> handler)
        {
            if (handler.Status == AsyncOperationStatus.Failed)
            {
                Debug.Log("⛔ Failed to load item details");
                return;
            }

            dataSource = handler.Result.ToDictionary(el => el.Id);

            Debug.Log("✅ Initialized item details");
        }

        public bool TryGetValue(string key, out IItemDetail detail)
        {
            if (dataSource.TryGetValue(key, out IItemDetail value))
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

        public bool TryGetValue<T>(string key, out T detail) where T : IItemDetail
        {
            TryGetValue(key, out IItemDetail detailInterface);

            if (detailInterface != null && detailInterface is T t)
            {
                detail = t;
                return true;
            }
            else
            {
                detail = default;
                return false;
            }

        }
    }

    public interface IItemDetailService
    {
        void Initialize();
        bool TryGetValue(string key, out IItemDetail detail);
        bool TryGetValue<T>(string key, out T detail) where T : IItemDetail;
    }
}