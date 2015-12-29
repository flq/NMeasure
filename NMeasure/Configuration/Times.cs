using static NMeasure.U;

namespace NMeasure.Configuration
{
    public class Times : IUnitConfigurationExtension
    {
        public void Extend(UnitConfiguration c)
        {
            c.Unit(Nanosecond)
                .IsPhysicalUnit(_TIME)
                .StartScale()
                .To(Microsecond, 1000)
                .To(Millisecond, 1000)
                .To(Second, 1000)
                .To(Minute, 60)
                .To(Hour, 60)
                .To(Day, 24);
        }
    }
}
