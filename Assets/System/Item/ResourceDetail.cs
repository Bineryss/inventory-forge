using System.Collections.Generic;
using System.Crafting;
using System.Item;
using UnityEngine;

[CreateAssetMenu(fileName = "Resource Detail", menuName = "Item/Resource Detail")]
public class ResourceDetail : ItemDetail, ICraftingResource
{
    [Header("Crafting")]
    [SerializeField] private Characteristic characteristic;
    [SerializeField] private List<ITrait> traits = new();

    public IEnumerable<ITrait> Traits => traits;
    public ICharacteristic Characteristic => characteristic;
    public int QualityScore => Quality.Score;
}
