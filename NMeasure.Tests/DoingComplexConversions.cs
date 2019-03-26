using NUnit.Framework;
using static NMeasure.U;

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
                c.Unit(Meter).IsPhysicalUnit(_LENGTH)
                    .StartScale()
                    .To(Kilometer, 1000);
                c.Unit(Second).IsPhysicalUnit(_TIME)
                    .StartScale()
                    .To(Hour, 3600);
            });

            var kmPerH = Kilometer / Hour;
            var mPerSec = Meter / Second;
            
            var v1 = 100 * kmPerH;
            var v2 = v1.ConvertTo(mPerSec);

            v2.Unit.IsEqualTo(mPerSec);
            v2.Value.IsEqualTo(27.78m);
        }
    }
}