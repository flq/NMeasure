using System;
using System.Diagnostics;

namespace NMeasure
{
    [DebuggerDisplay("{Value} {Unit}")]
    public struct Measure
    {
        public readonly decimal Value;
        public readonly Unit Unit;

        public Measure(decimal value) : this(value, U.Dimensionless)
        {
        }

        public Measure(decimal value, Unit unit)
        {
            Value = Math.Round(value, UnitConfiguration.UnitSystem.Precision(unit), MidpointRounding.AwayFromZero);
            Unit = unit;
        }


        public static implicit operator Measure(decimal value)
        {
            return new Measure(value);
        }

        public static Measure operator *(Measure x, Unit unit) { return new Measure(x.Value, (x.Unit * unit)); }

        public static Measure operator *(Measure x, Measure y) { return new Measure(x.Value * y.Value, (x.Unit * y.Unit)); }

        public static Measure operator *(decimal x, Measure y) { return new Measure(x * y.Value, y.Unit); }

        public static Measure operator *(int x, Measure y) { return new Measure(x * y.Value, y.Unit); }

        public static Measure operator *(Measure y, decimal x) { return new Measure(x * y.Value, y.Unit); }

        public static Measure operator *(Measure y, int x) { return new Measure(x * y.Value, y.Unit); }

        public static Measure operator /(Measure y, decimal x) { return new Measure(y.Value / x, y.Unit); }

        public static Measure operator /(Measure y, int x) { return new Measure(y.Value / x, y.Unit); }

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
            return new Measure(x.Value / y.Value, (x.Unit / y.Unit));
        }

        public static Measure operator -(Measure x, Measure y)
        {
            if (x.Unit.Equals(y.Unit))
                return new Measure(x.Value - y.Value, x.Unit);

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