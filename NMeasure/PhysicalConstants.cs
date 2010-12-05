namespace NMeasure
{
    public static class PhysicalConstants
    {
        private static Measure energyMassFactor;
        public static Measure EnergyMassFactor
        {
            get { return energyMassFactor.IsDimensionless ? energyMassFactor = SpeedOfLight*SpeedOfLight : energyMassFactor; }
        }

        private static Measure speedOfLight;
        public static Measure SpeedOfLight
        {
            get { return speedOfLight.IsDimensionless ? speedOfLight = new Measure(299792458, U.Meter.Per(U.Second)) : speedOfLight; }
        }
    }
}