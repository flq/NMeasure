namespace NMeasure
{
    public static class SingleUnitExtensions
    {
        public static Unit Times(this SingleUnit unit1, SingleUnit unit)
        {
            return new Unit(unit1) * unit;
        }

        public static Unit Per(this SingleUnit unit1, SingleUnit unit)
        {
            return new Unit(unit1) / unit;
        }
    }
}