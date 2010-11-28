namespace NMeasure
{
    public static class SingleUnitExtensions
    {
        public static Unit Times(this U unit1, U unit)
        {
            return new Unit(unit1) * unit;
        }

        public static Unit Per(this U unit1, U unit)
        {
            return new Unit(unit1) / unit;
        }
    }
}