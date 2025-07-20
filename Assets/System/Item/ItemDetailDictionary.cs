using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


namespace System.Item
{
    public class ItemDetailDictionary : MonoBehaviour, IItemDetailService
    {
        private Dictionary<string, IItemDetail> dataSource;
        public Dictionary<string, IItemDetail> DataSource => dataSource ??= loadedItems.ToDictionary(el => el.Id);
        private readonly List<IItemDetail> loadedItems = new();

        [Header("Data")]
        [SerializeField] private AssetLabelReference itemDetailsRef;
        [SerializeField] private List<ItemDetail> loadedItemsDisplay = new(); //only for editor, maybe build custom property drawer in futur
        [SerializeField] private List<ShipDetail> loadedShipsDisplay = new(); //only for editor, maybe build custom property drawer in futur

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
            loadedItemsDisplay = loadedItems.Cast<ItemDetail>().ToList();
            loadedShipsDisplay = loadedItemsDisplay.Where(el => el is ShipDetail).Select(el => el as ShipDetail).ToList();

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