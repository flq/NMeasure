using System;
using System.Diagnostics;

namespace NMeasure
{
    [DebuggerDisplay("{Value} {Unit.stringRepresentation()}")]
    public struct Measure
    {
        public readonly double Value;
        public readonly Unit Unit;

        public Measure(double value) : this(value, new Unit())
        {
        }

        public Measure(double value, Unit unit)
        {
            Value = Math.Round(value, UnitConfiguration.UnitSystem.Precision, MidpointRounding.AwayFromZero);
            Unit = unit;
        }

        public Measure(double value, U unit) : this(value, Unit.From(unit))
        {
        }

        public bool IsDimensionless
        {
            get { return Unit == null || Unit.IsDimensionless; }
        }

        public static explicit operator Measure(double value)
        {
            return new Measure(value);
        }

        public static Measure operator *(Measure x, U singleUnit)
        {
            return new Measure(x.Value, x.Unit * singleUnit);
        }

        public static Measure operator *(Measure x, Unit unit)
        {
            return new Measure(x.Value, (x.Unit * unit).TryCompaction());
        }

        public static Measure operator *(Measure x, Measure y)
        {
            return new Measure(x.Value * y.Value, (x.Unit * y.Unit).TryCompaction());
        }

        public static Measure operator +(Measure x, Measure y)
        {
            if (x.Unit.Equals(y.Unit))
                return new Measure(x.Value + y.Value, x.Unit);

            if (x.Unit.ToPhysicalUnit().Equals(y.Unit.ToPhysicalUnit()))
                return x + y.ConvertTo(x.Unit);
            throw new InvalidOperationException("These measures cannot be sensibly added to a single new measure");
        }

        public static Measure operator /(Measure x, Measure y)
        {
            return new Measure(x.Value / y.Value, (x.Unit / y.Unit).TryCompaction());
        }

        public static Measure operator -(Measure x, Measure y)
        {
            if (x.Unit.Equals(y.Unit))
                return new Measure(x.Value + y.Value, x.Unit);

            if (x.Unit.ToPhysicalUnit().Equals(y.Unit.ToPhysicalUnit()))
                return x - y.ConvertTo(x.Unit);
            throw new InvalidOperationException("These measures cannot be sensibly added to a single new measure");
        }

        public override string ToString()
        {
            return Value.ToString() + Unit;
        }
    }
}