using static NMeasure.U;

namespace NMeasure.Configuration
{
    public class Lengths : IUnitConfigurationExtension
    {
        public void Extend(UnitConfiguration c)
        {
            c.Unit(Nanometer)
                .IsPhysicalUnit(_LENGTH)
                .StartScale()
                .To(Micrometer, 1000)
                .To(Millimeter, 1000)
                .To(Centimeter, 10)
                .To(Meter, 100)
                .To(Kilometer, 1000);

            c.Unit(Microinch)
                .IsPhysicalUnit(_LENGTH)
                .StartScale()
                .To(Inch, 1000000)
                .To(Foot, 12)
                .To(Yard, 3)
                .To(Mile, 1760);

            c.Unit(Inch).ConvertValueBased(Centimeter, v => v * 2.54m, v => 0.393700787m * v);
            c.Unit(Mile).ConvertValueBased(Kilometer, v => v * 1.609344m, v => 0.621371192m * v);
        }
    }
}
