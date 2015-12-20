using static NMeasure.U;

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

            Unit(Joule)
                .IsPhysicalUnit((_MASS*_LENGTH.Squared())/_TIME.Squared())
                .EquivalentTo(Kilogram*Meter.Squared()/Second.Squared())
                .ConvertibleTo(Kilogram, 
                  m => m/PhysicalConstants.EnergyMassFactor,
                  m => m*PhysicalConstants.EnergyMassFactor);

            Unit(Newton)
                .IsPhysicalUnit(_MASS*_LENGTH / _TIME.Squared())
                .EquivalentTo(Kilogram*Meter / Second.Squared());
        }

        private void pressures()
        {
            Unit(Pascal)
                .IsPhysicalUnit(_MASS/(_TIME.Squared()*_LENGTH))
                .EquivalentTo(Newton/Meter.Squared())
                .ConvertValueBased(Bar, v => v*1E-5m, v => v*100000)
                .ConvertValueBased(Psi, v => 6.894E+3m, v => v*145.04E-6m);
        }

        private void areas()
        {
            Unit(SquareMeter)
                .IsPhysicalUnit(_LENGTH.Squared())
                .EquivalentTo(Meter.Squared())
                .ConvertValueBased(Hectare, v => v*0.0001m, v => v*10000);
        }

        private void temperatures()
        {
            Unit(Celsius)
                .IsPhysicalUnit(_TEMPERATURE)
                .ConvertValueBased(Kelvin, v => v + 273.15m, v => v - 273.15m);

            Unit(Fahrenheit)
                .IsPhysicalUnit(_TEMPERATURE)
                .ConvertValueBased(Celsius, v => (v - 32)*(5m/9m), v => v*(9m/5m) + 32);
        }

        private void masses()
        {
            Unit(Milligram)
                .IsPhysicalUnit(_MASS)
                .StartScale()
                .To(Carat,200)
                .To(Gram, 5)
                .To(Kilogram, 1000)
                .To(Ton, 1000);

            Unit(Ounce)
                .IsPhysicalUnit((_MASS))
                .StartScale()
                .To(Pound, 16);

            Unit(Gram).ConvertValueBased(Ounce, v => v*0.0352739619m, v => v*28.3495231m);
            Unit(Pound).ConvertValueBased(Kilogram, v => v * 0.45359237m, v => v * 2.20462262m);

            Unit(Kilogram).EquivalentTo(Joule*Second.Squared()/Meter.Squared());
        }

        private void times()
        {
            Unit(Nanosecond)
                .IsPhysicalUnit(_TIME)
                .StartScale()
                .To(Microsecond, 1000)
                .To(Millisecond, 1000)
                .To(Second, 1000)
                .To(Minute, 60)
                .To(Hour, 60)
                .To(Day, 24);

        }

        private void lengths()
        {
            Unit(Nanometer)
                .IsPhysicalUnit(_LENGTH)
                .StartScale()
                .To(Micrometer, 1000)
                .To(Millimeter, 1000)
                .To(Centimeter, 10)
                .To(Meter, 100)
                .To(Kilometer, 1000);

            Unit(Microinch)
                .IsPhysicalUnit(_LENGTH)
                .StartScale()
                .To(Inch, 1000000)
                .To(Foot, 12)
                .To(Yard, 3)
                .To(Mile,1760);

            Unit(Inch).ConvertValueBased(Centimeter, v => v*2.54m, v => 0.393700787m*v);
            Unit(Mile).ConvertValueBased(Kilometer, v => v * 1.609344m, v => 0.621371192m * v);
        }
    }
}