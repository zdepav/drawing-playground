using System;

namespace DrawingPlayground.JsApi {

    public class MathUtils {

        private const float
            DEGREES_TO_RADIANS_RATIO = (float)(180.0 / Math.PI),
            RADIANS_TO_DEGREES_RATIO = (float)(Math.PI / 180.0);

        public static float Deg2Rad(float degrees) => degrees * DEGREES_TO_RADIANS_RATIO;

        public static float Rad2Deg(float radians) => radians * RADIANS_TO_DEGREES_RATIO;

        internal static float TanD(float degrees) => (float)Math.Tan(degrees * DEGREES_TO_RADIANS_RATIO);
    }

}
