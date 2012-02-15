using NUnit.Framework;

namespace NMeasure.Tests
{
    [TestFixture]
    public class UnitTests
    {
        [Test]
        public void SingleUnitsCanBeMultiplied()
        {
            var u = U.Dimensionless;
            u.IsDimensionless.IsTrue();
            u = u*U.Meter;
            u.IsDimensionless.IsFalse();
        }

        [Test]
        public void MultiplicationAndDivisionCancelOut()
        {
            var u = U.Meter;
            u.IsDimensionless.IsFalse();
            u = u / U.Meter;
            u.IsDimensionless.IsTrue();
        }

        [Test]
        public void UnitSupportsGettingTheInverse()
        {
            var u = U.Meter.Inverse();
            var u2 = u * U.Meter;
            u2.IsDimensionless.IsTrue();
        }

        [Test]
        public void UnitscanBeDivided()
        {
            var u3 = U.Meter / U.Meter;
            u3.IsDimensionless.IsTrue();
        }

        [Test]
        public void AUnitConstructedOfFundamentalsIsFundamentalUnit()
        {
            var u = U._LENGTH.Per(U._TIME);
            u.IsFundamental.IsTrue();
        }

        //[Test]
        //public void PhysicalUnitsCanBeObtainedFromComplexUnit()
        //{
        //    AdHocConfig.Use(c =>
        //                        {
        //                            c.Unit(U.Joule).IsPhysicalUnit((U._MASS * U._LENGTH.Squared()) / U._TIME.Squared());
        //                            c.Unit(U.Meter).IsPhysicalUnit(U._LENGTH);
        //                            c.Unit(U.Second).IsPhysicalUnit(U._TIME);
        //                        });

        //    U.Joule.Unit().ToPhysicalUnit().IsEqualTo((U._MASS * U._LENGTH.Squared()) / U._TIME.Squared());

        //    var u = U.Meter.Per(U.Second);
        //    u.ToPhysicalUnit().IsEqualTo(U._LENGTH.Per(U._TIME));

        //}
    }
}