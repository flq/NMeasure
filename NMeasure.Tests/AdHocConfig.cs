using System;

namespace NMeasure.Tests
{
    public class AdHocConfig : UnitConfiguration
    {
        public static void Use(Action<UnitConfiguration> action)
        {
            new AdHocConfig(action);
        }

        public AdHocConfig(Action<UnitConfiguration> action)
        {
            action(this);
        }
    }
}