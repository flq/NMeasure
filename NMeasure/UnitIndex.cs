using System.Collections.Generic;

namespace NMeasure
{
    /// <summary>
    /// A dictionary whose keys are units, correctly set up with a
    /// fitting equality comparer
    /// </summary>
    public class UnitIndex<V> : Dictionary<Unit,V>
    {
        public UnitIndex() : base(new UnitEqualityComparer()) { }
    }
}