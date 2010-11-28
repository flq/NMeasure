using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NMeasure
{
    public class ExpandedUnit
    {
        public readonly ReadOnlyCollection<SingleUnit> Numerators;
        public readonly ReadOnlyCollection<SingleUnit> Denominators;

        public ExpandedUnit(IList<SingleUnit> numerators, IList<SingleUnit> denominators)
        {
            Numerators = new ReadOnlyCollection<SingleUnit>(numerators);
            Denominators = new ReadOnlyCollection<SingleUnit>(denominators);
        }
    }
}