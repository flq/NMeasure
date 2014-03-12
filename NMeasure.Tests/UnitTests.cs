using System.Collections.Generic;
using System.Diagnostics;
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
            u.IsDimensionless().IsTrue();
            u = u*U.Meter;
            u.IsDimensionless().IsFalse();
        }

        [Test]
        public void MultiplicationAndDivisionCancelOut()
        {
            var u = U.Meter;
            u.IsDimensionless().IsFalse();
            u = u / U.Meter;
            u.IsDimensionless().IsTrue();
        }

        [Test]
        public void UnitSupportsGettingTheInverse()
        {
            var u = U.Meter.Inverse();
            var u2 = u * U.Meter;
            u2.IsDimensionless().IsTrue();
        }

        [Test]
        public void UnitscanBeDivided()
        {
            var u3 = U.Meter / U.Meter;
            u3.IsDimensionless().IsTrue();
        }

        [Test]
        public void AUnitConstructedOfFundamentalsIsFundamentalUnit()
        {
            var u = U._LENGTH / U._TIME;
            u.IsFundamental.IsTrue();
        }

        [Test]
        public void AUnitConstructedOfFundamentalsIsFundamentalUnit2()
        {
            var u = U._LENGTH.Squared();
            u.IsFundamental.IsTrue();
        }

        [Test]
        public void FundamentalUnitIsNotDimensionless()
        {
            U._LENGTH.IsDimensionless().IsFalse();
        }

        [Test]
        public void FundamentalUnitCombinationIsNotDimensionless()
        {
            U._LENGTH.Squared().IsDimensionless().IsFalse();
        }

        [Test]
        public void AConcreteUnitIsNotFundamental()
        {
            var u = U.Meter;
            u.IsFundamental.IsFalse();
        }

        [Test]
        public void CorrectHandlingOfCalculatingWithCombinedUnits()
        {
            var u1 = U.Meter / U.Second;
            var u2 = U.Kilogram.Squared();
            var u3 = u1 * u2;

            u3.IsEqualTo(U.Meter * U.Kilogram.Squared() / U.Second);
        }

        [Test]
        public void CorrectShorteningOfUnits()
        {
            var u1 = U.Meter / U.Second;
            var u2 = U.Kilogram / U.Second;

            var u3 = u1 / u2;
            u3.IsEqualTo(U.Meter / U.Kilogram);
        }

        [Test]
        public void OutputOfUnits()
        {
            var u = U.Kilometer.Squared() / U.Second;
            Debug.WriteLine(u.ToString());
        }


        public IEnumerable<object[]> UnitEqualityTestsSource()
        {
            yield return new object[] { U.GetRootUnit("m"), U.Meter };
            yield return new object[] { U.Meter * U.Dimensionless, U.Meter };
        }

        [Test, TestCaseSource("UnitEqualityTestsSource")]
        public void UnitEqualityTests(Unit unit, Unit compareTo)
        {
            unit.IsEqualTo(compareTo);
        }
    }
}