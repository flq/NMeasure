using System;
using NMeasure.Converting;
using NUnit.Framework;

namespace NMeasure.Tests
{
    [TestFixture]
    public class UnitGraphTests
    {
        private UnitGraph ug;

        public UnitGraphTests()
        {
            AdHocConfig.Use(uc =>
                                {
                                    uc.SetMeasurePrecision(4);
                                    uc.Unit(U.Meter).IsPhysicalUnit(U._LENGTH);
                                    uc.Unit(U.Centimeter).IsPhysicalUnit(U._LENGTH);
                                    uc.Unit(U.Inch).IsPhysicalUnit(U._LENGTH);
                                    uc.Unit(U.Second).IsPhysicalUnit(U._TIME);
                                });

            ug = new UnitGraph();
            var n1 = ug.AddUnit(U.Inch);
            var n2 = ug.AddUnit(U.Centimeter);
            var n3 = ug.AddUnit(U.Meter);
            ug.AddConversion(n1, n2, v => v * 2.54m, v => v * 0.393700787m);
            ug.AddConversion(n2, n3, v => v * 0.01m, v => v * 100);
            
        }

        [Test]
        public void TheUnitGraphNodesAreAccessibleByIndexer()
        {
            ug[U.Inch].IsNotNull();
            ug[U.Centimeter].IsNotNull();
        }

        [Test]
        public void TwoUnitsAreLinkedByConversion()
        {
            ug[U.Inch].Conversions.HasCount(1);
            ug[U.Centimeter].Conversions.HasCount(2);
        }

        [Test]
        public void TheConversionProvidesWorkingMeasure()
        {
            var m = new Measure(1.0m, U.Inch);
            var m2 = ug[U.Inch].Conversions[0].ApplyConversion(m);
            
            m2.Value.IsEqualTo(2.54m);
            m2.Unit.Equals(U.Centimeter);
        }

        [Test]
        public void TheGraphSupportsConversionOverSeveralEdges()
        {
            var m = new Measure(1.0m, U.Inch);
            var m2 = ug.Convert(m, U.Meter);

            m2.Value.IsEqualTo(0.0254m);
            m2.Unit.Equals(U.Meter);
        }

        [Test]
        public void IncompatibleConversionsWillBeRejected()
        {
            var m = (Measure) 1.0m*U.Second;
            Assert.Throws<InvalidOperationException>(() => { var unused = ug.Convert(m, U.Inch); });
        }

        [Test]
        public void ConversionFromUnitToSameUnitGivesInvariantConversion()
        {
            var c = ug.GetConverter(U.Meter, U.Meter);
            c.IsOfType<InvariantConversion>();
        }
    }
}