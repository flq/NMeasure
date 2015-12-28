using System;
using System.Collections.Generic;
using System.Linq;

namespace NMeasure
{
    public static class UnitExtensions
    {
        public static Unit Squared(this Unit unit)
        {
            return unit * unit;
        }

        public static Unit Cubed(this Unit unit)
        {
            return unit.Squared() * unit;
        }

        public static UnitMeta GetUnitData(this Unit unit)
        {
            return UnitConfiguration.UnitSystem[unit];
        }

        public static Unit ToPhysicalUnit(this Unit unit)
        {
            var unitMeta = GetUnitData(unit);
            if (unitMeta != null)
                return unitMeta.PhysicalUnit;

            return DerivePhysicalUnitsFromConstituentParts(unit);
        }

        public static bool IsDimensionless(this Unit unit)
        {
            return Unit.IsDimensionless(unit);
        }

        public static Measure ConvertTo(this Measure measure, Unit unit)
        {
            return UnitConfiguration.UnitSystem.UnitGraph.Convert(measure, unit);
        }

        internal static string CreateStringRepresentation(this Unit unit, string format = null)
        {
            Func<Unit, string> selector = u => u.ToString();

            if (unit.IsDimensionless())
                return string.Empty;
            var exp = unit.Expand();
            if (exp.Denominators.Count == 0)
                return string.Join(UnitMeta.MultiplicationSign, exp.Numerators.Select(selector));
            return string.Concat(
                string.Join(
                    UnitMeta.MultiplicationSign,
                    exp.Numerators.Select(selector)),
                UnitMeta.DivisionSign,
                string.Join(
                    UnitMeta.MultiplicationSign,
                    exp.Denominators.Select(selector)));
        }

        internal static Unit TryCompaction(this Unit unit)
        {
            return UnitConfiguration.UnitSystem.GetEquivalent(unit) ?? unit;
        }

        internal static Unit DerivePhysicalUnitsFromConstituentParts(this Unit unit)
        {
            var expandedUnit = unit.Expand();

            try
            {
                var numerators = expandedUnit.Numerators.ToPhysicalUnit();

                if (expandedUnit.Denominators.Count > 0)
                {
                    var denominators = expandedUnit.Denominators.ToPhysicalUnit();
                    return numerators / denominators;
                }
                return numerators;
            }
            catch (NullReferenceException x)
            {
                throw new InvalidOperationException("No metadata could be derived for unit " + unit + ". Have you forgotten to run a configuration?", x);
            }
        }

        private static Unit ToPhysicalUnit(this IEnumerable<Unit> units)
        {
            return units.Select(GetUnitData).Select(info => info.PhysicalUnit).Aggregate((u1, u2) => u1 * u2);
        }
    }
}