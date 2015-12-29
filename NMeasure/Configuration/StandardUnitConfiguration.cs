using NMeasure.Configuration;
using static NMeasure.U;

namespace NMeasure
{
    /// <summary>
    /// Applies all Unit extensions contained in NMeasure
    /// </summary>
    public class StandardUnitConfiguration : UnitConfiguration
    {
        public static void Use()
        {
            new StandardUnitConfiguration();
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