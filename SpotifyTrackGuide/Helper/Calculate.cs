using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyTrackGuide.Helper
{
    public static class Calculate
    {
        public static double GetAverageTotal(List<float> records)
        {
            double average = records.Count > 0 ? records.Average() : 0.0;
            return average;
        }

        public static double GetMinValue(List<float> records)
        {
            double min = records.Min(x => x);
            return min;
        }

        public static double GetMaxValue(List<float> records)
        {
            double min = records.Max(x => x);
            return min;
        }
    }
}
