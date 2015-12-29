using static NMeasure.U;

namespace NMeasure.Configuration
{
    public class Areas : IUnitConfigurationExtension
    {
        public void Extend(UnitConfiguration c)
        {
            c.Unit(Meter ^ 2)
                .ConvertValueBased(Hectare, v => v * 0.0001m, v => v * 10000);
        }
    }
}
