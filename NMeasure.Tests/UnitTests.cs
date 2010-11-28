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
            u = u*SingleUnit.Meter;
            u.IsDimensionless.IsFalse();
        }

        [Test]
        public void MultiplicationAndDivisionCancelOut()
        {
            var u = new Unit();
            u.IsDimensionless.IsTrue();
            u = u * SingleUnit.Meter;
            u.IsDimensionless.IsFalse();
            u = u / SingleUnit.Meter;
            u.IsDimensionless.IsTrue();
        }

        [Test]
        public void UnitSupportsGettingTheInverse()
        {
            var u = Unit.Inverse(SingleUnit.Meter);
            var u2 = u*SingleUnit.Meter;
            u2.IsDimensionless.IsTrue();
        }

        [Test]
        public void UnitscanBeMultiplied()
        {
            var u = Unit.Inverse(SingleUnit.Meter);
            var u2 = Unit.From(SingleUnit.Meter);
            var u3 = u*u2;
            u3.IsDimensionless.IsTrue();
        }

        [Test]
        public void UnitscanBeDivided()
        {
            var u = Unit.From(SingleUnit.Meter);
            var u2 = Unit.From(SingleUnit.Meter);
            var u3 = u / u2;
            u3.IsDimensionless.IsTrue();
        }
    }
}