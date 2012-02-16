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
            get { return speedOfLight.IsDimensionless ? speedOfLight = new Measure(299792458, U.Meter / U.Second) : speedOfLight; }
        }

        private static Measure boltzmannConstant;
        public static Measure BoltzmannConstant
        {
            get { return boltzmannConstant.IsDimensionless ? boltzmannConstant = new Measure(1.3806503E-23, U.Joule / U.Kelvin) : boltzmannConstant; }
        }

        private static Measure electronMass;
        public static Measure ElectronMass
        {
            get { return electronMass.IsDimensionless ? electronMass = new Measure(9.10938188E-31, U.Kilogram) : electronMass; }
        }

        private static Measure planckConstant;
        public static Measure PlanckConstant
        {
            get { return planckConstant.IsDimensionless ? planckConstant = new Measure(6.626068E-34, U.Joule * U.Second) : planckConstant; }
        }

        private static Measure gravitationalConstant;
        public static Measure GravitationalConstant
        {
            get { return gravitationalConstant.IsDimensionless ? gravitationalConstant = new Measure(6.673E-11, U.Meter.Cubed() / (U.Second.Squared() * U.Kilogram)) : gravitationalConstant; }
        }
    }
}