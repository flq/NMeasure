using System;
using System.Collections.Generic;

namespace NMeasure
{
    // ReSharper disable InconsistentNaming
    /// <summary>
    /// Markers for all units known to the system
    /// </summary>
    public static class U
    {
        private class FundamentalUnit : Unit
        {
            private readonly object hash = new object();
            public override bool IsFundamental { get { return true; } }
            public override bool Equals(Unit other) { return ReferenceEquals(this, other); }
            public override int  GetHashCode() { return hash.GetHashCode(); }
        }

        private class RootUnit : Unit, IComparable
        {
            private readonly string _id;
            public RootUnit(string id) { _id = id; }
            public override bool Equals(Unit obj) { return ReferenceEquals(this, obj); }
            public override int GetHashCode() { return _id.GetHashCode(); }
            public override bool IsDimensionless { get { return false; } }
            public override string ToString() { return _id; }

            int IComparable.CompareTo(object obj)
            {
                if (!(obj is RootUnit)) return 0;
                return _id.CompareTo(((RootUnit)obj)._id);
            }
        }

        private class AnyUnit : Unit
        {
            public AnyUnit() { }
            public AnyUnit(IEnumerable<Unit> numerators, IEnumerable<Unit> denominators) : base(numerators, denominators) {}
        }

        public static readonly Unit _MASS = new FundamentalUnit();
        public static readonly Unit _TIME = new FundamentalUnit();
        public static readonly Unit _LENGTH = new FundamentalUnit();

        public static readonly Unit Dimensionless = new AnyUnit();
        public static readonly Unit Meter = FromRootUnit(new RootUnit("Meter"));
        public static readonly Unit Milligram = FromRootUnit(new RootUnit("Milligram"));
        public static readonly Unit Kilogram = FromRootUnit(new RootUnit("Kilogram"));
        public static readonly Unit Gram = FromRootUnit(new RootUnit("Gram"));
        public static readonly Unit Second = FromRootUnit(new RootUnit("Second"));

        public static readonly Unit Foot = FromRootUnit(new RootUnit("Foot"));

        private static AnyUnit FromRootUnit(RootUnit unit)
        {
            return new AnyUnit(new [] { unit }, new Unit[0]);
        }

        //Dimensionless,
        ///// <summary>
        ///// Physical Unit of Mass
        ///// </summary>
        //_MASS,
        //Milligram,
        //Carat,
        //Gram,
        //Kilogram,
        //Ton,
        //Ounce,
        //Pound,
        ///// <summary>
        ///// Physical Unit of Space
        ///// </summary>
        //_LENGTH,
        //Nanometer,
        //Micrometer,
        //Millimeter,
        //Centimeter,
        //Meter,
        //Kilometer,
        //Microinch,
        //Inch,
        //Foot,
        //Yard,
        //Mile,
        ///// <summary>
        ///// Physical Unit of Time
        ///// </summary>
        //_TIME,
        //Nanosecond,
        //Microsecond,
        //Millisecond,
        //Second,
        //Minute,
        //Hour,
        //Day,
        ///// <summary>
        ///// Physical Unit of Temperature
        ///// </summary>
        //_TEMPERATURE,
        //Kelvin,
        //Celsius,
        //Fahrenheit,
        //// Others
        //Joule,
        //Newton,
        //SquareMeter,
        //Hectare,
        //Pascal,
        //Bar,
        //Psi
    }
    // ReSharper restore InconsistentNaming


}