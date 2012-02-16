using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NMeasure
{
    public class Unit
    {
        private readonly ReadOnlyCollection<Unit> numerators;
        private readonly ReadOnlyCollection<Unit> denominators;
        private readonly string stringRepresentation;

        protected Unit() : this(Enumerable.Empty<Unit>(), Enumerable.Empty<Unit>()) {}
        
        protected Unit(IEnumerable<Unit> numerators, IEnumerable<Unit> denominators)
        {
            this.numerators = new ReadOnlyCollection<Unit>(numerators.OrderBy(u => u).ToList());
            this.denominators = new ReadOnlyCollection<Unit>(denominators.OrderBy(u => u).ToList());
            stringRepresentation = CreateRepresentation();
        }

        public virtual bool IsDimensionless
        {
            get { return HasNoNumeratorsAndNoDenominators(); }
        }

        public virtual bool IsFundamental
        {
            get { return numerators.All(u => u.IsFundamental) && denominators.All(u => u.IsFundamental); }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Unit)) return false;
            return Equals((Unit) obj);
        }

        public virtual bool Equals(Unit other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.numerators.Except(numerators).Count() == 0 && other.denominators.Except(denominators).Count() == 0;
        }

        public override int GetHashCode()
        {
            return stringRepresentation.GetHashCode();
        }

        public ExpandedUnit Expand()
        {
            return new ExpandedUnit(numerators, denominators);
        }

        public override string ToString()
        {
            return stringRepresentation;
        }

        public Unit Inverse()
        {
            var expanded = Expand();
            return new Unit(expanded.Denominators, expanded.Numerators);
        }

        public static Unit operator *(Unit unit1, Unit unit2)
        {

            var denominators = unit1.denominators.ToList();
            var numerators = unit1.numerators.ToList();

            if (unit1.HasNoNumeratorsAndNoDenominators() && unit1.IsFundamental && !unit1.IsDimensionless)
                numerators.Add(unit1);
            if (unit2.HasNoNumeratorsAndNoDenominators() && unit2.IsFundamental && !unit2.IsDimensionless)
                numerators.Add(unit2);

            foreach (var u in unit2.numerators)
            {
                if (denominators.Contains(u))
                    denominators.Remove(u);
                else
                    numerators.Add(u);
            }

            foreach (var u in unit2.denominators)
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
            var newUnit = new Unit(unit2.denominators, unit2.numerators);
            return unit1*newUnit;
        }

        public static bool operator ==(Unit x, Unit y)
        {
            if (!ReferenceEquals(x, null))
                return x.Equals(y);
            if (!ReferenceEquals(y, null))
                return y.Equals(x);
            return true;
        }

        public static bool operator !=(Unit x, Unit y)
        {
            return !(x == y);
        }

        private bool HasNoNumeratorsAndNoDenominators()
        {
            return numerators.Count == 0 && denominators.Count == 0;
        }

        private string CreateRepresentation()
        {
            if (IsDimensionless)
                return "Dimensionless";
            if (denominators.Count == 0)
                return string.Join("*", numerators);
            return string.Concat(string.Join("*", numerators), "/", string.Join("*", denominators));
        }
    }
}