using System.Collections.Generic;
using System.Item;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class Items
{
    [Test]
    public void Mth()
    {
        IItemDetail itemDetail = ScriptableObject.CreateInstance<ItemDetail>();
        Trait trait = ScriptableObject.CreateInstance<Trait>();
        List<Trait> traits = new();

        IItemDetail t1 = itemDetail;

        ITrait t2 = trait;


        ShipDetail detail = ScriptableObject.CreateInstance<ShipDetail>();
        detail.shipData = "shipasda";

        IItemDetail idetail = detail;

        ShipDetail casted = idetail as ShipDetail;
        Debug.Log(casted.shipData);
    }
}