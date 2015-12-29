using System.Collections.Generic;
using Xunit;

using static NMeasure.U;

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
            yield return new object[] { Centimeter, Meter, 1m, 0.01m };
            yield return new object[] { Mile, Inch, 1m, 63360m };
            yield return new object[] { Day, Second, 1m, 86400m };
            yield return new object[] { Kilogram, Ounce, 1m, 35.2740m };
            yield return new object[] { Fahrenheit, Celsius, 100m, 37.7778m };
            yield return new object[] { Fahrenheit, Kelvin, 100m, 310.9278m };
            yield return new object[] { Pascal, Psi, 1m, 6894m };
            yield return new object[] { Kilometer, Mile, 100m, 62.1371m };
            yield return new object[] { GigaJoule, Joule, 1m, 1e+9m };
        }

        public static IEnumerable<object[]> ComplexConversionChecksSource()
        {
            yield return new object[] { Kilometer / Hour, Mile / Hour, 100m, 62.1371m };
            yield return new object[] {Joule, Kilogram, 89875517873681764m, 1m};
            yield return new object[] {Meter.Squared(), Hectare, 10000m, 1m};
            yield return new object[] { Kilometer / Hour, Meter / Second, 100m, 27.7778m };
        }

        public static IEnumerable<object[]> MeasureMultiplicationTestsSource()
        {
            yield return new object[] { new Measure(1, Meter / Second.Squared()), new Measure(1, Kilogram), new Measure(1, Newton) };
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
            var m = 3 * Meter;
            var m2 = m - (10 * Inch);
            m2.Unit.IsEqualTo(Meter);
            m2.Value.IsEqualTo(2.746m);
        }
    }
}