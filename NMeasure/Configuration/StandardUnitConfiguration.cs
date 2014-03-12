namespace NMeasure
{
    public class StandardUnitConfiguration : UnitConfiguration
    {
        public static void Use()
        {
            new StandardUnitConfiguration();
        }

        public StandardUnitConfiguration()
        {
            SetMeasurePrecision(4);
            
            lengths();
            times();
            masses();
            temperatures();
            areas();
            pressures();

            Unit(U.Joule)
                .IsPhysicalUnit((U._MASS*U._LENGTH.Squared())/U._TIME.Squared())
                .EquivalentTo(U.Kilogram*U.Meter.Squared()/U.Second.Squared())
                .ConvertibleTo(U.Kilogram, 
                  m => m/PhysicalConstants.EnergyMassFactor,
                  m => m*PhysicalConstants.EnergyMassFactor);

            Unit(U.Newton)
                .IsPhysicalUnit(U._MASS*U._LENGTH / U._TIME.Squared())
                .EquivalentTo(U.Kilogram*U.Meter / U.Second.Squared());
        }

        private void pressures()
        {
            Unit(U.Pascal)
                .IsPhysicalUnit(U._MASS/(U._TIME.Squared()*U._LENGTH))
                .EquivalentTo(U.Newton/U.Meter.Squared())
                .ConvertValueBased(U.Bar, v => v*1E-5m, v => v*100000)
                .ConvertValueBased(U.Psi, v => 6.894E+3m, v => v*145.04E-6m);
        }

        private void areas()
        {
            Unit(U.SquareMeter)
                .IsPhysicalUnit(U._LENGTH.Squared())
                .EquivalentTo(U.Meter.Squared())
                .ConvertValueBased(U.Hectare, v => v*0.0001m, v => v*10000);
        }

        private void temperatures()
        {
            Unit(U.Celsius)
                .IsPhysicalUnit(U._TEMPERATURE)
                .ConvertValueBased(U.Kelvin, v => v + 273.15m, v => v - 273.15m);

            Unit(U.Fahrenheit)
                .IsPhysicalUnit(U._TEMPERATURE)
                .ConvertValueBased(U.Celsius, v => (v - 32)*(5m/9m), v => v*(9m/5m) + 32);
        }

        private void masses()
        {
            Unit(U.Milligram)
                .IsPhysicalUnit(U._MASS)
                .StartScale()
                .To(U.Carat,200)
                .To(U.Gram, 5)
                .To(U.Kilogram, 1000)
                .To(U.Ton, 1000);

            Unit(U.Ounce)
                .IsPhysicalUnit((U._MASS))
                .StartScale()
                .To(U.Pound, 16);

            Unit(U.Gram).ConvertValueBased(U.Ounce, v => v*0.0352739619m, v => v*28.3495231m);
            Unit(U.Pound).ConvertValueBased(U.Kilogram, v => v * 0.45359237m, v => v * 2.20462262m);

            Unit(U.Kilogram).EquivalentTo(U.Joule*U.Second.Squared()/U.Meter.Squared());
        }

        private void times()
        {
            Unit(U.Nanosecond)
                .IsPhysicalUnit(U._TIME)
                .StartScale()
                .To(U.Microsecond, 1000)
                .To(U.Millisecond, 1000)
                .To(U.Second, 1000)
                .To(U.Minute, 60)
                .To(U.Hour, 60)
                .To(U.Day, 24);

        }

        private void lengths()
        {
            Unit(U.Nanometer)
                .IsPhysicalUnit(U._LENGTH)
                .StartScale()
                .To(U.Micrometer, 1000)
                .To(U.Millimeter, 1000)
                .To(U.Centimeter, 10)
                .To(U.Meter, 100)
                .To(U.Kilometer, 1000);

            Unit(U.Microinch)
                .IsPhysicalUnit(U._LENGTH)
                .StartScale()
                .To(U.Inch, 1000000)
                .To(U.Foot, 12)
                .To(U.Yard, 3)
                .To(U.Mile,1760);

            Unit(U.Inch).ConvertValueBased(U.Centimeter, v => v*2.54m, v => 0.393700787m*v);
            Unit(U.Mile).ConvertValueBased(U.Kilometer, v => v * 1.609344m, v => 0.621371192m * v);
        }
    }
}