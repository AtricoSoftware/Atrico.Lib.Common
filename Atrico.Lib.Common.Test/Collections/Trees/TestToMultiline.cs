using System.Linq;
using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.Common.Collections.Tree;
using Atrico.Lib.Testing.NUnitAttributes;

namespace Atrico.Lib.Common.Test.Collections.Trees
{
    [TestFixture]
    public class TestToMultiline : TreeTestFixtureBase
    {

        [Test]
        public void TestSingleNode()
        {
            // Arrange
            var tree = Tree<string>.Create(false);
            tree.Add("root");

            // Act
            var lines = tree.ToMultilineString().ToArray();

            // Assert
            Display(tree);
            Assert.That(Value.Of(lines).Count().Is().EqualTo(1), "Number of lines");
            Assert.That(Value.Of(lines[0]).Is().EqualTo(ConvertAscii(@"~-root")), "Root node");
        }

        [Test]
        public void TestOneNodeSingleBranch()
        {
            // Arrange
            var tree = Tree<string>.Create(false);
            var root = tree.Add("root");
            root.Add("branch");

            // Act
            var lines = tree.ToMultilineString().ToArray();

            // Assert
            Display(tree);
            Assert.That(Value.Of(lines).Count().Is().EqualTo(2), "Number of lines");
            Assert.That(Value.Of(lines[0]).Is().EqualTo(ConvertAscii(@"~-root")), "Root");
            Assert.That(Value.Of(lines[1]).Is().EqualTo(ConvertAscii(@"  \-branch")), "Branch");
        }

        [Test]
        public void TestMultipleSingleBranches()
        {
            // Arrange
            var tree = Tree<string>.Create(false);
            var root = tree.Add("root");
            var one = root.Add("one");
            var two = one.Add("two");
            two.Add("three");

            // Act
            var lines = tree.ToMultilineString().ToArray();

            // Assert
            Display(tree);
            Assert.That(Value.Of(lines).Count().Is().EqualTo(4), "Number of lines");
            Assert.That(Value.Of(lines[0]).Is().EqualTo(ConvertAscii(@"~-root")), "Root");
            Assert.That(Value.Of(lines[1]).Is().EqualTo(ConvertAscii(@"  \-one")), "One");
            Assert.That(Value.Of(lines[2]).Is().EqualTo(ConvertAscii(@"    \-two")), "Two");
            Assert.That(Value.Of(lines[3]).Is().EqualTo(ConvertAscii(@"      \-three")), "Three");
        }

        [Test]
        public void TestOneNodeDoubleBranch()
        {
            // Arrange
            var tree = Tree<string>.Create(false);
            var root = tree.Add("root");
            root.Add("one");
            root.Add("two");

            // Act
            var lines = tree.ToMultilineString().ToArray();

            // Assert
            Display(tree);
            Assert.That(Value.Of(lines).Count().Is().EqualTo(3), "Number of lines");
            Assert.That(Value.Of(lines[0]).Is().EqualTo(ConvertAscii(@"  /-one")), "One");
            Assert.That(Value.Of(lines[1]).Is().EqualTo(ConvertAscii(@"~-root")), "Root");
            Assert.That(Value.Of(lines[2]).Is().EqualTo(ConvertAscii(@"  \-two")), "Two");
        }

        [Test]
        public void TestMultipleBranches()
        {
            // Arrange
            var tree = Tree<string>.Create(false);
            var root = tree.Add("root");
            var one = root.Add("one");
            one.Add("one1");
            one.Add("one2");
            var two = root.Add("two");
            two.Add("two1");
            two.Add("two2");

            // Act
            var lines = tree.ToMultilineString().ToArray();

            // Assert
            Display(tree);
            Assert.That(Value.Of(lines).Count().Is().EqualTo(7), "Number of lines");
            Assert.That(Value.Of(lines[0]).Is().EqualTo(ConvertAscii(@"    /-one1")), "One1");
            Assert.That(Value.Of(lines[1]).Is().EqualTo(ConvertAscii(@"  /-one")), "One");
            Assert.That(Value.Of(lines[2]).Is().EqualTo(ConvertAscii(@"  | \-one2")), "One2");
            Assert.That(Value.Of(lines[3]).Is().EqualTo(ConvertAscii(@"~-root")), "Root");
            Assert.That(Value.Of(lines[4]).Is().EqualTo(ConvertAscii(@"  | /-two1")), "Two1");
            Assert.That(Value.Of(lines[5]).Is().EqualTo(ConvertAscii(@"  \-two")), "Two");
            Assert.That(Value.Of(lines[6]).Is().EqualTo(ConvertAscii(@"    \-two2")), "Two2");
        }

