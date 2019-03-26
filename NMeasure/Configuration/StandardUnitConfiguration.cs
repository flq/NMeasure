using NMeasure.Configuration;

namespace NMeasure
{
    /// <summary>
    /// Applies all Unit extensions contained in NMeasure
    /// </summary>
    public class StandardUnitConfiguration : UnitConfiguration
    {
        public static UnitConfiguration Use()
        {
            return new StandardUnitConfiguration();
        }

        public StandardUnitConfiguration()
        {
            SetMeasurePrecision(4);

            Extend(
                new Lengths(), 
                new Times(), 
                new Masses(), 
                new Temperatures(), 
                new Areas(),
                new Pressures(),
                new EnergyAndForce());
        }
    }
}