using System;
using Xunit;

namespace NMeasure.Tests
{
    public class SmallConfig : UnitConfiguration
    {
        public static void Use()
        {
            new SmallConfig();
        }

        public SmallConfig()
        {
            Unit(U.Meter)
                .BelongsToTypeSystem(NMeasure.UnitSystem.SI, NMeasure.UnitSystem.Metric)
                .IsPhysicalUnit(U._LENGTH);
        }
    }

    public class UnitDSLTest
    {

        [Fact]
        public void MetadataToUnitIsSpecifiableAndRetrievable()
        {
            SmallConfig.Use();
            var unitMeta = U.Meter.GetUnitData();
            unitMeta.IsMemberOfUnitSystem(UnitSystem.SI).IsTrue();
            unitMeta.PhysicalUnit.IsEqualTo(U._LENGTH);
        }

        [Fact]
        public void OnlyUnderscoreUnitsArePhysicalUnits()
        {
            Assert.Throws<InvalidOperationException>(() =>AdHocConfig.Use(c => c.Unit(U.Second).IsPhysicalUnit(U.Meter)));
        }

        [Fact]
        public void UnitsAreReducibleToTheirPhysicalUnits()
        {
            AdHocConfig.Use(c =>
                                {
                                    c.Unit(U.Meter).IsPhysicalUnit(U._LENGTH);
                                    c.Unit(U.Second).IsPhysicalUnit(U._TIME);
                                });

            var velocity = U.Meter/U.Second;
            var physicalVelocity = velocity.ToPhysicalUnit();

            physicalVelocity.IsEqualTo(U._LENGTH / U._TIME);
        }

        [Fact]
        public void PhysicalUnitMustBeAvailableToDefineConversion()
        {
            Assert.Throws<InvalidOperationException>(() =>
              AdHocConfig.Use(c => c.Unit(U.Centimeter).ConvertValueBased(U.Inch, v => v*0.393700787m, v => v*2.54m))
            );
        }

        [Fact]
        public void YetUnknownUnitUsedInConversionAssumesPhysicalUnit()
        {
            AdHocConfig.Use(c => c.Unit(U.Centimeter)
                                     .IsPhysicalUnit(U._LENGTH)
                                     .ConvertValueBased(U.Inch,v => v*0.393700787m, v => v*2.54m));
            var data = U.Inch.GetUnitData();
            data.PhysicalUnit.IsEqualTo(U._LENGTH);
        }

        [Fact]
        public void ConversionDefinitionMustMatchWithPhysicalUnit3()
        {
            Assert.Throws<InvalidOperationException>(() =>
              AdHocConfig.Use(c =>
                                  {
                                      c.Unit(U.Inch).IsPhysicalUnit(U._TIME);
                                      c.Unit(U.Centimeter).IsPhysicalUnit(U._LENGTH)
                                          .ConvertValueBased(U.Inch, v => v*0.393700787m, v => v*2.54m);
                                  })
            );
        }

        [Fact]
        public void AConversionCanBeSet()
        {
            AdHocConfig.Use(c =>
                                {
                                    c.Unit(U.Inch).IsPhysicalUnit(U._LENGTH);
                                    c.Unit(U.Centimeter).IsPhysicalUnit(U._LENGTH)
                                        .ConvertValueBased(U.Inch, v => v*0.393700787m, v => v*2.54m);
                                });
            var m = new Measure(1.0m, U.Inch);
            m.ConvertTo(U.Centimeter).Value.IsEqualTo(2.54m);
        }

        [Fact]
        public void UseOfScaleSystemCreatesConversions()
        {
            AdHocConfig.Use(c => c.Unit(U.Millimeter)
                .IsPhysicalUnit(U._LENGTH)
                .StartScale()
                .To(U.Centimeter, 10)
                .To(U.Meter, 100)
                .To(U.Kilometer, 1000));
            var m = new Measure(1.0m, U.Kilometer);
            var m2 = m.ConvertTo(U.Millimeter);
            m2.Value.IsEqualTo(1000000);
        }

        [Fact]
        public void DefaultRoundingIsApplied()
        {
            AdHocConfig.Use(c => c.SetMeasurePrecision(1));
            var m = new Measure(1.15m, U.Inch);
            m.Value.IsEqualTo(1.2m);
        }

        [Fact]
        public void DefaultConfigIsOverridablePerUnit()
        {
            AdHocConfig.Use(c =>
                                {
                                    c.SetMeasurePrecision(1);
                                    c.Unit(U.Nanosecond).HasPrecision(0);
                                });
            var m = new Measure(1.15m, U.Second);
            m.Value.IsEqualTo(1.2m);
            var m2 = new Measure(1.15m, U.Nanosecond);
            m2.Value.IsEqualTo(1.0m);
        }

        [Fact]
        public void ConfigIsExtensible()
        {
            var u = new UnitConfiguration();
            u.Extend<SomeDefinitions>();
            u[Unit.Parse("Foo")].PhysicalUnit.IsEqualTo(U._TIME);
        }

        private class SomeDefinitions : IUnitConfigurationExtension
        {
            public void Extend(UnitConfiguration c)
            {
                c.Unit(Unit.Parse("Foo")).IsPhysicalUnit(U._TIME);
            }
        }
    }
}