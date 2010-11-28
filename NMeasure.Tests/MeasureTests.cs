using NUnit.Framework;

namespace NMeasure.Tests
{
    [TestFixture]
    public class MeasureTests
    {
        [Test]
        public void AnyNumberIsDimensionlessMeasure()
        {
            var measure = (Measure) 1.2;
            measure.Value.IsEqualTo(1.2);
        }

        [Test]
        public void MultiplyWithUnitGivesUnitMeasure()
        {
            var m = new Measure(1.2);
            var m2 = m * SingleUnit.Meter;
            m2.Unit.IsEqualTo(Unit.From(SingleUnit.Meter));
        }

        [Test]
        public void MeasuresCanBeMultiplied()
        {
            var m1 = new Measure(2.0, SingleUnit.Meter);
            var m2 = new Measure(6.0, Unit.Inverse(SingleUnit.Second));

            var m3 = m1*m2;

            m3.Value.IsEqualTo(12.0);
            m3.Unit.IsEqualTo(Unit.From(new[] { SingleUnit.Meter }, new[] { SingleUnit.Second }));

        }

        [Test]
        public void MeasuresCanBeDivided()
        {
            var m1 = new Measure(6.0, SingleUnit.Meter);
            var m2 = new Measure(2.0, SingleUnit.Second);

            var m3 = m1 / m2;

            m3.Value.IsEqualTo(3.0);
            m3.Unit.IsEqualTo(Unit.From(new[] { SingleUnit.Meter }, new[] { SingleUnit.Second }));

        }

    }
}
