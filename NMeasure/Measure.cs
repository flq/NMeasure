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

        public Measure(double value, SingleUnit unit) : this(value, Unit.From(unit))
        {
        }

        public bool IsDimensionless
        {
            get { return Unit.IsDimensionless; }
        }

        public static explicit operator Measure(double value)
        {
            return new Measure(value);
        }

        public static Measure operator *(Measure x, SingleUnit singleUnit)
        {
            return new Measure(x.Value, x.Unit * singleUnit);
        }

        public static Measure operator *(Measure x, Measure y)
        {
            return new Measure(x.Value * y.Value, x.Unit * y.Unit);
        }

        public static Measure operator /(Measure x, Measure y)
        {
            return new Measure(x.Value / y.Value, x.Unit / y.Unit);
        }

        
    }
}