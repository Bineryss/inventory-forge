using System.Collections.Generic;
using System.Crafting;
using System.Item;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Resource Detail", menuName = "Item/Resource Detail")]
public class ResourceDetail : ItemDetail, ICraftingResource
{
    [Header("Crafting")]
    [SerializeField] private Characteristic characteristic;
    [SerializeField] private List<Trait> traits;

    public IEnumerable<ITrait> Traits => traits.Cast<ITrait>().ToList(); //TODO write custom editor so i can use interface for props
    public ICharacteristic Characteristic => characteristic;
    public int QualityScore => Quality.Score;
}
