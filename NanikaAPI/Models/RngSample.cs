namespace NanikaAPI.Models
{
    public class RngSample<T>
    {
        public RngSample(T value, double probability, double resetratio)
        {
            this.Value = value;
            this.Probability = BaseProbability = probability;
            this.ResetRatio = resetratio;
        }
        public T Value { get; set; }
        public double? Probability { get; set; }
        public double? BaseProbability { get; set; }
        public double? ResetRatio { get; set; }
        public (int, int)? Interval { get; set; }
    }
}
