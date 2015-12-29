using static NMeasure.U;

namespace NMeasure.Configuration
{
    public class Temperatures : IUnitConfigurationExtension
    {
        public void Extend(UnitConfiguration c)
        {
            c.Unit(Celsius)
               .IsPhysicalUnit(_TEMPERATURE)
               .ConvertValueBased(Kelvin, v => v + 273.15m, v => v - 273.15m);

            c.Unit(Fahrenheit)
                .IsPhysicalUnit(_TEMPERATURE)
                .ConvertValueBased(Celsius, v => (v - 32) * (5m / 9m), v => v * (9m / 5m) + 32);
        }
    }
}
