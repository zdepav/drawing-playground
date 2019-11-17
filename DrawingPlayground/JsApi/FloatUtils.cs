using System;
using System.Linq;

namespace DrawingPlayground.JsApi {

    public static class FloatUtils {

        public static bool InfiniteOrNaN(float value) {
            return float.IsInfinity(value) || float.IsNaN(value);
        }
        
        public static bool InfiniteOrNaN(float v1, float v2) => InfiniteOrNaN(v1) || InfiniteOrNaN(v2);

        public static bool InfiniteOrNaN(params float[] values) => values.Any(InfiniteOrNaN);
        
        public static bool InfiniteZeroOrNaN(float value) {
            return float.IsInfinity(value) || float.IsNaN(value) || Math.Abs(value) < 0.000001f;
        }
        
        public static bool InfiniteZeroOrNaN(float v1, float v2) => InfiniteZeroOrNaN(v1) || InfiniteZeroOrNaN(v2);

        public static bool InfiniteZeroOrNaN(params float[] values) => values.Any(InfiniteZeroOrNaN);

        public static bool InfiniteNegativeOrNaN(float value) {
            return float.IsInfinity(value) || float.IsNaN(value) || value < 0;
        }
        
        public static bool InfiniteNegativeOrNaN(float v1, float v2) => InfiniteNegativeOrNaN(v1) || InfiniteNegativeOrNaN(v2);

        public static bool InfiniteNegativeOrNaN(params float[] values) => values.Any(InfiniteNegativeOrNaN);

        public static bool InfiniteZeroNegativeOrNaN(float value) {
            return float.IsInfinity(value) || float.IsNaN(value) || value <= 0;
        }
        
        public static bool InfiniteZeroNegativeOrNaN(float v1, float v2) => InfiniteZeroNegativeOrNaN(v1) || InfiniteZeroNegativeOrNaN(v2);

        public static bool InfiniteZeroNegativeOrNaN(params float[] values) => values.Any(InfiniteZeroNegativeOrNaN);

    }

}
