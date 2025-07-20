using NUnit.Framework;
using System.Crafting;
using System.Item;
using System.Collections.Generic;
using NSubstitute;
using System.Linq;

namespace Crafting
{

    public class CraftingServiceTest
    {
        ICraftedItem failedItem;
        List<ICraftingRecipe> recipes;

        [SetUp]
        public void Setup()
        {
            failedItem = Substitute.For<ICraftedItem>();
            recipes = BuildRecipes();
        }

        [Test]
        public void ShouldReturnFailedItem()
        {
            CraftingService test = new(new List<ICraftingRecipe>(), failedItem);
            ICraftedItem actual = test.Evaluate(null);

            Assert.That(actual, Is.EqualTo(failedItem));
        }

        [TestCaseSource(nameof(RecipeTestData))]
        public void ShouldReturnCorrectItem(List<ICraftingRecipe> additionalRecipes, ICraftedItem failedItem, Dictionary<ICraftingResource, int> materials, string expected)
        {
            List<ICraftingRecipe> fullList = recipes.Concat(additionalRecipes).ToList();
            CraftingService test = new(fullList, failedItem);

            ICraftedItem actual = test.Evaluate(materials);

            Assert.That(actual.Tier, Is.EqualTo(CraftingOutcomeTier.STABLE));
            Assert.That(actual.Data.Id, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(5, CraftingOutcomeTier.STABLE)]
        [TestCase(0, CraftingOutcomeTier.BRITTEL)]
        public void ShouldReturnCorrectTier(int quality, CraftingOutcomeTier expectedTier)
        {
            CraftingService service = new(recipes, failedItem);
            ICraftingResource material = CreateMaterial("A", quality);

            Dictionary<ICraftingResource, int> materials = new()
        {
            {material, 2}
        };


            ICraftedItem actual = service.Evaluate(materials);

            Assert.That(actual.Data.Id, Is.Not.EqualTo("F"));
            Assert.That(actual.Tier, Is.EqualTo(expectedTier));
        }

        public static IEnumerable<TestCaseData> RecipeTestData()
        {
            IItemDetail failedDetail = Substitute.For<IItemDetail>();
            failedDetail.Id.Returns("F");

            ICraftedItem failedItem = Substitute.For<ICraftedItem>();
            failedItem.Data.Returns(failedDetail);

            ICharacteristic characteristicA = CreateCharacteristic("A");
            ICharacteristic characteristicB = CreateCharacteristic("B");

            ICraftingResource materialA = Substitute.For<ICraftingResource>();
            materialA.Characteristic.Returns(characteristicA);
            materialA.QualityScore.Returns(5);

            ICraftingResource materialB = Substitute.For<ICraftingResource>();
            materialB.Characteristic.Returns(characteristicB);
            materialB.QualityScore.Returns(5);


            yield return new TestCaseData(new List<ICraftingRecipe>(), failedItem, new Dictionary<ICraftingResource, int>()
        {
            {materialA, 2},
        }, "A");
            yield return new TestCaseData(new List<ICraftingRecipe>(), failedItem, new Dictionary<ICraftingResource, int>()
        {
            {materialA, 2},
            {materialB, 2},
        }, "B");
            yield return new TestCaseData(new List<ICraftingRecipe>(), failedItem, new Dictionary<ICraftingResource, int>()
        {
            {materialA, 2},
            {materialB, 5},
        }, "F");
        }

        static List<ICraftingRecipe> BuildRecipes()
        {
            IItemDetail detailA = Substitute.For<IItemDetail>();
            detailA.Id.Returns("A");

            IItemDetail detailB = Substitute.For<IItemDetail>();
            detailB.Id.Returns("B");

            ICharacteristic characteristicA = CreateCharacteristic("A");
            ICharacteristic characteristicB = CreateCharacteristic("B");

            ICraftingRecipe recipe1 = Substitute.For<ICraftingRecipe>();
            recipe1.MinimumQuality.Returns(5);
            recipe1.OutputData.Returns(detailA);
            recipe1.OutputType.Returns(CraftedItemType.SHIP);
            recipe1.RequiredCharacteristics.Returns(new Dictionary<ICharacteristic, int>()
        {
                    {characteristicA, 2}
        });
            ICraftingRecipe recipe2 = Substitute.For<ICraftingRecipe>();
            recipe2.MinimumQuality.Returns(5);
            recipe2.OutputData.Returns(detailB);
            recipe2.OutputType.Returns(CraftedItemType.SHIP);
            recipe2.RequiredCharacteristics.Returns(new Dictionary<ICharacteristic, int>()
        {
                    {characteristicA, 2},
                    {characteristicB, 2},
        });

            return new List<ICraftingRecipe>()
        {
            recipe1, recipe2
        };
        }


        public static ICraftingResource CreateMaterial(string characteristic = default, int quality = 5)
        {
            ICharacteristic characteristicMock = CreateCharacteristic(characteristic);
            ITrait traitMock = Substitute.For<ITrait>();
            ICraftingResource material = Substitute.For<ICraftingResource>();
            material.Characteristic.Returns(characteristicMock);
            material.QualityScore.Returns(quality);
            material.Traits.Returns(new List<ITrait>() { traitMock });
            return material;
        }

        public static ICharacteristic CreateCharacteristic(string characteristic = default)
        {
            ICharacteristic characteristicMock = Substitute.For<ICharacteristic>();
            characteristicMock.Id.Returns(characteristic);
            return characteristicMock;
        }
    }
}