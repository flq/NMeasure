using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace NMeasure
{
    /// <summary>
    /// A factory to create your own units for starting a unit system.
    /// Note that during the lifetime and within the scope of the factory, 
    /// strings will be validated for uniqueness, as this is necessary for proper
    /// unit equality to work. This string will also be used for basic string output of units.
    /// </summary>
    public interface IUnitFactory
    {
        /// <summary>
        /// A fundamental unit relates a multitude of units to the fundamental thing that they
        /// are actually measuring (e.g. length or time)
        /// NMeasure has taken the convention to write it as [UNIT] (e.g. [LENGTH]).
        /// </summary>
        Unit CreateFundamentalUnit(string unit);

        /// <summary>
        /// Create a basic unit which you can use in calculations
        /// </summary>
        Unit CreateRootUnit(string unit);
    }

    // ReSharper disable InconsistentNaming
    /// <summary>
    /// All units known to the system and used by the <see cref="StandardUnitConfiguration"/>
    /// </summary>
    public static class U
    {
        private class FundamentalUnit : Unit, IComparable
        {
            private readonly string _id;
            private readonly bool _isDimensionless;

            public FundamentalUnit(string id, bool isDimensionless = false)
            {
                _id = id;
                _isDimensionless = isDimensionless;

            }
            public override bool IsFundamental { get { return true; } }

            public override string ToString() { return _id; }

            int IComparable.CompareTo(object obj)
            {
                if (!(obj is FundamentalUnit)) return 0;
                return String.CompareOrdinal(_id, ((FundamentalUnit)obj)._id);
            }

            protected override bool DetermineIsDimensionless()
            {
                return _isDimensionless;
            }
        }

        private class RootUnit : Unit, IComparable
        {
            private readonly string _id;
            public RootUnit(string id) { _id = id; }
            public override bool IsFundamental { get { return false; } }
            public override string ToString() { return _id; }

            int IComparable.CompareTo(object obj)
            {
                if (!(obj is RootUnit)) return 0;
                return String.CompareOrdinal(_id, ((RootUnit)obj)._id);
            }

            protected override bool DetermineIsDimensionless()
            {
                return false;
            }
        }

        internal class AnyUnit : Unit
        {
            public AnyUnit() { }
            public AnyUnit(IEnumerable<Unit> numerators, IEnumerable<Unit> denominators) : base(numerators, denominators) {}
        }

        /// <summary>
        /// Provides a unit factory as a basis for your own unit system
        /// </summary>
        public static IUnitFactory GetUnitFactory()
        {
            return new UnitFactory();
        }

        public static Unit GetRootUnit(string unit)
        {
            return new AnyUnit(new [] { new RootUnit(unit) }, new Unit[0]);
        }

        public static readonly Unit Dimensionless = new AnyUnit();

        // ------- LENGTH --------
        public static readonly Unit _LENGTH = new FundamentalUnit("[LENGTH]");
        public static readonly Unit Kilometer = GetRootUnit("km");
        public static readonly Unit Meter = GetRootUnit("m");
        public static readonly Unit Centimeter = GetRootUnit("cm");
        public static readonly Unit Millimeter = GetRootUnit("mm");
        public static readonly Unit Nanometer = GetRootUnit("nm");
        public static readonly Unit Micrometer = GetRootUnit("µ");

        public static readonly Unit Mile = GetRootUnit("mi");
        public static readonly Unit Yard = GetRootUnit("yd");
        public static readonly Unit Foot = GetRootUnit("ft");
        public static readonly Unit Inch = GetRootUnit("in");
        public static readonly Unit Microinch = GetRootUnit("µin");
        
        public static readonly Unit SquareMeter = GetRootUnit("m²");
        public static readonly Unit Hectare = GetRootUnit("ha");
        // ------- MASS --------
        public static readonly Unit _MASS = new FundamentalUnit("[MASS]");
        public static readonly Unit Ton = GetRootUnit("t");
        public static readonly Unit Kilogram = GetRootUnit("kg");
        public static readonly Unit Gram = GetRootUnit("g");
        public static readonly Unit Milligram = GetRootUnit("mg");
        public static readonly Unit Carat = GetRootUnit("kt");
        public static readonly Unit Ounce = GetRootUnit("oz");
        public static readonly Unit Pound = GetRootUnit("lb");
        // ------- TIME --------
        public static readonly Unit _TIME = new FundamentalUnit("[TIME]");
        public static readonly Unit Day = GetRootUnit("d");
        public static readonly Unit Hour = GetRootUnit("h");
        public static readonly Unit Minute = GetRootUnit("min");
        public static readonly Unit Second = GetRootUnit("sec");
        public static readonly Unit Millisecond = GetRootUnit("ms");
        public static readonly Unit Microsecond = GetRootUnit("µs");
        public static readonly Unit Nanosecond = GetRootUnit("ns");
        // ------- PRESSURE --------
        public static readonly Unit Pascal = GetRootUnit("Pa");
        public static readonly Unit Psi = GetRootUnit("psi");
        public static readonly Unit Bar = GetRootUnit("bar");

        // ------- TEMPERATURE --------
        public static readonly Unit _TEMPERATURE = new FundamentalUnit("[TEMPERATURE]");
        public static readonly Unit Kelvin = GetRootUnit("K");
        public static readonly Unit Celsius = GetRootUnit("°C");
        public static readonly Unit Fahrenheit = GetRootUnit("°F");

        public static readonly Unit ElectronVolt = GetRootUnit("eV");
        public static readonly Unit Joule = GetRootUnit("J");
        public static readonly Unit KiloJoule = GetRootUnit("kJ");
        public static readonly Unit MegaJoule = GetRootUnit("MJ");
        public static readonly Unit GigaJoule = GetRootUnit("GJ");
        public static readonly Unit TeraJoule = GetRootUnit("TJ");

        public static readonly Unit Newton = GetRootUnit("N");


        static U()
        {
            // This is a self-check, since the functionality heavily depends on all units being different and there is an off-chance that
            // a Hashcode is not unique (especially when somebody copy-pastes a Unit)
            var allUnits = typeof(U)
                .GetRuntimeFields()
                .Where(fi => typeof(Unit).GetTypeInfo().IsAssignableFrom(fi.FieldType))
                .Select(fi => fi.GetValue(null))
                .OfType<Unit>().ToList();

            var distinctUnitsCount = allUnits.Distinct(new UnitEqualityComparer()).Count();

            if (allUnits.Count != distinctUnitsCount)
                throw new InvalidOperationException("The known units are not unique in themselves!");

        }

        private class UnitFactory : IUnitFactory
        {
            private readonly HashSet<string> _validation = new HashSet<string>();

            public Unit CreateFundamentalUnit(string unit)
            {
                Validate(unit);
                return new FundamentalUnit(unit);
            }

            public Unit CreateRootUnit(string unit)
            {
                Validate(unit);
                return GetRootUnit(unit);
            }

            private void Validate(string unit)
            {
                if (_validation.Contains(unit))
                    throw new InvalidOperationException($"Unit {unit} has already been created by this factory.");
                _validation.Add(unit);
            }
        }
    }
    // ReSharper restore InconsistentNaming

    
}