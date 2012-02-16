using System.Collections.Generic;
using NUnit.Framework;

namespace NMeasure.Tests
{
    [TestFixture]
    public class StandardConfigurationTests
    {
        [TestFixtureSetUp]
        public void Given()
        {
            StandardUnitConfiguration.Use();
        }

        public IEnumerable<object[]> BasicConversionCases()
        {
            yield return new object[] { U.Centimeter, U.Meter, 1, 0.01 };
            yield return new object[] { U.Mile, U.Inch, 1, 63360 };
            yield return new object[] { U.Day, U.Second, 1, 86400 };
            yield return new object[] { U.Kilogram, U.Ounce, 1, 35.2740 };
            yield return new object[] { U.Fahrenheit, U.Celsius, 100, 37.7778 };
            yield return new object[] { U.Fahrenheit, U.Kelvin, 100, 310.9278 };
            yield return new object[] { U.Pascal, U.Psi, 1, 6894 };
        }

        [Test]
        [TestCaseSource("BasicConversionCases")]
        public void BasicconversionChecks(Unit from, Unit to, double input, double expectedOutput)
        {
            var m = (Measure) input*from;
            var m2 = m.ConvertTo(to);
            m2.Value.IsEqualTo(expectedOutput);
        }

        #pragma warning disable 169
        // Used as value factory for the "ComplexConversionChecks" Test
        private static object[] complexCases =
        {
            new object[] {U.Kilometer / U.Hour, U.Mile / U.Hour, 100, 62.1371},
            //new object[] {U.Joule, U.Kilogram, 89875517873681764, 1},
            //new object[] {U.Meter.Squared(), U.Hectare, 10000, 1}
        };
        #pragma warning restore 169

        [Test, TestCaseSource("complexCases")]
        public void ComplexConversionChecks(Unit from, Unit to, double input, double expectedOutput)
        {
            var m = (Measure)input * from;
            var m2 = m.ConvertTo(to);
            m2.Value.IsEqualTo(expectedOutput);
        }

        #pragma warning disable 169
        // Used as value factory for the "MeasureMultiplicationTests" Test
        private static object[] measureMultiplicationTests = 
        {
            new object[] { new Measure(1, U.Meter /U.Second.Squared()), new Measure(1, U.Kilogram), new Measure(1, U.Newton) }
        };
        #pragma warning restore 169

        [Test, TestCaseSource("measureMultiplicationTests")]
        public void MeasureMultiplicationTests(Measure m1, Measure m2, Measure expected)
        {
            var result = m1*m2;
            result.IsEqualTo(expected);
        }
    }
}