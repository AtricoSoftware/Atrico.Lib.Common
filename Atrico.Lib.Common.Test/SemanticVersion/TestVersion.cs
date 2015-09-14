using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.Testing;
using Atrico.Lib.Testing.TestAttributes.NUnit;

namespace Atrico.Lib.Common.Tests.SemanticVersion
{
    [TestFixture]
    public class TestVersion : TestFixtureBase
    {
        #region Create

        [Test]
        public void TestCreateFromValuesSingle()
        {
            // Arrange

            // Act
            var version = Common.SemanticVersion.Version.From(1);

            // Assert
            Assert.That(Value.Of(version.GetValues()).Count().Is().EqualTo(1), "Length");
            Assert.That(Value.Of(version.GetValues()).Is().EqualTo(new[] { 1 }), "Values");
        }

        [Test]
        public void TestCreateFromValuesMultiple()
        {
            // Arrange

            // Act
            var version = Common.SemanticVersion.Version.From(1, 2, 3, 4, 5);

            // Assert
            Assert.That(Value.Of(version.GetValues()).Count().Is().EqualTo(5), "Length");
            Assert.That(Value.Of(version.GetValues()).Is().EqualTo(new[] { 1, 2, 3, 4, 5 }), "Values");
        }

        [Test]
        public void TestCreateFromStringSingle()
        {
            // Arrange

            // Act
            var version = Common.SemanticVersion.Version.From("1");

            // Assert
            Assert.That(Value.Of(version.GetValues()).Count().Is().EqualTo(1), "Length");
            Assert.That(Value.Of(version.GetValues()).Is().EqualTo(new[] { 1 }), "Values");
        }

        [Test]
        public void TestCreateFromStringMultiple()
        {
            // Arrange

            // Act
            var version = Common.SemanticVersion.Version.From("1.2.3.4.5");

            // Assert
            Assert.That(Value.Of(version.GetValues()).Count().Is().EqualTo(5), "Length");
            Assert.That(Value.Of(version.GetValues()).Is().EqualTo(new[] { 1, 2, 3, 4, 5 }), "Values");
        }

        #endregion

        #region Equals

        [Test]
        public void TestEqualsSingle()
        {
            // Arrange
            var lhs = Common.SemanticVersion.Version.From(1);
            var rhs = Common.SemanticVersion.Version.From(1);

            // Act
            var result1 = lhs.Equals(rhs);
            var result2 = rhs.Equals(lhs);

            // Assert
            Assert.That(Value.Of(result1).Is().True(), "lhs == rhs");
            Assert.That(Value.Of(result2).Is().True(), "rhs == 1hs");
        }

        [Test]
        public void TestEqualsMultiple()
        {
            // Arrange
            var lhs = Common.SemanticVersion.Version.From(1, 2, 3);
            var rhs = Common.SemanticVersion.Version.From(1, 2, 3);

            // Act
            var result1 = lhs.Equals(rhs);
            var result2 = rhs.Equals(lhs);

            // Assert
            Assert.That(Value.Of(result1).Is().True(), "lhs == rhs");
            Assert.That(Value.Of(result2).Is().True(), "rhs == 1hs");
        }

        [Test]
        public void TestEqualsTrailingZero()
        {
            // Arrange
            var lhs = Common.SemanticVersion.Version.From(1, 2, 3);
            var rhs = Common.SemanticVersion.Version.From(1, 2, 3, 0);

            // Act
            var result1 = lhs.Equals(rhs);
            var result2 = rhs.Equals(lhs);

            // Assert
            Assert.That(Value.Of(result1).Is().True(), "lhs == rhs");
            Assert.That(Value.Of(result2).Is().True(), "rhs == 1hs");
        }

        [Test]
        public void TestEqualsTrailingZeros()
        {
            // Arrange
            var lhs = Common.SemanticVersion.Version.From(1, 2, 3, 0);
            var rhs = Common.SemanticVersion.Version.From(1, 2, 3, 0, 0, 0);

            // Act
            var result1 = lhs.Equals(rhs);
            var result2 = rhs.Equals(lhs);

            // Assert
            Assert.That(Value.Of(result1).Is().True(), "lhs == rhs");
            Assert.That(Value.Of(result2).Is().True(), "rhs == 1hs");
        }

        [Test]
        public void TestNotEqualsSingle()
        {
            // Arrange
            var lhs = Common.SemanticVersion.Version.From(1);
            var rhs = Common.SemanticVersion.Version.From(2);

            // Act
            var result1 = lhs.Equals(rhs);
            var result2 = rhs.Equals(lhs);

            // Assert
            Assert.That(Value.Of(result1).Is().False(), "lhs == rhs");
            Assert.That(Value.Of(result2).Is().False(), "rhs == 1hs");
        }

