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

        public IEnumerable<object[]> BasicConversionChecksSource()
        {
            yield return new object[] { U.Centimeter, U.Meter, 1, 0.01 };
            yield return new object[] { U.Mile, U.Inch, 1, 63360 };
            yield return new object[] { U.Day, U.Second, 1, 86400 };
            yield return new object[] { U.Kilogram, U.Ounce, 1, 35.2740 };
            yield return new object[] { U.Fahrenheit, U.Celsius, 100, 37.7778 };
            yield return new object[] { U.Fahrenheit, U.Kelvin, 100, 310.9278 };
            yield return new object[] { U.Pascal, U.Psi, 1, 6894 };
            yield return new object[] { U.Kilometer, U.Mile, 100, 62.1371 };
        }

        public IEnumerable<object[]> ComplexConversionChecksSource()
        {
            yield return new object[] { U.Kilometer / U.Hour, U.Mile / U.Hour, 100, 62.1371 };
            yield return new object[] {U.Joule, U.Kilogram, 89875517873681764, 1};
            yield return new object[] {U.Meter.Squared(), U.Hectare, 10000, 1};
            yield return new object[] { U.Kilometer / U.Hour, U.Meter / U.Second, 100, 27.7778 };
        }

        public IEnumerable<object[]> MeasureMultiplicationTestsSource()
        {
            yield return new object[] { new Measure(1, U.Meter / U.Second.Squared()), new Measure(1, U.Kilogram), new Measure(1, U.Newton) };
        }

        [Test, TestCaseSource("BasicConversionChecksSource")]
        public void BasicConversionChecks(Unit from, Unit to, double input, double expectedOutput)
        {
            var m = (Measure) input*from;
            var m2 = m.ConvertTo(to);
            m2.Value.IsEqualTo(expectedOutput);
        }

        [Test, TestCaseSource("ComplexConversionChecksSource")]
        public void ComplexConversionChecks(Unit from, Unit to, double input, double expectedOutput)
        {
            var m = (Measure)input * from;
            var m2 = m.ConvertTo(to);
            m2.Value.IsEqualTo(expectedOutput);
        }

        [Test, TestCaseSource("MeasureMultiplicationTestsSource")]
        public void MeasureMultiplicationTests(Measure m1, Measure m2, Measure expected)
        {
            var result = m1*m2;
            result.IsEqualTo(expected);
        }

        [Test]
        public void Unit_of_measure_xxx()
        {
            var m = 3 * U.Meter;
            var m2 = m - (10 * U.Inch);
            m2.Unit.IsEqualTo(U.Meter);
            m2.Value.IsEqualTo(2.746);

            

        }
    }
}