        [Test]
        public void TestMultipleRoots()
        {
            // Arrange
            var tree = Tree<string>.Create(false);
            var root1 = tree.Add("root1");
            var one1 = root1.Add("one1");
            var two1 = one1.Add("two1");
            two1.Add("three1");
            var root2 = tree.Add("root2");
            var one2 = root2.Add("one2");
            var two2 = one2.Add("two2");
            two2.Add("three2");

            // Act
            var lines = tree.ToMultilineString().ToArray();

            // Assert
            Display(tree);
            Assert.That(Value.Of(lines).Count().Is().EqualTo(8), "Number of lines");
            Assert.That(Value.Of(lines[0]).Is().EqualTo(ConvertAscii(@">-root1")), "Root1");
            Assert.That(Value.Of(lines[1]).Is().EqualTo(ConvertAscii(@"| \-one1")), "One1");
            Assert.That(Value.Of(lines[2]).Is().EqualTo(ConvertAscii(@"|   \-two1")), "Two1");
            Assert.That(Value.Of(lines[3]).Is().EqualTo(ConvertAscii(@"|     \-three1")), "Three1");
            Assert.That(Value.Of(lines[4]).Is().EqualTo(ConvertAscii(@"\-root2")), "Root2");
            Assert.That(Value.Of(lines[5]).Is().EqualTo(ConvertAscii(@"  \-one2")), "One2");
            Assert.That(Value.Of(lines[6]).Is().EqualTo(ConvertAscii(@"    \-two2")), "Two2");
            Assert.That(Value.Of(lines[7]).Is().EqualTo(ConvertAscii(@"      \-three2")), "Three2");
        }

        [Test]
        public void TestAllOverThePlace()
        {
            // Arrange
            var tree = Tree<string>.Create(false);
            var root1 = tree.Add("root1");
            var root2 = tree.Add("root2");
            var one1 = root2.Add("one1");
            var one11 = one1.Add("one11");
            var one12 = one1.Add("one12");
            var one2 = root2.Add("one2");
            var one21 = one2.Add("one21");
            var one22 = one2.Add("one22");
            var one23 = one2.Add("one23");
            var root3 = tree.Add("root3");

            // Act
            var lines = tree.ToMultilineString().ToArray();

            // Assert
            Display(tree);
            Assert.That(Value.Of(lines).Count().Is().EqualTo(10), "Number of lines");
            Assert.That(Value.Of(lines[0]).Is().EqualTo(ConvertAscii(@"/-root1")), "Root1");
            Assert.That(Value.Of(lines[1]).Is().EqualTo(ConvertAscii(@"|   /-one11")), "One11");
            Assert.That(Value.Of(lines[2]).Is().EqualTo(ConvertAscii(@"| /-one1")), "One1");
            Assert.That(Value.Of(lines[3]).Is().EqualTo(ConvertAscii(@"| | \-one12")), "One12");
            Assert.That(Value.Of(lines[4]).Is().EqualTo(ConvertAscii(@"#-root2")), "Root2");
            Assert.That(Value.Of(lines[5]).Is().EqualTo(ConvertAscii(@"| | /-one21")), "One21");
            Assert.That(Value.Of(lines[6]).Is().EqualTo(ConvertAscii(@"| \-one2")), "One2");
            Assert.That(Value.Of(lines[7]).Is().EqualTo(ConvertAscii(@"|   +-one22")), "One22");
            Assert.That(Value.Of(lines[8]).Is().EqualTo(ConvertAscii(@"|   \-one23")), "One23");
            Assert.That(Value.Of(lines[9]).Is().EqualTo(ConvertAscii(@"\-root3")), "Root3");
        }

        [Test]
        public void TestAllOverThePlace2()
        {
            // Arrange
            var tree = Tree<string>.Create(false);
            var root = tree.Add("0");
            var one1 = root.Add("1");
            var one2 = root.Add("2");
            var one3 = root.Add("3");
            var one11 = one1.Add("11");
            var one12 = one1.Add("12");
            var one13 = one1.Add("13");
            var one21 = one2.Add("21");
            var one22 = one2.Add("22");
            var one23 = one2.Add("23");
            var one31 = one3.Add("31");
            var one32 = one3.Add("32");
            var one33 = one3.Add("33");

            // Act
            var lines = tree.ToMultilineString().ToArray();

            // Assert
            Display(tree);
            Assert.That(Value.Of(lines).Count().Is().EqualTo(13), "Number of lines");
            Assert.That(Value.Of(lines[00]).Is().EqualTo(ConvertAscii(@"    /-11")), "11");
            Assert.That(Value.Of(lines[01]).Is().EqualTo(ConvertAscii(@"  /-1")), "1");
            Assert.That(Value.Of(lines[02]).Is().EqualTo(ConvertAscii(@"  | +-12")), "12");
            Assert.That(Value.Of(lines[03]).Is().EqualTo(ConvertAscii(@"  | \-13")), "13");
            Assert.That(Value.Of(lines[04]).Is().EqualTo(ConvertAscii(@"~-0")), "0");
            Assert.That(Value.Of(lines[05]).Is().EqualTo(ConvertAscii(@"  | /-21")), "21");
            Assert.That(Value.Of(lines[06]).Is().EqualTo(ConvertAscii(@"  +-2")), "2");
            Assert.That(Value.Of(lines[07]).Is().EqualTo(ConvertAscii(@"  | +-22")), "22");
            Assert.That(Value.Of(lines[08]).Is().EqualTo(ConvertAscii(@"  | \-23")), "23");
            Assert.That(Value.Of(lines[09]).Is().EqualTo(ConvertAscii(@"  | /-31")), "31");
            Assert.That(Value.Of(lines[10]).Is().EqualTo(ConvertAscii(@"  \-3")), "3");
            Assert.That(Value.Of(lines[11]).Is().EqualTo(ConvertAscii(@"    +-32")), "32");
            Assert.That(Value.Of(lines[12]).Is().EqualTo(ConvertAscii(@"    \-33")), "33");
        }
    }
}