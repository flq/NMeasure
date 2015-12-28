using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NMeasure
{
    /// <summary>
    /// This class represents a physical unit. It follows the rules with regard to multiplying and dividing Units.
    /// You will find a set of known units in the class <see cref="U"/>, file "KnownUnits"
    /// </summary>
    public class Unit : IEquatable<Unit>
    {
        private readonly ReadOnlyCollection<Unit> _numerators;
        private readonly ReadOnlyCollection<Unit> _denominators;
        private readonly string _stringRepresentation;

        protected Unit() : this(Enumerable.Empty<Unit>(), Enumerable.Empty<Unit>()) { }

        protected Unit(IEnumerable<Unit> numerators, IEnumerable<Unit> denominators)
        {
            _numerators = new ReadOnlyCollection<Unit>(numerators.OrderBy(u => u).ToList());
            _denominators = new ReadOnlyCollection<Unit>(denominators.OrderBy(u => u).ToList());
            _stringRepresentation = this.CreateStringRepresentation();
        }

        /// <summary>
        /// Fundamental units are physical units like Length or time, i.e. Units with physical significance
        /// but not bound to any Unit System like Metric or Imperial. A combination of Fundamental units (e.g. Length / Time)
        /// also counts as fundamental.
        /// </summary>
        public virtual bool IsFundamental
        {
            get { return _numerators.All(u => u.IsFundamental) && _denominators.All(u => u.IsFundamental); }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as Unit);
        }

        public bool Equals(Unit other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(ToString(), other.ToString());
        }

        public override int GetHashCode()
        {
            var val = ToString();
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            return val == null ? 0 : val.GetHashCode();
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
        }

        public override string ToString()
        {
            if (this.IsDimensionless())
                return string.Empty;
            return _stringRepresentation ?? string.Empty;
        }

        public string ToString(string format)
        {
            return this.CreateStringRepresentation(format);
        }

        public static Unit Parse(string txt)
        {
            return string.IsNullOrEmpty(txt) ? null : U.GetRootUnit(txt);
        }

        /// <summary>
        /// States whether the unit is dimensionless
        /// </summary>
        public static bool IsDimensionless(Unit unit)
        {
            return ReferenceEquals(null, unit) || unit.DetermineIsDimensionless();
        }

        protected virtual bool DetermineIsDimensionless()
        {
            return HasNoNumeratorsNorDenominators();
        }

        private bool HasNoNumeratorsNorDenominators()
        {
            return _numerators.Count == 0 && _denominators.Count == 0;
        }

        /// <summary>
        /// This returns the unit inverted, i.e. if you'd calculate 1 / Unit
        /// </summary>
        public Unit Inverse()
        {
            var expanded = Expand();
            return new Unit(expanded.Denominators, expanded.Numerators);
        }

        public static Unit operator *(Unit unit1, Unit unit2)
        {
            var denominators = unit1._denominators.ToList();
            var numerators = unit1._numerators.ToList();

            if (IsRootFundamentalUnit(unit1))
                numerators.Add(unit1);
            if (unit2.HasNoNumeratorsNorDenominators() && unit2.IsFundamental && !unit2.IsDimensionless())
                numerators.Add(unit2);

            foreach (var u in unit2._numerators)
            {
                if (denominators.Contains(u))
                    denominators.Remove(u);
                else
                    numerators.Add(u);
            }

            foreach (var u in unit2._denominators)
            {
                if (numerators.Contains(u))
                    numerators.Remove(u);
                else
                    denominators.Add(u);
            }

            return new Unit(numerators, denominators).TryCompaction();
        }

        public static Unit operator /(Unit unit1, Unit unit2)
        {
            var newUnit = new Unit(unit2._denominators, unit2._numerators);
            return unit1 * newUnit;
        }

        public static Unit operator ^(Unit unit, int exponent)
        {
            return new U.AnyUnit(Enumerable.Repeat(unit, exponent), Enumerable.Empty<Unit>());
        }


        public static Measure operator *(decimal value, Unit unit)
        {
            return new Measure(value, unit);
        }

        public static Measure operator *(int value, Unit unit)
        {
            return new Measure(value, unit);
        }

        public static bool operator ==(Unit x, Unit y)
        {
            if (!ReferenceEquals(x, null))
                return x.Equals(y);

            return ReferenceEquals(y, null);
        }

        public static bool operator !=(Unit x, Unit y)
        {
            return !(x == y);
        }

        internal ExpandedUnit Expand()
        {
            return new ExpandedUnit(_numerators, _denominators);
        }

        private static bool IsRootFundamentalUnit(Unit unit)
        {
            return unit.HasNoNumeratorsNorDenominators() && unit.IsFundamental && !unit.IsDimensionless();
        }
    }
}