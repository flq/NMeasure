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
            private readonly string _id;

            public FundamentalUnit(string id) { _id = id; }
            public override bool IsFundamental { get { return true; } }
            public override bool IsDimensionless { get { return false; } }
            public override bool Equals(Unit other) { return ReferenceEquals(this, other); }
            public override int  GetHashCode() { return _id.GetHashCode(); }
            public override string ToString() { return _id; }
        }

        private class RootUnit : Unit, IComparable
        {
            private readonly string _id;
            public RootUnit(string id) { _id = id; }
            public override bool Equals(Unit obj) { return obj.ToString().Equals(ToString()); }
            public override int GetHashCode() { return _id.GetHashCode(); }
            public override bool IsDimensionless { get { return false; } }
            public override bool IsFundamental { get { return false; } }
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

        public static readonly Unit _MASS = new FundamentalUnit("[MASS]");
        public static readonly Unit _TIME = new FundamentalUnit("[TIME]");
        public static readonly Unit _LENGTH = new FundamentalUnit("[LENGTH]");

        public static readonly Unit Dimensionless = new AnyUnit();
        public static readonly Unit Kilometer = GetRootUnit("Kilometer");
        public static readonly Unit Meter = GetRootUnit("Meter");
        public static readonly Unit Centimeter = GetRootUnit("Centimeter");
        public static readonly Unit Millimeter = GetRootUnit("Millimeter");
        public static readonly Unit Milligram = GetRootUnit("Milligram");
        public static readonly Unit Kilogram = GetRootUnit("Kilogram");
        public static readonly Unit Gram = GetRootUnit("Gram");
        public static readonly Unit Second = GetRootUnit("Second");
        public static readonly Unit Foot = GetRootUnit("Foot");
        public static readonly Unit Joule = GetRootUnit("Joule");
        public static readonly Unit Kelvin = GetRootUnit("Kelvin");
        public static Unit Inch = GetRootUnit("Inch");

        public static Unit GetRootUnit(string unit)
        {
            return new AnyUnit(new [] { new RootUnit(unit) }, new Unit[0]);
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