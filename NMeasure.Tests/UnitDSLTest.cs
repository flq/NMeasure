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
            var unitMeta = UnitInfo.GetUnitData(SingleUnit.Meter);
            unitMeta.IsMemberOfUnitSystem(UnitSystem.SI).IsTrue();
            unitMeta.PhysicalUnit.IsEqualTo(SingleUnit._LENGTH);
        }

        [Test]
        public void OnlyUnderscoreUnitsArePhysicalUnits()
        {
            Assert.Throws<InvalidOperationException>(() =>AdHocConfig.Use(c => c.Unit(SingleUnit.Second).IsPhysicalUnit(SingleUnit.Meter)));
        }

        [Test]
        public void UnitsAreReducibleToTheirPhysicalUnits()
        {
            AdHocConfig.Use(c =>
                                {
                                    c.Unit(SingleUnit.Meter).IsPhysicalUnit(SingleUnit._LENGTH);
                                    c.Unit(SingleUnit.Second).IsPhysicalUnit(SingleUnit._TIME);
                                });

            var velocity = Unit.From(SingleUnit.Meter)/SingleUnit.Second;
            var physicalVelocity = UnitInfo.ToPhysicalUnit(velocity);

            physicalVelocity.IsEqualTo(Unit.From(SingleUnit._LENGTH) / SingleUnit._TIME);
        }

        [Test]
        public void ConversionDefinitionMustMatchWithPhysicalUnit1()
        {
            Assert.Throws<InvalidOperationException>(() =>
              AdHocConfig.Use(c => c.Unit(SingleUnit.Centimeter).ConvertibleTo(SingleUnit.Inch, v => v * 0.393700787, v => v * 2.54))
            );
        }

        [Test]
        public void ConversionDefinitionMustMatchWithPhysicalUnit2()
        {
            Assert.Throws<InvalidOperationException>(() =>
              AdHocConfig.Use(c => c.Unit(SingleUnit.Centimeter).IsPhysicalUnit(SingleUnit._LENGTH).ConvertibleTo(SingleUnit.Inch, v => v * 0.393700787, v => v * 2.54))
            );
        }

        [Test]
        public void ConversionDefinitionMustMatchWithPhysicalUnit3()
        {
            Assert.Throws<InvalidOperationException>(() =>
              AdHocConfig.Use(c =>
                                  {
                                      c.Unit(SingleUnit.Inch).IsPhysicalUnit(SingleUnit._TIME);
                                      c.Unit(SingleUnit.Centimeter).IsPhysicalUnit(SingleUnit._LENGTH)
                                          .ConvertibleTo(SingleUnit.Inch, v => v*0.393700787, v => v*2.54);
                                  })
            );
        }

        [Test]
        public void AConversionCanBeSet()
        {
            AdHocConfig.Use(c =>
                                {
                                    c.Unit(SingleUnit.Inch).IsPhysicalUnit(SingleUnit._LENGTH);
                                    c.Unit(SingleUnit.Centimeter).IsPhysicalUnit(SingleUnit._LENGTH)
                                        .ConvertibleTo(SingleUnit.Inch, v => v*0.393700787, v => v*2.54);
                                });
            var m = new Measure(1.0, SingleUnit.Inch);
            m.ConvertTo(SingleUnit.Centimeter).Value.IsEqualTo(2.54);
        }

        [Test]
        public void UseOfScaleSystemCreatesConversions()
        {
            AdHocConfig.Use(c => c.Unit(SingleUnit.Millimeter)
                .IsPhysicalUnit(SingleUnit._LENGTH)
                .StartScale()
                .To(SingleUnit.Centimeter, 10)
                .To(SingleUnit.Meter, 100)
                .To(SingleUnit.Kilometer, 1000));
            var m = new Measure(1.0, SingleUnit.Kilometer);
            var m2 = m.ConvertTo(SingleUnit.Millimeter);
            m2.Value.IsEqualTo(1000000);
        }

        [Test]
        public void DefaultRoundingIsApplied()
        {
            AdHocConfig.Use(c => c.SetMeasurePrecision(1));
            var m = new Measure(1.15, SingleUnit.Inch);
            m.Value.IsEqualTo(1.2);
        }
        
    }
}