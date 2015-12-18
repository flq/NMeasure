using System.Collections.Generic;
using Xunit;

namespace NMeasure.Tests
{
    
    public class StandardConfigurationTests
    {
        public StandardConfigurationTests()
        {
            StandardUnitConfiguration.Use();
        }

        public static IEnumerable<object[]> BasicConversionChecksSource()
        {
            yield return new object[] { U.Centimeter, U.Meter, 1m, 0.01m };
            yield return new object[] { U.Mile, U.Inch, 1m, 63360m };
            yield return new object[] { U.Day, U.Second, 1m, 86400m };
            yield return new object[] { U.Kilogram, U.Ounce, 1m, 35.2740m };
            yield return new object[] { U.Fahrenheit, U.Celsius, 100m, 37.7778m };
            yield return new object[] { U.Fahrenheit, U.Kelvin, 100m, 310.9278m };
            yield return new object[] { U.Pascal, U.Psi, 1m, 6894m };
            yield return new object[] { U.Kilometer, U.Mile, 100m, 62.1371m };
        }

        public static IEnumerable<object[]> ComplexConversionChecksSource()
        {
            yield return new object[] { U.Kilometer / U.Hour, U.Mile / U.Hour, 100m, 62.1371m };
            yield return new object[] {U.Joule, U.Kilogram, 89875517873681764m, 1m};
            yield return new object[] {U.Meter.Squared(), U.Hectare, 10000m, 1m};
            yield return new object[] { U.Kilometer / U.Hour, U.Meter / U.Second, 100m, 27.7778m };
        }

        public static IEnumerable<object[]> MeasureMultiplicationTestsSource()
        {
            yield return new object[] { new Measure(1, U.Meter / U.Second.Squared()), new Measure(1, U.Kilogram), new Measure(1, U.Newton) };
        }

        [Theory, MemberData("BasicConversionChecksSource")]
        public void BasicConversionChecks(Unit from, Unit to, decimal input, decimal expectedOutput)
        {
            var m = (Measure) input*from;
            var m2 = m.ConvertTo(to);
            m2.Value.IsEqualTo(expectedOutput);
        }

        [Theory, MemberData("ComplexConversionChecksSource")]
        public void ComplexConversionChecks(Unit from, Unit to, decimal input, decimal expectedOutput)
        {
            var m = (Measure)input * from;
            var m2 = m.ConvertTo(to);
            m2.Value.IsEqualTo(expectedOutput);
        }

        [Theory, MemberData("MeasureMultiplicationTestsSource")]
        public void MeasureMultiplicationTests(Measure m1, Measure m2, Measure expected)
        {
            var result = m1*m2;
            result.IsEqualTo(expected);
        }

        [Fact]
        public void Unit_of_measure_xxx()
        {
            var m = 3 * U.Meter;
            var m2 = m - (10 * U.Inch);
            m2.Unit.IsEqualTo(U.Meter);
            m2.Value.IsEqualTo(2.746m);
        }
    }
}