using System;
using System.Collections;
using System.Collections.Generic;
using System.Inventory;
using UnityEngine;

public class TestScirpt : MonoBehaviour
{
    [SerializeField] private InventoryDataSource inventory;

    void Awake()
    {
        inventory.DataSource.ValueChanged += HandleValueChanged;
    }

    private void HandleValueChanged(IList<ItemInstance> data)
    {
        Debug.Log("Value Changed in Inventory");
        foreach (var item in data)
        {
            Debug.Log($"🚀 {item.Detail.Icon}-{item.quantity}");
        }
    }
}