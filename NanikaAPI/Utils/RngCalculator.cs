using NanikaAPI.Models;

namespace NanikaAPI.Utils
{
    public class RngCalculator<T> : List<RngSample<T>>
    {
        public int Ceil { get => _ceil; private set => _ceil = value; }
        private int _ceil = 0;
        private readonly bool _allowPity;
        public RngCalculator(IEnumerable<RngSample<T>> collection, bool allowPitySystem = false) : base(collection) 
        {
            _allowPity = allowPitySystem;
            UpdateCollection();
        }

        private void UpdateCollection()
        {
            Sort(new RngSampleComparer<T>());
            Ceil = 0;
            for (int i = 0; i<Count; i++)
            {
                int value = (int)Math.Floor(this[i].Probability!.Value);
                if (value <= 1) this[i].Interval = (Ceil, Ceil);
                else this[i].Interval = (Ceil, Ceil + value - 1);
                Ceil += value;
            }
        }
        /// <summary>
        /// <para>When getting a pull with a reset ratio, the probability of each item will approach the base value by the ratio.</para>
        /// <para>If not, then it will increase the probability of other samples, starting from the most rare to last sample before the pull,
        /// decreasing the increment each time.</para>
        /// </summary>
        /// <param name="pullIndex"></param>
        private void ApplyPity(int pullIndex)
        {
            if ((this[pullIndex].ResetRatio ?? 0) > 0)
            {
                ResetBase(this[pullIndex].ResetRatio!.Value);
                UpdateCollection();
            }
            else
            {
                double ratio = Ceil / this[pullIndex].Probability!.Value / 100.0;
                for (int i = 0; i < pullIndex; i++)
                {
                    this[i].Probability *= 1.0 + ratio;
                    ratio *= 0.8;
                    UpdateCollection();
                }
            }  
        }

        private void ResetBase(double ratio)
        {
            for (int i = 0; i < Count; i++)
            {
                this[i].Probability += (this[i].BaseProbability - this[i].Probability) * ratio;
            }
        }
        private int IntervalBinarySearch(int number) => IntervalBinarySearch(number, 0, Count-1);
        private int IntervalBinarySearch(int number, int left, int right)
        {
            int middle = (left + right) / 2;
            if (middle <= left) return RngSampleComparer<T>.CompareInterval(number, this[middle]) == 0 ? middle : right;
            if (RngSampleComparer<T>.CompareInterval(number, this[middle]) >= 1) return IntervalBinarySearch(number, middle, right);
            else if (RngSampleComparer<T>.CompareInterval(number, this[middle]) <= -1) return IntervalBinarySearch(number, left, middle);
            else return middle;
        }

        public RngSample<T> Pull() => Pull(new Random().Next(0, Ceil));
        public RngSample<T> Pull(int number)
        {
            number = Math.Clamp(number, 0, Ceil-1);
            int pullIndex = IntervalBinarySearch(number);
            if (_allowPity) ApplyPity(pullIndex);
            return this[pullIndex];
        }


    }
}
