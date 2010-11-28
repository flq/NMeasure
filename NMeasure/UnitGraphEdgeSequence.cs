using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NMeasure
{
    public class UnitGraphEdgeSequence : IEnumerable<UnitGraphEdge>
    {
        private readonly List<UnitGraphEdge> edges = new List<UnitGraphEdge>();

        private UnitGraphEdgeSequence(IEnumerable<UnitGraphEdge> edges)
        {
            this.edges = new List<UnitGraphEdge>(edges);
        }

        public UnitGraphEdgeSequence() { }

        public Measure Convert(Measure measure)
        {
            return edges.Aggregate(measure, (m, edge) => edge.ApplyConversion(m));
        }

        public IEnumerator<UnitGraphEdge> GetEnumerator()
        {
            return edges.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(UnitGraphEdge edge)
        {
            edges.Add(edge);
        }

        public UnitGraphEdge Last { get { return edges.Count > 0 ? edges[edges.Count - 1] : null; } }

        public bool References(UnitGraphEdge edge)
        {
            return this.Any(e => e.Equals(edge));
        }

        public UnitGraphEdgeSequence Clone()
        {
            return new UnitGraphEdgeSequence(this);
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, this);
        }
    }
}