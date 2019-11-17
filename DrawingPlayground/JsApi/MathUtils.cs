using System;

namespace DrawingPlayground.JsApi {

    internal static class MathUtils {

        private const float
            DEGREES_TO_RADIANS_RATIO = (float)(180.0 / Math.PI),
            RADIANS_TO_DEGREES_RATIO = (float)(Math.PI / 180.0),
            RADIANS_FULL_CIRCLE = (float)(Math.PI * 2.0);

        public static float Deg2Rad(float degrees) => degrees * DEGREES_TO_RADIANS_RATIO;

        public static float Rad2Deg(float radians) => radians * RADIANS_TO_DEGREES_RATIO;

        public static float TanD(float degrees) => (float)Math.Tan(degrees * DEGREES_TO_RADIANS_RATIO);

        public static float WrapAngleDeg(float a) => a < 0 ? (360 - -a % 360) % 360 : a % 360;

        public static float WrapAngleRad(float a) =>
            a < 0
                ? (RADIANS_FULL_CIRCLE - -a % RADIANS_FULL_CIRCLE) % RADIANS_FULL_CIRCLE
                : a % RADIANS_FULL_CIRCLE;

    }

}
