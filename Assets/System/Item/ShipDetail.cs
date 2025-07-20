using System.Item;
using UnityEngine;

[CreateAssetMenu(fileName = "Ship Detail", menuName = "Item/Ship Detail")]
public class ShipDetail : ItemDetail
{
    [Header("Stats")]
    [SerializeField] private int someStats;
}
