﻿using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.Testing;
using Atrico.Lib.Testing.TestAttributes.NUnit;

namespace Atrico.Lib.Common.Tests.PropertyContainer
{
    [TestFixture]
    public class TestPropertyContainerEvents : TestFixtureBase
    {
        private class PropertyHandler
        {
            public int CallCount { get; private set; }
            public PropertyChangedEventArgs Args { get; private set; }
            public object Sender { get; private set; }
            public void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                Args = e;
                Sender = sender;
                ++CallCount;
            }
        }

        private static string GetCallingMethodName([CallerMemberName] string name = null)
        {
            return name;
        }

        [Test]
        public void TestSetDefault()
        {
            var name = GetCallingMethodName();
            var sender = this;

            // Arrange
            var props = new Common.PropertyContainer.PropertyContainer(sender);
            var handler = new PropertyHandler();
            props.PropertyChanged += handler.OnPropertyChanged;

            // Act
            props.Set(RandomValues.Integer());
            props.PropertyChanged -= handler.OnPropertyChanged;

            // Assert
            Assert.That(Value.Of(handler.CallCount).Is().EqualTo(1), "Single callback");
            Assert.That(Value.Of(handler.Args.PropertyName).Is().EqualTo(name), "Correct name");
            Assert.That(Value.Of(handler.Sender).Is().ReferenceEqualTo(sender), "Correct sender");
        }

        [Test]
        public void TestSetMultiple()
        {
            var name = GetCallingMethodName();
            const int count = 10;
            var values = RandomValues.UniqueValues<int>(count);
            object sender = null;

            // Arrange
            var props = new Common.PropertyContainer.PropertyContainer(sender);
            var handler = new PropertyHandler();
            props.PropertyChanged += handler.OnPropertyChanged;

            // Act
            foreach (var val in values)
            {
                props.Set(val);
            }
            props.PropertyChanged -= handler.OnPropertyChanged;

            // Assert
            Assert.That(Value.Of(handler.CallCount).Is().EqualTo(count), "Multiple callbacks");
            Assert.That(Value.Of(handler.Args.PropertyName).Is().EqualTo(name), "Correct name");
            Assert.That(Value.Of(handler.Sender).Is().ReferenceEqualTo(sender), "Correct sender");
        }

        [Test]
        public void TestSetNoChange()
        {
            var name = GetCallingMethodName();
            var values = Enumerable.Repeat(123, 10);
            var sender = new object();

            // Arrange
            var props = new Common.PropertyContainer.PropertyContainer(sender);
            var handler = new PropertyHandler();
            props.PropertyChanged += handler.OnPropertyChanged;

            // Act
            foreach (var val in values)
            {
                props.Set(val);
            }
            props.PropertyChanged -= handler.OnPropertyChanged;

            // Assert
            Assert.That(Value.Of(handler.CallCount).Is().EqualTo(1), "Single callback");
            Assert.That(Value.Of(handler.Args.PropertyName).Is().EqualTo(name), "Correct name");
            Assert.That(Value.Of(handler.Sender).Is().ReferenceEqualTo(sender), "Correct sender");
        }
    }
}