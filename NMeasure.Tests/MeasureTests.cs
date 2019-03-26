using System;
using System.Diagnostics;
using System.Globalization;
using NUnit.Framework;
using static NMeasure.U;

namespace NMeasure.Tests
{

    [TestFixture]
    public class MeasureTests
    {
        public MeasureTests()
        {
            AdHocConfig.UseEmpty();
        }

        [Test]
        public void AnyNumberIsDimensionlessMeasure()
        {
            var measure = (Measure) 1.2m;
            measure.Value.IsEqualTo(1.2m);
        }

        [Test]
        public void MultiplyWithUnitGivesUnitMeasure()
        {
            var m = new Measure(1.2m);
            var m2 = m * Meter;
            m2.Unit.IsEqualTo(Meter);
        }

        [Test]
        public void MeasuresCanBeMultiplied()
        {
            var m1 = new Measure(2.0m, Meter);
            var m2 = new Measure(6.0m, Second);

            var m3 = m1*m2;

            m3.Value.IsEqualTo(12.0m);
            m3.Unit.IsEqualTo(Meter * Second);

        }

        [Test]
        public void MeasuresCanBeDivided()
        {
            var m1 = new Measure(6.0m, Meter);
            var m2 = new Measure(2.0m, Second);

            var m3 = m1 / m2;

            m3.Value.IsEqualTo(3.0m);
            m3.Unit.IsEqualTo(Meter / Second);
        }

        [Test]
        public void MeasureFromNumberAndUnit()
        {
            (3.2m * Minute).IsEqualTo(new Measure(3.2m, Minute));
            (5 * GetRootUnit("Palette")).IsEqualTo(new Measure(5, GetRootUnit("Palette")));
        }

        [Test]
        public void MultiplicationWithNumber()
        {
            var m1 = new Measure(6.0m, Meter);
            var m2 = 3 * m1;
            var m3 = m1 * 3;
            m2.IsEqualTo(new Measure(18.0m, Meter));
            m3.IsEqualTo(new Measure(18.0m, Meter));
        }

        [Test]
        public void DivisionWithNumber()
        {
            var m1 = new Measure(12.0m, Meter);
            var m2 = m1 / 2;
            m2.IsEqualTo(new Measure(6.0m, Meter));
        }

        [Test]
        public void UsingExponent()
        {
            var m1 = 3 * Meter;
            var m2 = m1 ^ 3;
            m2.IsEqualTo(new Measure(27m, Meter.Cubed()));
        }

        [Test]
        public void SimplifyWritingMeasurePerUnit()
        {
            var m1 = 3 * Meter / Second;
            m1.IsEqualTo(new Measure(3m, Meter / Second));
        }

        [Test]
        public void StringOutput1()
        {
            (Meter * Second).ToString().IsEqualTo("m*sec");
        }

        [Test]
        public void StringOutput2()
        {
            (Meter.Squared() / Second).ToString().IsEqualTo("m*m/sec");
        }

        [Test]
        public void CannotAddApplesToOranges()
        {
            AdHocConfig.Use(c =>
                                {
                                    c.Unit(Foot).IsPhysicalUnit(_LENGTH);
                                    c.Unit(Gram).IsPhysicalUnit(_MASS);
                                });
            var m1 = new Measure(1, Foot);
            var m2 = new Measure(1, Gram);
            Assert.Throws<InvalidOperationException>(() =>
            {
                var unused = m1 + m2;
            });
        }

        [Test]
        public void CanAddGrannySmithToGoldenDelicious()
        {
            AdHocConfig.Use(c => c.Unit(Gram).IsPhysicalUnit(_MASS).StartScale().To(Kilogram, 1000));

            var m1 = new Measure(1, Kilogram);
            var m2 = new Measure(1, Gram);
            var m3 = m1 + m2;
            m3.Unit.IsEqualTo(Kilogram);
            m3.Value.IsEqualTo(1.001m);
        }

        [Test]
        public void AdditionOfMeasuresIsSupported()
        {
            var m1 = new Measure(1, Foot);
            var m2 = new Measure(1, Foot);
            var m3 = m1 + m2;
            m3.Unit.IsEqualTo(Foot);
            m3.Value.IsEqualTo(2);
        }

        [Test]
        public void SubtractionOfMeasuresIsSupported()
        {
            var m1 = new Measure(1, Foot);
            var m2 = new Measure(1, Foot);
            var m3 = m1 - m2;
            m3.Unit.IsEqualTo(Foot);
            m3.Value.IsEqualTo(0);
        }



        [Test]
        public void AttemptToConvertWithoutinfoGivesInvalidOpException()
        {
            AdHocConfig.UseEmpty();
            Assert.Throws<InvalidOperationException>(() => { var unused = new Measure(1, Kilogram).ConvertTo(Gram); });
        }

        [Test]
        public void MeasuresSupportEquality()
        {
            var m1 = new Measure(1, Milligram);
            var m2 = new Measure(1, Milligram);
            m1.Equals(m2).IsTrue();
        }

        [Test]
        public void CheckWhereCancelingOutDidntWork()
        {
            StandardUnitConfiguration.Use();
            var dAir = 1.225m * Kilogram / Meter.Cubed();
            var someVolume = (1 * Meter) ^ 3;

            Debug.WriteLine(dAir.ToString());
            Debug.WriteLine(someVolume.ToString());

            var measure = (dAir * someVolume);
            measure.Unit.IsEqualTo(Kilogram);

            measure.ToString(CultureInfo.InvariantCulture).IsEqualTo("1.225kg");

        }


    }
}
