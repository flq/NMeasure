using System;
using Xunit;
using Xunit.Abstractions;
using static NMeasure.U;

namespace NMeasure.Tests
{

    public class MeasureTests
    {
        private ITestOutputHelper _output;

        public MeasureTests(ITestOutputHelper output)
        {
            _output = output;
            AdHocConfig.UseEmpty();
        }

        [Fact]
        public void AnyNumberIsDimensionlessMeasure()
        {
            var measure = (Measure) 1.2m;
            measure.Value.IsEqualTo(1.2m);
        }

        [Fact]
        public void MultiplyWithUnitGivesUnitMeasure()
        {
            var m = new Measure(1.2m);
            var m2 = m * Meter;
            m2.Unit.IsEqualTo(Meter);
        }

        [Fact]
        public void MeasuresCanBeMultiplied()
        {
            var m1 = new Measure(2.0m, Meter);
            var m2 = new Measure(6.0m, Second);

            var m3 = m1*m2;

            m3.Value.IsEqualTo(12.0m);
            m3.Unit.IsEqualTo(Meter * Second);

        }

        [Fact]
        public void MeasuresCanBeDivided()
        {
            var m1 = new Measure(6.0m, Meter);
            var m2 = new Measure(2.0m, Second);

            var m3 = m1 / m2;

            m3.Value.IsEqualTo(3.0m);
            m3.Unit.IsEqualTo(Meter / Second);
        }

        [Fact]
        public void MeasureFromNumberAndUnit()
        {
            (3.2m * Minute).IsEqualTo(new Measure(3.2m, Minute));
            (5 * GetRootUnit("Palette")).IsEqualTo(new Measure(5, GetRootUnit("Palette")));
        }

        [Fact]
        public void MultiplicationWithNumber()
        {
            var m1 = new Measure(6.0m, Meter);
            var m2 = 3 * m1;
            var m3 = m1 * 3;
            m2.IsEqualTo(new Measure(18.0m, Meter));
            m3.IsEqualTo(new Measure(18.0m, Meter));
        }

        [Fact]
        public void DivisionWithNumber()
        {
            var m1 = new Measure(12.0m, Meter);
            var m2 = m1 / 2;
            m2.IsEqualTo(new Measure(6.0m, Meter));
        }

        [Fact]
        public void UsingExponent()
        {
            var m1 = 3 * Meter;
            var m2 = m1 ^ 3;
            m2.IsEqualTo(new Measure(27m, Meter.Cubed()));
        }

        [Fact]
        public void SimplifyWritingMeasurePerUnit()
        {
            var m1 = 3 * Meter / Second;
            m1.IsEqualTo(new Measure(3m, Meter / Second));
        }

        [Fact]
        public void StringOutput1()
        {
            (Meter * Second).ToString().IsEqualTo("m*sec");
        }

        [Fact]
        public void StringOutput2()
        {
            (Meter.Squared() / Second).ToString().IsEqualTo("m*m/sec");
        }

        [Fact]
        public void CannotAddApplesToOranges()
        {
            AdHocConfig.Use(c =>
                                {
                                    c.Unit(Foot).IsPhysicalUnit(_LENGTH);
                                    c.Unit(Gram).IsPhysicalUnit(_MASS);
                                });
            var m1 = new Measure(1, Foot);
            var m2 = new Measure(1, Gram);
            Assert.Throws<InvalidOperationException>(() => { var m3 = m1 + m2; });
        }

        [Fact]
        public void CanAddGrannySmithToGoldenDelicious()
        {
            AdHocConfig.Use(c => c.Unit(Gram).IsPhysicalUnit(_MASS).StartScale().To(Kilogram, 1000));

            var m1 = new Measure(1, Kilogram);
            var m2 = new Measure(1, Gram);
            var m3 = m1 + m2;
            m3.Unit.IsEqualTo(Kilogram);
            m3.Value.IsEqualTo(1.001m);
        }

        [Fact]
        public void AdditionOfMeasuresIsSupported()
        {
            var m1 = new Measure(1, Foot);
            var m2 = new Measure(1, Foot);
            var m3 = m1 + m2;
            m3.Unit.IsEqualTo(Foot);
            m3.Value.IsEqualTo(2);
        }

        [Fact]
        public void SubtractionOfMeasuresIsSupported()
        {
            var m1 = new Measure(1, Foot);
            var m2 = new Measure(1, Foot);
            var m3 = m1 - m2;
            m3.Unit.IsEqualTo(Foot);
            m3.Value.IsEqualTo(0);
        }



        [Fact]
        public void AttemptToConvertWithoutinfoGivesInvalidOpException()
        {
            AdHocConfig.UseEmpty();
            Assert.Throws<InvalidOperationException>(() => { var m1 = new Measure(1, Kilogram).ConvertTo(Gram); });
        }

        [Fact]
        public void MeasuresSupportEquality()
        {
            var m1 = new Measure(1, Milligram);
            var m2 = new Measure(1, Milligram);
            m1.Equals(m2);
        }

        [Fact]
        public void CheckWhereCancelingOutDidntWork()
        {
            StandardUnitConfiguration.Use();
            var dAir = 1.225m * Kilogram / Meter.Cubed();
            var someVolume = (1 * Meter) ^ 3;

            _output.WriteLine(dAir.ToString());
            _output.WriteLine(someVolume.ToString());

            var measure = (dAir * someVolume);
            measure.Unit.IsEqualTo(Kilogram);

            measure.ToString().IsEqualTo("1,225kg");

        }


    }
}
