using System.Inventory;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [Header("Document")]
    [SerializeField] private UIDocument document;
    [SerializeField] private StyleSheet styleSheet;

    [Header("Inventory")]
    [SerializeField] private IInventoryDataSource inventoryDataSource;
    // [SerializeField] private 
}