using NUnit.Framework;

namespace NMeasure.Tests
{
    [TestFixture]
    public class UnitGraphTests
    {
        private UnitGraph ug;

        [TestFixtureSetUp]
        public void Given()
        {
            ug = new UnitGraph();
            var n1 = ug.AddUnit(Unit.From(U.Inch));
            var n2 = ug.AddUnit(Unit.From(U.Centimeter));
            var n3 = ug.AddUnit(Unit.From(U.Meter));
            ug.AddConversion(n1, n2, v => v * 2.54, v => v * 0.393700787);
            ug.AddConversion(n2, n3, v => v * 0.01, v => v * 100);
            UnitConfiguration.UnitSystem.SetMeasurePrecision(4);
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
            var m = new Measure(1.0, U.Inch);
            var m2 = ug[U.Inch].Conversions[0].ApplyConversion(m);
            
            m2.Value.IsEqualTo(2.54);
            m2.Unit.Equals(Unit.From(U.Centimeter));
        }

        [Test]
        public void TheGraphSupportsConversionOverSeveralEdges()
        {
            var m = new Measure(1.0, U.Inch);
            var m2 = ug.Convert(m, Unit.From(U.Meter));

            m2.Value.IsEqualTo(0.0254);
            m2.Unit.Equals(Unit.From(U.Meter));
        }
    }
}