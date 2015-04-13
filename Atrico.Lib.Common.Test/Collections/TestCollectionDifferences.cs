using System.Collections.Generic;
using System.Diagnostics;
using Atrico.Lib.Assertions;
using Atrico.Lib.Common.Collections;
using Atrico.Lib.Testing.NUnitAttributes;

namespace Atrico.Lib.Common.Test.Collections
{
    [TestFixture]
    public class TestCollectionComparer
    {
        [Test]
        public void TestEqual()
        {
            // Arrange
            var actual = new List<int> {1, 2, 3, 4};
            var expected = new[] {1, 2, 3, 4};

            // Act
            var differences = CollectionComparer.Differences(actual, expected);

            // Assert
            Assert.That(differences, Is.TypeOf<CollectionDifferencesEqual>(), "Return type");
            Debug.WriteLine(differences.ToString());
        }

        [Test]
        public void TestMissingItem()
        {
            // Arrange
            var actual = new List<int> {1, 2, 4, 5};
            var expected = new[] {1, 2, 3, 4, 5};

            // Act
            var differences = CollectionComparer.Differences(actual, expected);

            // Assert
            Assert.That(differences, Is.TypeOf<CollectionDifferencesWrongItems>(), "Return type");
            var differencesT = differences as CollectionDifferencesWrongItems;
// ReSharper disable once PossibleNullReferenceException (Test will throw appropriately if so)
            Assert.That(differencesT.CountDelta, Is.EqualTo(-1), "Count delta");
            Assert.That(differencesT.Missing, AsCollection.Is.EquivalentTo(new[] {3}), "Missing");
            Assert.That(differencesT.Extra, AsCollection.Is.EquivalentTo(new int[] {}), "Extra");
            Debug.WriteLine(differences.ToString());
        }

        [Test]
        public void TestExtraItem()
        {
            // Arrange
            var actual = new List<int> {1, 2, 3, 4, 5};
            var expected = new[] {1, 2, 4, 5};

            // Act
            var differences = CollectionComparer.Differences(actual, expected);

            // Assert
            Assert.That(differences, Is.TypeOf<CollectionDifferencesWrongItems>(), "Return type");
            var differencesT = differences as CollectionDifferencesWrongItems;
// ReSharper disable once PossibleNullReferenceException (Test will throw appropriately if so)
            Assert.That(differencesT.CountDelta, Is.EqualTo(+1), "Count delta");
            Assert.That(differencesT.Extra, AsCollection.Is.EquivalentTo(new[] {3}), "Extra");
            Assert.That(differencesT.Missing, AsCollection.Is.EquivalentTo(new int[] {}), "Missing");
            Debug.WriteLine(differences.ToString());
        }

        [Test]
        public void TestMissingAndExtraItem()
        {
            // Arrange
            var actual = new List<int> {1, 2, 4, 5, 6};
            var expected = new[] {1, 2, 3, 4, 5};

            // Act
            var differences = CollectionComparer.Differences(actual, expected);

            // Assert
            Assert.That(differences, Is.TypeOf<CollectionDifferencesWrongItems>(), "Return type");
            var differencesT = differences as CollectionDifferencesWrongItems;
// ReSharper disable once PossibleNullReferenceException (Test will throw appropriately if so)
            Assert.That(differencesT.CountDelta, Is.EqualTo(0), "Count delta");
            Assert.That(differencesT.Missing, AsCollection.Is.EquivalentTo(new[] {3}), "Missing");
            Assert.That(differencesT.Extra, AsCollection.Is.EquivalentTo(new[] {6}), "Extra");
            Debug.WriteLine(differences.ToString());
        }

        [Test]
        public void TestExtraDuplicateItem()
        {
            // Arrange
            var actual = new List<int> {1, 2, 3, 3};
            var expected = new[] {1, 2, 3};

            // Act
            var differences = CollectionComparer.Differences(actual, expected);

            // Assert
            Assert.That(differences, Is.TypeOf<CollectionDifferencesWrongItems>(), "Return type");
            var differencesT = differences as CollectionDifferencesWrongItems;
// ReSharper disable once PossibleNullReferenceException (Test will throw appropriately if so)
            Assert.That(differencesT.CountDelta, Is.EqualTo(+1), "Count delta");
            Assert.That(differencesT.Extra, AsCollection.Is.EquivalentTo(new[] {3}), "Extra");
            Assert.That(differencesT.Missing, AsCollection.Is.EquivalentTo(new int[] {}), "Missing");
            Debug.WriteLine(differences.ToString());
        }

        [Test]
        public void TestOneOutOfOrder()
        {
            // Arrange
            var actual = new List<int> {1, 2, 5, 3, 4};
            var expected = new[] {1, 2, 3, 4, 5};

            // Act
            var differences = CollectionComparer.Differences(actual, expected);

            // Assert
            Assert.That(differences, Is.TypeOf<CollectionDifferencesWrongOrder>(), "Return type");
            var differencesT = differences as CollectionDifferencesWrongOrder;
// ReSharper disable once PossibleNullReferenceException (Test will throw appropriately if so)
            Assert.That(differencesT.OutOfOrder, AsCollection.Is.EquivalentTo(new[] {5}), "Out of order");
            Debug.WriteLine(differences.ToString());
        }

        [Test]
        public void TestTwoOutOfOrderAdjacent()
        {
            // Arrange
            var actual = new List<int> {1, 2, 4, 3, 5};
            var expected = new[] {1, 2, 3, 4, 5};

            // Act
            var differences = CollectionComparer.Differences(actual, expected);

            // Assert
            Assert.That(differences, Is.TypeOf<CollectionDifferencesWrongOrder>(), "Return type");
            var differencesT = differences as CollectionDifferencesWrongOrder;
// ReSharper disable once PossibleNullReferenceException (Test will throw appropriately if so)
            Assert.That(differencesT.OutOfOrder, AsCollection.Is.EquivalentTo(new[] {3, 4}), "Out of order");
            Debug.WriteLine(differences.ToString());
        }

        [Test]
        public void TestTwoOutOfOrderNonAdjacent()
        {
            // Arrange
            var actual = new List<int> {5, 2, 3, 4, 1};
            var expected = new[] {1, 2, 3, 4, 5};

            // Act
            var differences = CollectionComparer.Differences(actual, expected);

            // Assert
            Assert.That(differences, Is.TypeOf<CollectionDifferencesWrongOrder>(), "Return type");
            var differencesT = differences as CollectionDifferencesWrongOrder;
// ReSharper disable once PossibleNullReferenceException (Test will throw appropriately if so)
            Assert.That(differencesT.OutOfOrder, AsCollection.Is.EquivalentTo(new[] {1, 5}), "Out of order");
            Debug.WriteLine(differences.ToString());
        }
    }
}
