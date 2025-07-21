using System.Collections.Generic;
using System.Item;
using System.Linq;
using UnityEngine;

namespace System.Crafting
{
    public class CraftingService
    {
        private readonly Dictionary<string, ICraftingRecipe> recipeLookup;
        private readonly ICraftedItem failedCrafting;
        private readonly Dictionary<CraftedItemType, Func<IItemDetail, int, int, List<ITrait>, ICraftedItem>> itemCreation;


        public CraftingService(List<ICraftingRecipe> recipes, ICraftedItem failedCrafting)
        {
            recipeLookup = recipes.ToDictionary(recipe => ToSignature(recipe.RequiredCharacteristics), recipe => recipe);
            this.failedCrafting = failedCrafting;

            itemCreation = new()
            {
                {CraftedItemType.SHIP, CreateShip}
            };
        }

        private ICraftedItem CreateShip(IItemDetail detail, int requiredMinQuality, int quality, List<ITrait> traits)
        {
            CraftingOutcomeTier tier = requiredMinQuality <= quality ? CraftingOutcomeTier.STABLE : CraftingOutcomeTier.BRITTEL;
            return new ShipInstance()
            {
                Data = detail,
                Traits = traits,
                Tier = tier,
            };
        }


        public ICraftedItem Evaluate(Dictionary<ICraftingResource, int> materials)
        {
            if (materials == null) return failedCrafting;

            string materialSignature = ToSignature(materials
            .Where(entry => entry.Key.Characteristic != null)
            .GroupBy(entry => entry.Key.Characteristic)
            .ToDictionary(group => group.Key, group => group.Sum(entry => entry.Value)));

            if (!recipeLookup.TryGetValue(materialSignature, out ICraftingRecipe recipe))
            {
                Debug.Log("Couldn't find a matching recipe");
                return failedCrafting;
            }

            if (!itemCreation.TryGetValue(recipe.OutputType, out var createItem))
            {
                Debug.Log("Couldn't find a matching creation method");
                return failedCrafting;
            }

            int evaluatedQuality = materials.Keys.Sum(resource => resource.QualityScore) / materials.Count();
            List<ITrait> collectedTrait = materials.Keys.SelectMany(resource => resource.Traits).Distinct().ToList();

            ICraftedItem item = createItem(recipe.OutputData, recipe.MinimumQuality, evaluatedQuality, collectedTrait);
            return item;
        }


        private string ToSignature(Dictionary<ICharacteristic, int> characteristics)
        {
            return string.Join("-", characteristics.OrderBy(entry => entry.Key.Id).Select(entry => $"{entry.Key.Id}:{entry.Value}"));
        }
    }

    public interface ICraftingResource
    {
        ICharacteristic Characteristic { get; }
        IEnumerable<ITrait> Traits { get; }
        int QualityScore { get; }
    }

    public interface ICraftedItem
    {
        IItemDetail Data { get; }
        IEnumerable<ITrait> Traits { get; }
        CraftingOutcomeTier Tier { get; }
        CraftedItemType Type { get; }
    }

    public enum CraftedItemType
    {
        SHIP,
        WEAPON
    }

    public class ShipInstance : ICraftedItem
    {
        public IItemDetail Data { get; set; }
        public CraftingOutcomeTier Tier { get; set; }
        public IEnumerable<ITrait> Traits { get; set; }

        public CraftedItemType Type => CraftedItemType.SHIP;
    }

    public enum CraftingOutcomeTier
    {
        STABLE,
        BRITTEL
    }

}