using System;
using Xunit;

namespace NMeasure.Tests
{
    
    public class MeasureTests
    {
        public MeasureTests()
        {
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
            var m2 = m * U.Meter;
            m2.Unit.IsEqualTo(U.Meter);
        }

        [Fact]
        public void MeasuresCanBeMultiplied()
        {
            var m1 = new Measure(2.0m, U.Meter);
            var m2 = new Measure(6.0m, U.Second);

            var m3 = m1*m2;

            m3.Value.IsEqualTo(12.0m);
            m3.Unit.IsEqualTo(U.Meter * U.Second);

        }

        [Fact]
        public void MeasuresCanBeDivided()
        {
            var m1 = new Measure(6.0m, U.Meter);
            var m2 = new Measure(2.0m, U.Second);

            var m3 = m1 / m2;

            m3.Value.IsEqualTo(3.0m);
            m3.Unit.IsEqualTo(U.Meter / U.Second);
        }

        [Fact]
        public void MeasureFromNumberAndUnit()
        {
            (3.2m * U.Minute).IsEqualTo(new Measure(3.2m, U.Minute));
            (5 * U.GetRootUnit("Palette")).IsEqualTo(new Measure(5, U.GetRootUnit("Palette")));
        }

        [Fact]
        public void MultiplicationWithNumber()
        {
            var m1 = new Measure(6.0m, U.Meter);
            var m2 = 3 * m1;
            var m3 = m1 * 3;
            m2.IsEqualTo(new Measure(18.0m, U.Meter));
            m3.IsEqualTo(new Measure(18.0m, U.Meter));
        }

        [Fact]
        public void DivisionWithNumber()
        {
            var m1 = new Measure(12.0m, U.Meter);
            var m2 = m1 / 2;
            m2.IsEqualTo(new Measure(6.0m, U.Meter));
        }



        [Fact]
        public void StringOutput1()
        {
            (U.Meter * U.Second).ToString().IsEqualTo("m*sec");
        }

        [Fact]
        public void StringOutput2()
        {
            (U.Meter.Squared() / U.Second).ToString().IsEqualTo("m*m/sec");
        }

        [Fact]
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

        [Fact]
        public void CanAddGrannySmithToGoldenDelicious()
        {
            AdHocConfig.Use(c => c.Unit(U.Gram).IsPhysicalUnit(U._MASS).StartScale().To(U.Kilogram, 1000));

            var m1 = new Measure(1, U.Kilogram);
            var m2 = new Measure(1, U.Gram);
            var m3 = m1 + m2;
            m3.Unit.IsEqualTo(U.Kilogram);
            m3.Value.IsEqualTo(1.001m);
        }

        [Fact]
        public void AdditionOfMeasuresIsSupported()
        {
            var m1 = new Measure(1, U.Foot);
            var m2 = new Measure(1, U.Foot);
            var m3 = m1 + m2;
            m3.Unit.IsEqualTo(U.Foot);
            m3.Value.IsEqualTo(2);
        }

        [Fact]
        public void SubtractionOfMeasuresIsSupported()
        {
            var m1 = new Measure(1, U.Foot);
            var m2 = new Measure(1, U.Foot);
            var m3 = m1 - m2;
            m3.Unit.IsEqualTo(U.Foot);
            m3.Value.IsEqualTo(0);
        }



        [Fact]
        public void AttemptToConvertWithoutinfoGivesInvalidOpException()
        {
            AdHocConfig.UseEmpty();
            Assert.Throws<InvalidOperationException>(() => { var m1 = new Measure(1, U.Kilogram).ConvertTo(U.Gram); });
        }

        [Fact]
        public void MeasuresSupportEquality()
        {
            var m1 = new Measure(1, U.Milligram);
            var m2 = new Measure(1, U.Milligram);
            m1.Equals(m2);
        }
    }
}
