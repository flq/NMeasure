using System;

namespace NMeasure
{
    public static class PhysicalConstants
    {
        private static readonly Lazy<Measure> energyMassFactor = new Lazy<Measure>(() => SpeedOfLight * SpeedOfLight);
        private static readonly Lazy<Measure> boltzmannConstant = new Lazy<Measure>(() => new Measure(1.3806503E-23, U.Joule / U.Kelvin));
        private static readonly Lazy<Measure> speedOfLight = new Lazy<Measure>(() => new Measure(299792458, U.Meter / U.Second));
        private static readonly Lazy<Measure> electronMass = new Lazy<Measure>(() => new Measure(9.10938188E-31, U.Kilogram));
        private static readonly Lazy<Measure> planckConstant = new Lazy<Measure>(() => new Measure(6.626068E-34, U.Joule * U.Second));
        private static readonly Lazy<Measure> gravitationalConstant = new Lazy<Measure>(() => new Measure(6.673E-11, U.Meter.Cubed() / (U.Second.Squared() * U.Kilogram)));

        public static Measure EnergyMassFactor { get { return energyMassFactor.Value; } }

        public static Measure SpeedOfLight { get { return speedOfLight.Value; } }

        public static Measure BoltzmannConstant { get { return boltzmannConstant.Value; } }
        
        public static Measure ElectronMass { get { return electronMass.Value; } }
        
        public static Measure PlanckConstant { get { return planckConstant.Value; } }

        public static Measure GravitationalConstant { get { return gravitationalConstant.Value; } }
    }
}