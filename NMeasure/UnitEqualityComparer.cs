using System.Collections.Generic;

namespace NMeasure
{
    /// <summary>
    /// For some reason the standard equality comparer is kicking my butt,
    /// even though Equals returns true and GetHashCode returns the same
    /// I do not get the correct key
    /// </summary>
    internal class UnitEqualityComparer : IEqualityComparer<Unit>
    {
        public bool Equals(Unit x, Unit y)
        {
            return x.GetHashCode() == y.GetHashCode();
        }

        public int GetHashCode(Unit obj)
        {
            return obj.GetHashCode();
        }
    }
}