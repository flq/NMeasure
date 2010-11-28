using System.Collections.Generic;
using System.Linq;

namespace NMeasure
{
    public class ConversionTracker
    {
        private readonly UnitGraph unitGraph;

        public ConversionTracker(UnitGraph unitGraph)
        {
            this.unitGraph = unitGraph;
        }

        public UnitGraphEdgeSequence FindConversionSequence(Unit unit, Unit target)
        {
            return unitGraph[unit].Conversions.SelectMany(e=> findConversionSequence(new UnitGraphEdgeSequence { e }, target)).FirstOrDefault();
        }

        private IEnumerable<UnitGraphEdgeSequence> findConversionSequence(UnitGraphEdgeSequence currentRoute, Unit target)
        {
            var sequences = new List<UnitGraphEdgeSequence>();
            var to = currentRoute.Last.To;
            if (to.Equals(target))
            {
                sequences.Add(currentRoute);
                return sequences;
            }

            foreach (var c in unitGraph[to].Conversions)
            {
                if (currentRoute.References(c))
                    continue;
                var newRoute = currentRoute.Clone();
                newRoute.Add(c);
                var innerSequences = findConversionSequence(newRoute, target);
                sequences.AddRange(innerSequences.Where(seq=>seq.Last.To.Equals(target)));
            }
            return sequences;
        }
    }
}