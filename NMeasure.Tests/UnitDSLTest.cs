using System;
using NUnit.Framework;

namespace NMeasure.Tests
{
    [TestFixture]
    public class UnitDSLTest
    {

        [Test]
        public void MetadataToUnitIsSpecifiableAndRetrievable()
        {
            SmallConfig.Use();
            var unitMeta = U.Meter.GetUnitData();
            unitMeta.IsMemberOfUnitSystem(UnitSystem.SI).IsTrue();
            unitMeta.PhysicalUnit.IsEqualTo(U._LENGTH.Unit());
        }

        [Test]
        public void OnlyUnderscoreUnitsArePhysicalUnits()
        {
            Assert.Throws<InvalidOperationException>(() =>AdHocConfig.Use(c => c.Unit(U.Second).IsPhysicalUnit(U.Meter)));
        }

        [Test]
        public void UnitsAreReducibleToTheirPhysicalUnits()
        {
            AdHocConfig.Use(c =>
                                {
                                    c.Unit(U.Meter).IsPhysicalUnit(U._LENGTH);
                                    c.Unit(U.Second).IsPhysicalUnit(U._TIME);
                                });

            var velocity = Unit.From(U.Meter)/U.Second;
            var physicalVelocity = velocity.ToPhysicalUnit();

            physicalVelocity.IsEqualTo(Unit.From(U._LENGTH) / U._TIME);
        }

        [Test]
        public void PhysicalUnitMustBeAvailableToDefineConversion()
        {
            Assert.Throws<InvalidOperationException>(() =>
              AdHocConfig.Use(c => c.Unit(U.Centimeter).ConvertibleTo(U.Inch, v => v*0.393700787, v => v*2.54))
            );
        }

        [Test]
        public void YetUnknownUnitUsedInConversionAssumesPhysicalUnit()
        {
            AdHocConfig.Use(c => c.Unit(U.Centimeter)
                                     .IsPhysicalUnit(U._LENGTH)
                                     .ConvertibleTo(U.Inch,v => v*0.393700787, v => v*2.54));
            var data = U.Inch.GetUnitData();
            data.PhysicalUnit.IsEqualTo(U._LENGTH.Unit());
        }

        [Test]
        public void ConversionDefinitionMustMatchWithPhysicalUnit3()
        {
            Assert.Throws<InvalidOperationException>(() =>
              AdHocConfig.Use(c =>
                                  {
                                      c.Unit(U.Inch).IsPhysicalUnit(U._TIME);
                                      c.Unit(U.Centimeter).IsPhysicalUnit(U._LENGTH)
                                          .ConvertibleTo(U.Inch, v => v*0.393700787, v => v*2.54);
                                  })
            );
        }

        [Test]
        public void AConversionCanBeSet()
        {
            AdHocConfig.Use(c =>
                                {
                                    c.Unit(U.Inch).IsPhysicalUnit(U._LENGTH);
                                    c.Unit(U.Centimeter).IsPhysicalUnit(U._LENGTH)
                                        .ConvertibleTo(U.Inch, v => v*0.393700787, v => v*2.54);
                                });
            var m = new Measure(1.0, U.Inch);
            m.ConvertTo(U.Centimeter).Value.IsEqualTo(2.54);
        }

        [Test]
        public void UseOfScaleSystemCreatesConversions()
        {
            AdHocConfig.Use(c => c.Unit(U.Millimeter)
                .IsPhysicalUnit(U._LENGTH)
                .StartScale()
                .To(U.Centimeter, 10)
                .To(U.Meter, 100)
                .To(U.Kilometer, 1000));
            var m = new Measure(1.0, U.Kilometer);
            var m2 = m.ConvertTo(U.Millimeter);
            m2.Value.IsEqualTo(1000000);
        }

        [Test]
        public void DefaultRoundingIsApplied()
        {
            AdHocConfig.Use(c => c.SetMeasurePrecision(1));
            var m = new Measure(1.15, U.Inch);
            m.Value.IsEqualTo(1.2);
        }
        
    }
}