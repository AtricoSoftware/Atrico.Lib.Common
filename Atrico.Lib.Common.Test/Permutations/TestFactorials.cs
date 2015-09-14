using System;
using System.Collections.Generic;
using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.Common.Permutations;
using Atrico.Lib.Testing.TestAttributes.NUnit;

namespace Atrico.Lib.Common.Tests.Permutations
{
    [TestFixture]
    public class TestFactorials
    {
        private static readonly ulong[] _factorialValues = new ulong[11];

        static TestFactorials()
        {
            _factorialValues[0] = 0;
            ulong result = 1;
            for (uint val = 1; val < _factorialValues.Length; ++val)
            {
                result *= val;
                _factorialValues[val] = result;
            }
        }

        public IEnumerable<Tuple<uint, ulong>> FactorialValues
        {
            get
            {
                for (uint val = 0; val < _factorialValues.Length; ++val)
                {
                    yield return new Tuple<uint, ulong>(val, _factorialValues[val]);
                }
            }
        }

        public IEnumerable<Tuple<uint, uint, ulong>> DivideValues
        {
            get
            {
                for (uint bot = 0; bot < _factorialValues.Length; ++bot)
                {
                    for (var top = bot; top < _factorialValues.Length; ++top)
                    {
                        if (bot > 0)
                        {
                            yield return new Tuple<uint, uint, ulong>(top, bot, _factorialValues[top] / _factorialValues[bot]);
                        }
                    }
                }
            }
        }

        [Test]
        public void TestCalculate([ValueSource("FactorialValues")] Tuple<uint, ulong> value)
        {
            // Act
            var result = Factorials.Calculate(value.Item1);

            // Assert
            Assert.That(Value.Of(result).Is().EqualTo(value.Item2));
        }

        [Test]
        public void TestDivide([ValueSource("DivideValues")] Tuple<uint, uint, ulong> value)
        {
            // Act
            var result = Factorials.Divide(value.Item1, value.Item2);

            // Assert
            Assert.That(Value.Of(result).Is().EqualTo(value.Item3));
        }
    }
}
