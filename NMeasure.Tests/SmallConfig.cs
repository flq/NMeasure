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
            Unit(U.Meter)
                .BelongsToTypeSystem(NMeasure.UnitSystem.SI, NMeasure.UnitSystem.Metric)
                .IsPhysicalUnit(U._LENGTH);
        }
    }
}