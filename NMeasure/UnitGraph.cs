using System;
using System.Collections.Generic;

namespace NMeasure
{
    internal class UnitGraph
    {
        private readonly Dictionary<Unit,UnitGraphNode> nodes = new Dictionary<Unit, UnitGraphNode>();

        public UnitGraphNode this[Unit unit]
        {
            get
            {
                UnitGraphNode node;
                nodes.TryGetValue(unit, out node);
                return node;
            }
        }

        public UnitGraphNode AddUnit(Unit unit)
        {
            UnitGraphNode unitGraphNode;
            var success = nodes.TryGetValue(unit, out unitGraphNode);
            if (!success)
                nodes.Add(unit, unitGraphNode = new UnitGraphNode(unit));
            return unitGraphNode;
        }

        public void AddConversion(UnitGraphNode from, UnitGraphNode to, Func<double, double> toTo, Func<double, double> toFrom)
        {
            from.AddConversion(to, toTo);
            to.AddConversion(from, toFrom);
        }

        public void AddConversion(UnitGraphNode from, UnitGraphNode to, Func<Measure, Measure> toTo, Func<Measure, Measure> toFrom)
        {
            from.AddConversion(to, toTo);
            to.AddConversion(from, toFrom);
        }

        //public Measure Convert(Measure measure, Unit target)
        //{
        //    var conversionSequence = GetConverter(measure.Unit, target);
        //    return conversionSequence.Convert(measure);
        //}

        //public IConversion GetConverter(Unit from, Unit to)
        //{
        //    var seq = new ConversionTracker(this);
        //    var conversionSequence = seq.FindConversionSequence(from, to);
        //    if (conversionSequence == null)
        //        throw new InvalidOperationException("No conversion could be found between the provided units.");
        //    return conversionSequence;
        //}

    }
}