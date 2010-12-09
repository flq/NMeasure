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

            Unit(U.Joule)
                .IsPhysicalUnit((U._MASS*U._LENGTH.Squared())/U._TIME.Squared())
                .CompactionOf(U.Kilogram*U.Meter.Squared()/U.Second.Squared())
                .ConvertibleTo(U.Kilogram.Unit(), 
                  m => m/PhysicalConstants.EnergyMassFactor,
                  m => m*PhysicalConstants.EnergyMassFactor);

            Unit(U.Newton)
                .IsPhysicalUnit(U._MASS*U._LENGTH.Unit()/U._TIME.Squared())
                .CompactionOf(U.Kilogram*U.Meter.Unit()/U.Second.Squared());
        }

        private void temperatures()
        {
            Unit(U.Celsius)
                .IsPhysicalUnit(U._TEMPERATURE)
                .ConvertibleTo(U.Kelvin, v => v + 273.15, v => v - 273.15);

            Unit(U.Fahrenheit)
                .IsPhysicalUnit(U._TEMPERATURE)
                .ConvertibleTo(U.Celsius, v => (v - 32)*(5d/9d), v => v*(9d/5d) + 32);
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

            Unit(U.Gram).ConvertibleTo(U.Ounce, v => v*0.0352739619, v => v*28.3495231);
            Unit(U.Pound).ConvertibleTo(U.Kilogram, v => v * 0.45359237, v => v * 2.20462262);

            Unit(U.Kilogram).CompactionOf(U.Joule*U.Second.Squared()/U.Meter.Squared());
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

            Unit(U.Inch).ConvertibleTo(U.Centimeter, v => v*2.54, v => 0.393700787*v);
            Unit(U.Mile).ConvertibleTo(U.Kilometer, v => v * 1.609344, v => 0.621371192 * v);
        }
    }
}