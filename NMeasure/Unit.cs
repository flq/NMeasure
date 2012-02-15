using System.Collections.Generic;
using System.Linq;

namespace NMeasure
{
    public class Unit
    {
        private readonly List<Unit> numerators = new List<Unit>();
        private readonly List<Unit> denominators = new List<Unit>();

        protected Unit() {}
        
        protected Unit(IEnumerable<Unit> numerators, IEnumerable<Unit> denominators)
        {
            this.numerators.AddRange(numerators);
            this.denominators.AddRange(denominators);
        }

        public virtual bool IsDimensionless
        {
            get { return numerators.Count == 0 && denominators.Count == 0; }
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
            return stringRepresentation().GetHashCode();
        }

        private Unit Clone()
        {
            var u = new Unit();
            u.numerators.AddRange(numerators);
            u.denominators.AddRange(denominators);
            return u;
        }

        public ExpandedUnit Expand()
        {
            return new ExpandedUnit(numerators, denominators);
        }

        public override string ToString()
        {
            return stringRepresentation();
        }

        private string stringRepresentation()
        {
            numerators.Sort();
            denominators.Sort();
            if (IsDimensionless)
                return "Dimensionless";
            if (denominators.Count == 0)
                return string.Join("*", numerators);
            return string.Concat(string.Join("*", numerators), "/", string.Join("*", denominators));
        }

        public Unit Inverse()
        {
            var expanded = Expand();
            return new Unit(expanded.Denominators, expanded.Numerators);
        }

        public static Unit operator *(Unit unit1, Unit unit2)
        {
            var newUnit = unit1.Clone();

            foreach (var u in unit2.numerators)
            {
                if (newUnit.denominators.Contains(u))
                    newUnit.denominators.Remove(u);
                else
                    newUnit.numerators.Add(u);
            }

            foreach (var u in unit2.denominators)
            {
                if (newUnit.numerators.Contains(u))
                    newUnit.numerators.Remove(u);
                else
                    newUnit.denominators.Add(u);
            }

            return newUnit;
        }

        public static Unit operator /(Unit unit1, Unit unit2)
        {
            var newUnit = new Unit();
            newUnit.numerators.AddRange(unit2.denominators);
            newUnit.denominators.AddRange(unit2.numerators);

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
    }
}