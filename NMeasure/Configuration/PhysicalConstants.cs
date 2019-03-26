using System;

namespace NMeasure
{
    public static class PhysicalConstants
    {
        // ReSharper disable InconsistentNaming
        private static readonly Lazy<Measure> energyMassFactor = new Lazy<Measure>(() => SpeedOfLight * SpeedOfLight);
        private static readonly Lazy<Measure> boltzmannConstant = new Lazy<Measure>(() => new Measure(1.3806503E-23m, U.Joule / U.Kelvin));
        private static readonly Lazy<Measure> speedOfLight = new Lazy<Measure>(() => new Measure(299792458, U.Meter / U.Second));
        private static readonly Lazy<Measure> electronMass = new Lazy<Measure>(() => new Measure(9.10938188E-31m, U.Kilogram));
        private static readonly Lazy<Measure> planckConstant = new Lazy<Measure>(() => new Measure(6.626068E-34m, U.Joule * U.Second));
        private static readonly Lazy<Measure> gravitationalConstant = new Lazy<Measure>(() => new Measure(6.673E-11m, U.Meter.Cubed() / (U.Second.Squared() * U.Kilogram)));
        // ReSharper restore InconsistentNaming

        public static Measure EnergyMassFactor => energyMassFactor.Value;

        public static Measure SpeedOfLight => speedOfLight.Value;

        public static Measure BoltzmannConstant => boltzmannConstant.Value;

        public static Measure ElectronMass => electronMass.Value;

        public static Measure PlanckConstant { get { return planckConstant.Value; } }

        public static Measure GravitationalConstant { get { return gravitationalConstant.Value; } }
    }
}