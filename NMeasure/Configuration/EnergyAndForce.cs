using static NMeasure.U;

namespace NMeasure.Configuration
{
    public class EnergyAndForce : IUnitConfigurationExtension
    {
        public void Extend(UnitConfiguration c)
        {
            c.Unit(Joule)
                .IsPhysicalUnit((_MASS * _LENGTH.Squared()) / _TIME.Squared())
                .EquivalentTo(Kilogram * Meter.Squared() / Second.Squared())
                .ConvertibleTo(Kilogram,
                  m => m / PhysicalConstants.EnergyMassFactor,
                  m => m * PhysicalConstants.EnergyMassFactor)
                .ConvertibleTo(ElectronVolt, j => 6.242e+18m * j, e => 1.6022e-19m * e)
                .StartScale()
                .To(KiloJoule, 1000)
                .To(MegaJoule, 1000)
                .To(GigaJoule, 1000)
                .To(TeraJoule, 1000);

            c.Unit(Newton)
                .IsPhysicalUnit(_MASS * _LENGTH / _TIME.Squared())
                .EquivalentTo(Kilogram * Meter / Second.Squared());
        }
    }
}
