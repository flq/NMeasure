using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NMeasure
{
    public class ExpandedUnit
    {
        public readonly ReadOnlyCollection<U> Numerators;
        public readonly ReadOnlyCollection<U> Denominators;

        public ExpandedUnit(IList<U> numerators, IList<U> denominators)
        {
            Numerators = new ReadOnlyCollection<U>(numerators);
            Denominators = new ReadOnlyCollection<U>(denominators);
        }
    }
}