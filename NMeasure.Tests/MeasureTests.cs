using System;
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
            var m2 = m * U.Meter;
            m2.Unit.IsEqualTo(Unit.From(U.Meter));
        }

        [Test]
        public void MeasuresCanBeMultiplied()
        {
            var m1 = new Measure(2.0, U.Meter);
            var m2 = new Measure(6.0, Unit.Inverse(U.Second));

            var m3 = m1*m2;

            m3.Value.IsEqualTo(12.0);
            m3.Unit.IsEqualTo(Unit.From(new[] { U.Meter }, new[] { U.Second }));

        }

        [Test]
        public void MeasuresCanBeDivided()
        {
            var m1 = new Measure(6.0, U.Meter);
            var m2 = new Measure(2.0, U.Second);

            var m3 = m1 / m2;

            m3.Value.IsEqualTo(3.0);
            m3.Unit.IsEqualTo(U.Meter.Per(U.Second));
        }

        [Test]
        public void CannotAddApplesToOranges()
        {
            var m1 = new Measure(1, U.Foot);
            var m2 = new Measure(1, U.Gram);
            Assert.Throws<InvalidOperationException>(() => { var m3 = m1 + m2; });
        }

        [Test]
        public void AdditionOfMeasuresIsSupported()
        {
            var m1 = new Measure(1, U.Foot);
            var m2 = new Measure(1, U.Foot);
            var m3 = m1 + m2;
            m3.Unit.IsEqualTo(Unit.From(U.Foot));
            m3.Value.IsEqualTo(2);
        }

    }
}
