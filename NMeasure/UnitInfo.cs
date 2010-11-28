using System.Linq;

namespace NMeasure
{
    public static class UnitInfo
    {

        private static UnitConfiguration getConfig()
        {
            return UnitConfiguration.UnitSystem;
        }

        public static UnitMeta GetUnitData(this SingleUnit unit)
        {
            return getConfig()[Unit.From(unit)];
        }

        public static UnitMeta GetUnitData(this Unit unit)
        {
            return getConfig()[unit];
        }

        public static Unit ToPhysicalUnit(this Unit unit)
        {
            var expandedUnit = unit.Expand();

            var numerators = from u in expandedUnit.Numerators
                             select GetUnitData(u).PhysicalUnit;
            var denominators = from u in expandedUnit.Denominators
                               select GetUnitData(u).PhysicalUnit;

            return Unit.From(numerators, denominators);
        }

        public static Measure ConvertTo(this Measure measure, Unit unit)
        {
            return UnitConfiguration.UnitSystem.UnitGraph.Convert(measure, unit);
        }

        public static Measure ConvertTo(this Measure measure, SingleUnit unit)
        {
            return measure.ConvertTo(Unit.From(unit));
        }
    }
}