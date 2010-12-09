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
            AdHocConfig.Use(c =>
                                {
                                    c.Unit(U.Foot).IsPhysicalUnit(U._LENGTH);
                                    c.Unit(U.Gram).IsPhysicalUnit(U._MASS);
                                });
            var m1 = new Measure(1, U.Foot);
            var m2 = new Measure(1, U.Gram);
            Assert.Throws<InvalidOperationException>(() => { var m3 = m1 + m2; });
        }

        [Test]
        public void CanAddGrannySmithToGoldenDelicious()
        {
            AdHocConfig.Use(c=>c.Unit(U.Gram).IsPhysicalUnit(U._MASS).StartScale().To(U.Kilogram, 1000));

            var m1 = new Measure(1, U.Kilogram);
            var m2 = new Measure(1, U.Gram);
            var m3 = m1 + m2;
            m3.Unit.IsEqualTo(U.Kilogram.Unit());
            m3.Value.IsEqualTo(1.001);
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

        [Test]
        public void AttemptToConvertWithoutinfoGivesInvalidOpException()
        {
            Assert.Throws<InvalidOperationException>(() => { var m1 = new Measure(1, U.Kilogram).ConvertTo(U.Gram); });
        }

        [Test]
        public void MeasuresSupportEquality()
        {
            var m1 = new Measure(1, U.Milligram);
            var m2 = new Measure(1, U.Milligram);
            m1.Equals(m2);
        }

    }
}
