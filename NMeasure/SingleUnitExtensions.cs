namespace NMeasure
{
    public static class SingleUnitExtensions
    {
        public static Unit Squared(this Unit unit)
        {
            return unit * unit;
        }

        public static Unit Cubed(this Unit unit)
        {
            return unit.Squared() * unit;
        }
    }
}