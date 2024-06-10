using NanikaAPI.Models;

namespace NanikaAPI.Utils
{
    public class RngSampleComparer<T> : IComparer<RngSample<T>>
    {
        public int Compare(RngSample<T>? x, RngSample<T>? y)
        {
            return x?.Probability > y?.Probability ? 1 : x?.Probability < y?.Probability ? -1 : 0;
        }

        public static int CompareInterval(int number, RngSample<T> x)
        {
            return number < x.Interval?.Item1 ? -1 : number > x.Interval?.Item2 ? 1 : 0;
        }
    }
}
