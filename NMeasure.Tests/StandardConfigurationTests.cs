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

        [TestCase(U.Centimeter, U.Meter, 1, 0.01)]
        [TestCase(U.Mile, U.Inch, 1, 63360)]
        [TestCase(U.Day, U.Second, 1, 86400)]
        [TestCase(U.Kilogram, U.Ounce, 1, 35.2740)]
        public void BasicconversionChecks(U from, U to, double input, double expectedOutput)
        {
            var m = (Measure) input*from.Unit();
            var m2 = m.ConvertTo(to);
            m2.Value.IsEqualTo(expectedOutput);
        }

        #pragma warning disable 169
        // Used as value factory for the "ComplexxConversionChecks" Test
        private static object[] complexCases = {

                                                   new object[] { U.Kilometer.Per(U.Hour), U.Mile.Per(U.Hour), 100, 62.1371 }
                                               };
        #pragma warning restore 169

        [Test, TestCaseSource("complexCases")]
        public void ComplexConversionChecks(Unit from, Unit to, double input, double expectedOutput)
        {
            var m = (Measure)input * from;
            var m2 = m.ConvertTo(to);
            m2.Value.IsEqualTo(expectedOutput);
        }
    }
}