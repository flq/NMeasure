using System.Collections.Generic;
using System.Diagnostics;
using Xunit;

namespace NMeasure.Tests
{
    public class UnitTests
    {
        [Fact]
        public void SingleUnitsCanBeMultiplied()
        {
            var u = U.Dimensionless;
            u.IsDimensionless().IsTrue();
            u = u*U.Meter;
            u.IsDimensionless().IsFalse();
        }

        [Fact]
        public void MultiplicationAndDivisionCancelOut()
        {
            var u = U.Meter;
            u.IsDimensionless().IsFalse();
            u = u / U.Meter;
            u.IsDimensionless().IsTrue();
        }

        [Fact]
        public void UnitSupportsGettingTheInverse()
        {
            var u = U.Meter.Inverse();
            var u2 = u * U.Meter;
            u2.IsDimensionless().IsTrue();
        }

        [Fact]
        public void UnitscanBeDivided()
        {
            var u3 = U.Meter / U.Meter;
            u3.IsDimensionless().IsTrue();
        }

        [Fact]
        public void AUnitConstructedOfFundamentalsIsFundamentalUnit()
        {
            var u = U._LENGTH / U._TIME;
            u.IsFundamental.IsTrue();
        }

        [Fact]
        public void AUnitConstructedOfFundamentalsIsFundamentalUnit2()
        {
            var u = U._LENGTH.Squared();
            u.IsFundamental.IsTrue();
        }

        [Fact]
        public void FundamentalUnitIsNotDimensionless()
        {
            U._LENGTH.IsDimensionless().IsFalse();
        }

        [Fact]
        public void FundamentalUnitCombinationIsNotDimensionless()
        {
            U._LENGTH.Squared().IsDimensionless().IsFalse();
        }

        [Fact]
        public void AConcreteUnitIsNotFundamental()
        {
            var u = U.Meter;
            u.IsFundamental.IsFalse();
        }

        [Fact]
        public void CorrectHandlingOfCalculatingWithCombinedUnits()
        {
            var u1 = U.Meter / U.Second;
            var u2 = U.Kilogram.Squared();
            var u3 = u1 * u2;

            u3.IsEqualTo(U.Meter * U.Kilogram.Squared() / U.Second);
        }

        [Fact]
        public void CorrectShorteningOfUnits()
        {
            var u1 = U.Meter / U.Second;
            var u2 = U.Kilogram / U.Second;

            var u3 = u1 / u2;
            u3.IsEqualTo(U.Meter / U.Kilogram);
        }

        [Fact]
        public void OutputOfUnits()
        {
            var u = U.Kilometer.Squared() / U.Second;
            Debug.WriteLine(u.ToString());
        }


        public static IEnumerable<object[]> UnitEqualityTestsSource()
        {
            yield return new object[] { U.GetRootUnit("m"), U.Meter };
            yield return new object[] { U.Meter * U.Dimensionless, U.Meter };
        }

        [Theory, MemberData("UnitEqualityTestsSource")]
        public void UnitEqualityTests(Unit unit, Unit compareTo)
        {
            unit.IsEqualTo(compareTo);
        }
    }
}