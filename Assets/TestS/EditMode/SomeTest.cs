using System.Collections.Generic;
using System.Item;
using UnityEngine;

public class Items
{
    public void Mth()
    {
        ItemDetail itemDetail = ScriptableObject.CreateInstance<ItemDetail>();
        Trait trait = ScriptableObject.CreateInstance<Trait>();
        List<Trait> traits = new();

        IItemDetail t1 = itemDetail;

        ITrait t2 = trait;
    }
}