using System.Collections.Generic;
using System.Linq;

namespace NMeasure
{
    public class Unit
    {
        private readonly List<U> numerators = new List<U>();
        private readonly List<U> denominators = new List<U>();

        public Unit(U singleUnit) : this()
        {
            if (singleUnit == U.Dimensionless)
                return;
            numerators.Add(singleUnit);
        }

        public Unit()
        {
            
        }

        public bool IsDimensionless
        {
            get { return numerators.Count == 0 && denominators.Count == 0; }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Unit)) return false;
            return Equals((Unit) obj);
        }

        public bool Equals(Unit other)
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

        public static Unit From(U singleUnit)
        {
            return new Unit(singleUnit);
        }

        public static Unit From(IEnumerable<U> numerators = null, IEnumerable<U> denominators = null)
        {
            var u = new Unit();
            if (numerators != null)
                u.numerators.AddRange(numerators);
            if (denominators != null)
                u.denominators.AddRange(denominators);
            return u;
        }

        public static Unit Inverse(U singleUnit)
        {
            var u = new Unit();
            u.denominators.Add(singleUnit);
            return u;
        }

        public static Unit operator *(Unit unit, U singleUnit)
        {
            var newUnit = unit.Clone();
            if (newUnit.denominators.Contains(singleUnit))
                newUnit.denominators.Remove(singleUnit);
            else
                newUnit.numerators.Add(singleUnit);
            return newUnit;
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

        public static Unit operator /(Unit unit, U singleUnit)
        {
            var newUnit = unit.Clone();
            if (newUnit.numerators.Contains(singleUnit))
                newUnit.numerators.Remove(singleUnit);
            else
              newUnit.denominators.Add(singleUnit);
            return newUnit;
        }

        public ExpandedUnit Expand()
        {
            return new ExpandedUnit(numerators, denominators);
        }

        public override string ToString()
        {
            return stringRepresentation();
        }
    }
}