        [Test]
        public void TestNotEqualsMultiple()
        {
            // Arrange
            var lhs = Common.SemanticVersion.Version.From(1, 2, 3);
            var rhs = Common.SemanticVersion.Version.From(1, 4, 3);

            // Act
            var result1 = lhs.Equals(rhs);
            var result2 = rhs.Equals(lhs);

            // Assert
            Assert.That(Value.Of(result1).Is().False(), "lhs == rhs");
            Assert.That(Value.Of(result2).Is().False(), "rhs == 1hs");
        }

        [Test]
        public void TestNotEqualsdifferentSize()
        {
            // Arrange
            var lhs = Common.SemanticVersion.Version.From(1, 2, 3);
            var rhs = Common.SemanticVersion.Version.From(1, 2);

            // Act
            var result1 = lhs.Equals(rhs);
            var result2 = rhs.Equals(lhs);

            // Assert
            Assert.That(Value.Of(result1).Is().False(), "lhs == rhs");
            Assert.That(Value.Of(result2).Is().False(), "rhs == 1hs");
        }

        #endregion

        #region Compare

        [Test]
        public void TestCompareEqualSingle()
        {
            // Arrange
            var lhs = Common.SemanticVersion.Version.From(1);
            var rhs = Common.SemanticVersion.Version.From(1);

            // Act
            var result1 = lhs.CompareTo(rhs);
            var result2 = rhs.CompareTo(lhs);

            // Assert
            Assert.That(Value.Of(result1).Is().EqualTo(0), "lhs comp rhs");
            Assert.That(Value.Of(result2).Is().EqualTo(0), "rhs comp 1hs");
        }

        [Test]
        public void TestCompareEqualsMultiple()
        {
            // Arrange
            var lhs = Common.SemanticVersion.Version.From(1, 2, 3);
            var rhs = Common.SemanticVersion.Version.From(1, 2, 3);

            // Act
            var result1 = lhs.CompareTo(rhs);
            var result2 = rhs.CompareTo(lhs);

            // Assert
            Assert.That(Value.Of(result1).Is().EqualTo(0), "lhs comp rhs");
            Assert.That(Value.Of(result2).Is().EqualTo(0), "rhs comp 1hs");
        }

        [Test]
        public void TestCompareNotEqualsSingle()
        {
            // Arrange
            var lhs = Common.SemanticVersion.Version.From(1);
            var rhs = Common.SemanticVersion.Version.From(2);

            // Act
            var result1 = lhs.CompareTo(rhs);
            var result2 = rhs.CompareTo(lhs);

            // Assert
            Assert.That(Value.Of(result1).Is().LessThan(0), "lhs comp rhs");
            Assert.That(Value.Of(result2).Is().GreaterThan(0), "rhs comp 1hs");
        }

        [Test]
        public void TestCompareNotEqualsMultiple()
        {
            // Arrange
            var lhs = Common.SemanticVersion.Version.From(1, 2, 3);
            var rhs = Common.SemanticVersion.Version.From(1, 2, 4);

            // Act
            var result1 = lhs.CompareTo(rhs);
            var result2 = rhs.CompareTo(lhs);

            // Assert
            Assert.That(Value.Of(result1).Is().LessThan(0), "lhs comp rhs");
            Assert.That(Value.Of(result2).Is().GreaterThan(0), "rhs comp 1hs");
        }

        [Test]
        public void TestCompareNotEqualsdifferentSize()
        {
            // Arrange
            var lhs = Common.SemanticVersion.Version.From(1, 2);
            var rhs = Common.SemanticVersion.Version.From(1, 2, 3);

            // Act
            var result1 = lhs.CompareTo(rhs);
            var result2 = rhs.CompareTo(lhs);

            // Assert
            Assert.That(Value.Of(result1).Is().LessThan(0), "lhs comp rhs");
            Assert.That(Value.Of(result2).Is().GreaterThan(0), "rhs comp 1hs");
        }

        #endregion

        #region ToString

        [Test]
        public void TestToStringSingle()
        {
            // Arrange

            // Act
            var version = Common.SemanticVersion.Version.From(1);

            // Assert
            Assert.That(Value.Of(version.ToString()).Is().EqualTo("1"), "ToString");
        }

        [Test]
        public void TestToStringMultiple()
        {
            // Arrange

            // Act
            var version = Common.SemanticVersion.Version.From(1, 2, 3, 4, 5);

            // Assert
            Assert.That(Value.Of(version.ToString()).Is().EqualTo("1.2.3.4.5"), "ToString");
        }

        [Test]
        public void TestToStringMin()
        {
            // Arrange

            // Act
            var version = Common.SemanticVersion.Version.From(1, 2, 3);

            // Assert
            Assert.That(Value.Of(version.ToString(5)).Is().EqualTo("1.2.3.0.0"), "ToString");
        }

        [Test]
        public void TestToStringMax()
        {
            // Arrange

            // Act
            var version = Common.SemanticVersion.Version.From(1, 2, 3);

            // Assert
            Assert.That(Value.Of(version.ToString(0, 2)).Is().EqualTo("1.2"), "ToString");
        }

        #endregion
    }
}