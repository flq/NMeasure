using System;
using System.Collections.Generic;
using System.Linq;

namespace NMeasure
{
    internal class ConversionTracker
    {
        private readonly UnitGraph unitGraph;

        public ConversionTracker(UnitGraph unitGraph)
        {
            this.unitGraph = unitGraph;
        }

        public IConversion FindConversionSequence(Unit unit, Unit target)
        {
            // It can happen that the system requests unit conversion between the same unit. In this case we can have the cake easily:
            if (unit == target)
                return new InvariantConversion();
            if (unitGraph[unit] == null)
                return decomposeAndSearch(unit, target);
            // sequences are comparable by count of edges. The smallest count should lead to smaller calc errors.
            return unitGraph[unit].Conversions.SelectMany(edge=> findConversionSequence(new UnitGraphEdgeSequence { edge }, target)).Min();
        }

        private IConversion decomposeAndSearch(Unit unit, Unit target)
        {
            var exStart = unit.Expand();
            var exEnd = target.Expand();

            var matches = matchViaPhysicalUnits(exStart, exEnd);

            var numeratorconverters = matches.NumeratorPairs.Select(pair => FindConversionSequence(pair.Item1, pair.Item2)).ToList();
            // For denominators we search in the opposite way to give the inverse conversion, as they are divisors
            // This may lead to issues where a conversion doesn't run purely by multiplication (off hand I only remember conversion from celsius to fahrenheit)
            // But let's live with it for now.
            var denominatorConverters = matches.DenominatorPairs.Select(pair => FindConversionSequence(pair.Item2, pair.Item1));
            return new ComplexConversion(numeratorconverters, denominatorConverters);
        }

        private static MatchStructure matchViaPhysicalUnits(ExpandedUnit exStart, ExpandedUnit exEnd)
        {
            var ms = new MatchStructure();
            var mutatingList = new List<Unit>(exEnd.Numerators);
            foreach (var u in exStart.Numerators)
            {
                var u2 = mutatingList.First(unit => unit.ToPhysicalUnit() == u.ToPhysicalUnit());
                ms.AddNumeratorPair(u, u2);
                mutatingList.Remove(u2);
            }

            mutatingList = new List<Unit>(exEnd.Denominators);

            foreach (var u in exStart.Denominators)
            {
                var u2 = mutatingList.First(unit => unit.ToPhysicalUnit() == u.ToPhysicalUnit());
                ms.AddDenominatorPair(u, u2);
                mutatingList.Remove(u2);
            }
            return ms;
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

        private class MatchStructure
        {
            private readonly List<Tuple<Unit, Unit>> numerators = new List<Tuple<Unit, Unit>>();
            private readonly List<Tuple<Unit, Unit>> denominators = new List<Tuple<Unit, Unit>>();


            public void AddNumeratorPair(Unit startUnit, Unit endUnit)
            {
                numerators.Add(Tuple.Create(startUnit,endUnit));
            }

            public void AddDenominatorPair(Unit startUnit, Unit endUnit)
            {
                denominators.Add(Tuple.Create(startUnit, endUnit));
            }

            public IEnumerable<Tuple<Unit, Unit>> NumeratorPairs
            {
                get { return numerators; }
            }

            public IEnumerable<Tuple<Unit, Unit>> DenominatorPairs
            {
                get { return denominators; }
            }
        }
    }
}