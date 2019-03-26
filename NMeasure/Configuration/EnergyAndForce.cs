namespace NMeasure
{
    public class EnergyAndForce : IUnitConfigurationExtension
    {
        public void Extend(UnitConfiguration c)
        {
            c.Unit(U.Joule)
                .IsPhysicalUnit((U._MASS * U._LENGTH.Squared()) / U._TIME.Squared())
                .EquivalentTo(U.Kilogram * U.Meter.Squared() / U.Second.Squared())
                .ConvertibleTo(U.Kilogram,
                  m => m / PhysicalConstants.EnergyMassFactor,
                  m => m * PhysicalConstants.EnergyMassFactor)
                .ConvertibleTo(U.ElectronVolt, j => 6.242e+18m * j, e => 1.6022e-19m * e)
                .StartScale()
                .To(U.KiloJoule, 1000)
                .To(U.MegaJoule, 1000)
                .To(U.GigaJoule, 1000)
                .To(U.TeraJoule, 1000);

            c.Unit(U.Newton)
                .IsPhysicalUnit(U._MASS * U._LENGTH / U._TIME.Squared())
                .EquivalentTo(U.Kilogram * U.Meter / U.Second.Squared());
        }
    }
}
