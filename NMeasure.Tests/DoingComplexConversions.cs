using System;
using System.Linq.Expressions;
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
                                    c.Unit(SingleUnit.Meter)
                                        .IsPhysicalUnit(SingleUnit._LENGTH)
                                        .ConvertibleTo(SingleUnit.Kilometer, v => v*1000, v => v/1000);
                                    c.Unit(SingleUnit.Second)
                                        .IsPhysicalUnit(SingleUnit._TIME)
                                        .ConvertibleTo(SingleUnit.Hour, v => v/3600, v => v*3600);
                                });

            var kmPerH = SingleUnit.Kilometer.Per(SingleUnit.Hour);
            var mPerSec = SingleUnit.Meter.Per(SingleUnit.Second);
            
            var v1 = (Measure)100 * kmPerH;
            var v2 = v1.ConvertTo(mPerSec);

            v2.Unit.IsEqualTo(mPerSec);
            v2.Value.IsEqualTo(27);
        }
    }
}