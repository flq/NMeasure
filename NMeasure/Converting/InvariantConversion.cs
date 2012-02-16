namespace NMeasure.Converting
{
    internal class InvariantConversion : IConversion
    {
        public Measure Convert(Measure measure)
        {
            return measure;
        }
    }
}