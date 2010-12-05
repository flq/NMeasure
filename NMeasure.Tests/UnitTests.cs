using NUnit.Framework;

namespace NMeasure.Tests
{
    [TestFixture]
    public class UnitTests
    {
        [Test]
        public void SingleUnitsCanBeMultiplied()
        {
            var u = new Unit();
            u.IsDimensionless.IsTrue();
            u = u*U.Meter;
            u.IsDimensionless.IsFalse();
        }

        [Test]
        public void MultiplicationAndDivisionCancelOut()
        {
            var u = new Unit();
            u.IsDimensionless.IsTrue();
            u = u * U.Meter;
            u.IsDimensionless.IsFalse();
            u = u / U.Meter;
            u.IsDimensionless.IsTrue();
        }

        [Test]
        public void UnitSupportsGettingTheInverse()
        {
            var u = Unit.Inverse(U.Meter);
            var u2 = u*U.Meter;
            u2.IsDimensionless.IsTrue();
        }

        [Test]
        public void UnitscanBeMultiplied()
        {
            var u = Unit.Inverse(U.Meter);
            var u2 = Unit.From(U.Meter);
            var u3 = u*u2;
            u3.IsDimensionless.IsTrue();
        }

        [Test]
        public void UnitscanBeDivided()
        {
            var u = Unit.From(U.Meter);
            var u2 = Unit.From(U.Meter);
            var u3 = u / u2;
            u3.IsDimensionless.IsTrue();
        }

        [Test]
        public void AUnitConstructedOfFundamentalsIsFundamentalUnit()
        {
            var u = U._LENGTH.Per(U._TIME);
            u.IsFundamental.IsTrue();
        }

        [Test]
        public void PhysicalUnitsCanBeObtainedFromComplexUnit()
        {
            AdHocConfig.Use(c =>
                                {
                                    c.Unit(U.Joule).IsPhysicalUnit((U._MASS * U._LENGTH.Squared()) / U._TIME.Squared());
                                    c.Unit(U.Meter).IsPhysicalUnit(U._LENGTH);
                                    c.Unit(U.Second).IsPhysicalUnit(U._TIME);
                                });

            U.Joule.Unit().ToPhysicalUnit().IsEqualTo((U._MASS * U._LENGTH.Squared()) / U._TIME.Squared());

            var u = U.Meter.Per(U.Second);
            u.ToPhysicalUnit().IsEqualTo(U._LENGTH.Per(U._TIME));

        }
    }
}