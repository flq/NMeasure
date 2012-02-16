using NUnit.Framework;

namespace NMeasure.Tests
{
    [TestFixture]
    public class ComplexUnitGraphTests
    {
        private UnitGraph ug;

        [TestFixtureSetUp]
        public void Given()
        {
            AdHocConfig.Use(uc =>
            {
                uc.SetMeasurePrecision(4);
                uc.Unit(U.Meter).IsPhysicalUnit(U._LENGTH);
                uc.Unit(U.Second).IsPhysicalUnit(U._TIME);
                uc.Unit(U.Kilogram)
                    .IsPhysicalUnit(U._MASS)
                    .EquivalentTo(U.Joule*U.Second.Squared()/U.Meter.Squared());
                uc.Unit(U.Joule)
                    .IsPhysicalUnit((U._MASS*U._LENGTH.Squared())/U._TIME.Squared())
                    .EquivalentTo(U.Kilogram*U.Meter.Squared()/U.Second.Squared());

            });

            ug = new UnitGraph();
            var n1 = ug.AddUnit(U.Kilogram);
            var n2 = ug.AddUnit(U.Joule);
            ug.AddConversion(n1, n2, m => m * PhysicalConstants.EnergyMassFactor, m => m / PhysicalConstants.EnergyMassFactor);
        }

        [Test]
        public void AConversionFromEnergyToMass()
        {
            var m = new Measure(1, U.Kilogram);
            var m2 = ug.Convert(m, U.Joule);
            m2.Value.IsEqualTo(89875517873681764);
            m2.Unit.IsEqualTo(U.Joule);
        }

        [Test]
        public void AConversionFromMassToEnergy()
        {
            var m = new Measure(89875517873681764, U.Joule);
            var m2 = ug.Convert(m, U.Kilogram);
            m2.Unit.IsEqualTo(U.Kilogram);
            m2.Value.IsEqualTo(1);
        }
    }
}