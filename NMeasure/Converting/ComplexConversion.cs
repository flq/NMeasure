using System.Collections.Generic;
using System.Linq;

namespace NMeasure.Converting
{
    internal class ComplexConversion : IConversion
    {
        private readonly IEnumerable<IConversion> numeratorconverters;
        private readonly IEnumerable<IConversion> denominatorConverters;

        public ComplexConversion(IEnumerable<IConversion> numeratorconverters, IEnumerable<IConversion> denominatorConverters)
        {
            this.numeratorconverters = numeratorconverters;
            this.denominatorConverters = denominatorConverters;
        }

        public Measure Convert(Measure measure)
        {
            //Actual conversions are relaxed about the units. At the end we should come out fine.
            return numeratorconverters
                .Concat(denominatorConverters)
                .Aggregate(measure, (m, edge) => edge.Convert(m));
        }
    }
}