using NUnit.Framework;

namespace NMeasure.Tests
{
    [TestFixture]
    public class DoingComplexConversions
    {

        [Test]
        public void ConversionOfComplexUnits()
        {
            AdHocConfig.Use(c=>
                                {
                                    c.SetMeasurePrecision(2);
                                    c.Unit(U.Meter).IsPhysicalUnit(U._LENGTH)
                                        .StartScale()
                                        .To(U.Kilometer, 1000);
                                    c.Unit(U.Second).IsPhysicalUnit(U._TIME)
                                        .StartScale()
                                        .To(U.Hour, 3600);
                                });

            var kmPerH = U.Kilometer / U.Hour;
            var mPerSec = U.Meter / U.Second;
            
            var v1 = (Measure)100 * kmPerH;
            var v2 = v1.ConvertTo(mPerSec);

            v2.Unit.IsEqualTo(mPerSec);
            v2.Value.IsEqualTo(27.78m);
        }
    }
}