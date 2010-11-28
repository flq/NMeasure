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
                                    c.Unit(U.Meter)
                                        .IsPhysicalUnit(U._LENGTH)
                                        .ConvertibleTo(U.Kilometer, v => v*1000, v => v/1000);
                                    c.Unit(U.Second)
                                        .IsPhysicalUnit(U._TIME)
                                        .ConvertibleTo(U.Hour, v => v/3600, v => v*3600);
                                });

            var kmPerH = U.Kilometer.Per(U.Hour);
            var mPerSec = U.Meter.Per(U.Second);
            
            var v1 = (Measure)100 * kmPerH;
            var v2 = v1.ConvertTo(mPerSec);

            v2.Unit.IsEqualTo(mPerSec);
            v2.Value.IsEqualTo(27);
        }
    }
}