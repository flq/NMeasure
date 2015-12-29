using static NMeasure.U;

namespace NMeasure.Configuration
{
    public class Pressures : IUnitConfigurationExtension
    {
        public void Extend(UnitConfiguration c)
        {
            c.Unit(Pascal)
                .IsPhysicalUnit(_MASS / (_TIME.Squared() * _LENGTH))
                .EquivalentTo(Newton / Meter.Squared())
                .ConvertValueBased(Bar, v => v * 1E-5m, v => v * 100000)
                .ConvertValueBased(Psi, v => 6.894E+3m, v => v * 145.04E-6m);
        }
    }
}
