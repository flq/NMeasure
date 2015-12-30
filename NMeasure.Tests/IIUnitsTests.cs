using System.Collections.Generic;
using Xunit;
using static NMeasure.U;
using static NMeasure.Tests.IiUnits;

namespace NMeasure.Tests
{
    public class IIUnitsTests
    {
        public IIUnitsTests()
        {
            StandardUnitConfiguration.Use().Extend<IiUnitsDefinition>();           
        }

        [Theory, MemberData("TestSource")]
        public void SomeConversions(Unit first, Unit second, decimal input, decimal expected)
        {
            var m1 = input * first;
            m1.ConvertTo(second).Value.IsEqualTo(expected);
        }

        [Fact]
        public void SoWhatAboutTheSpeedOfLight()
        {
            var speedOfLight = PhysicalConstants.SpeedOfLight.ConvertTo(ShipLength / Heartbeat);
            speedOfLight.Value.IsEqualTo(100000m);
        }

        public static IEnumerable<object[]> TestSource()
        {
            yield return new object[] { ShipLength, Kilometer, 1m, 2.4640m };
            yield return new object[] { QMichron, Hour, 50m, 28.5388m }; 
        }
    }

    internal static class IiUnits
    {
        private static IUnitFactory _units;
        private static IUnitFactory UnitsFactory => _units ?? (_units = GetUnitFactory());

        public static readonly Unit ShipLength = UnitsFactory.CreateRootUnit("SL");
        public static readonly Unit MicroShipLength = UnitsFactory.CreateRootUnit("mSL");
        public static readonly Unit Heartbeat = UnitsFactory.CreateRootUnit("IH");
        public static readonly Unit Michron = UnitsFactory.CreateRootUnit("IMc");
        public static readonly Unit QMichron = UnitsFactory.CreateRootUnit("IQMc");
        public static readonly Unit GreatMichron = UnitsFactory.CreateRootUnit("IGMc");
        public static readonly Unit Period = UnitsFactory.CreateRootUnit("IP");
        public static readonly Unit ShipMass = UnitsFactory.CreateRootUnit("IM");
        public static readonly Unit SunMass = UnitsFactory.CreateRootUnit("ISM");

        public class IiUnitsDefinition : IUnitConfigurationExtension
        {
            public void Extend(UnitConfiguration c)
            {
                c.Unit(MicroShipLength)
                    .IsPhysicalUnit(_LENGTH)
                    .StartScale()
                    .To(ShipLength, 1000);
                c.Unit(Heartbeat)
                    .IsPhysicalUnit(_TIME)
                    .StartScale()
                    .To(Michron, 50)
                    .To(QMichron, 50)
                    .To(GreatMichron, 50)
                    .To(Period, 250);
                c.Unit(ShipMass)
                    .IsPhysicalUnit(_MASS)
                    .ConvertibleTo(SunMass, m => m / 10E+25m, m => m * 10E+25m);

                c[ShipLength].ConvertValueBased(Kilometer, v => v * 2.4640476m, v => v / 2.4640476m);
                c[Heartbeat].ConvertValueBased(Second, v => v * 0.821917808219178m, v => v / 0.821917808219178m);
                c[ShipMass].ConvertValueBased(Kilogram, v => v * 55402.15m, v => v / 55402.15m);
            }
        }
    }
}
