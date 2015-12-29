using static NMeasure.U;

namespace NMeasure.Configuration
{
    public class Masses : IUnitConfigurationExtension
    {
        public void Extend(UnitConfiguration c)
        {
            c.Unit(Milligram)
                .IsPhysicalUnit(_MASS)
                .StartScale()
                .To(Carat, 200)
                .To(Gram, 5)
                .To(Kilogram, 1000)
                .To(Ton, 1000);

            c.Unit(Ounce)
                .IsPhysicalUnit((_MASS))
                .StartScale()
                .To(Pound, 16);

            c.Unit(Gram).ConvertValueBased(Ounce, v => v * 0.0352739619m, v => v * 28.3495231m);
            c.Unit(Pound).ConvertValueBased(Kilogram, v => v * 0.45359237m, v => v * 2.20462262m);

        }
    }
}
