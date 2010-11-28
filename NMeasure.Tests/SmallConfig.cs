namespace NMeasure.Tests
{
    public class SmallConfig : UnitConfiguration
    {
        public static void Use()
        {
            new SmallConfig();
        }

        public SmallConfig()
        {
            Unit(SingleUnit.Meter)
                .BelongsToTypeSystem(NMeasure.UnitSystem.SI, NMeasure.UnitSystem.Metric)
                .IsPhysicalUnit(SingleUnit._LENGTH);
        }
    }
}