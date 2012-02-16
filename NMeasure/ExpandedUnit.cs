using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NMeasure
{
    internal class ExpandedUnit
    {
        public readonly ReadOnlyCollection<Unit> Numerators;
        public readonly ReadOnlyCollection<Unit> Denominators;

        public ExpandedUnit(IList<Unit> numerators, IList<Unit> denominators)
        {
            Numerators = new ReadOnlyCollection<Unit>(numerators);
            Denominators = new ReadOnlyCollection<Unit>(denominators);
        }
    }
}