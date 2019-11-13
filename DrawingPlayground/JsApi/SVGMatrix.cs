#nullable enable

using System;
using Jint;

#if RENDER_BACKEND_SYSTEM_DRAWING
using System.Drawing.Drawing2D;
#elif RENDER_BACKEND_SKIA
using SkiaSharp;
#endif

namespace DrawingPlayground.JsApi {

    public class SVGMatrix {

        private readonly Engine engine;

        private float _a;
        public float a {
            get => _a;
            set {
                if (!float.IsInfinity(value) && !float.IsNaN(value)) {
                    _a = value;
                }
            }
        }

        private float _b;
        public float b {
            get => _b;
            set {
                if (!float.IsInfinity(value) && !float.IsNaN(value)) {
                    _b = value;
                }
            }
        }

        private float _c;
        public float c {
            get => _c;
            set {
                if (!float.IsInfinity(value) && !float.IsNaN(value)) {
                    _c = value;
                }
            }
        }

        private float _d;
        public float d {
            get => _d;
            set {
                if (!float.IsInfinity(value) && !float.IsNaN(value)) {
                    _d = value;
                }
            }
        }

        private float _e;
        public float e {
            get => _e;
            set {
                if (!float.IsInfinity(value) && !float.IsNaN(value)) {
                    _e = value;
                }
            }
        }

        private float _f;
        public float f {
            get => _f;
            set {
                if (!float.IsInfinity(value) && !float.IsNaN(value)) {
                    _f = value;
                }
            }
        }

        internal SVGMatrix(Engine engine) {
            this.engine = engine;
        }

        private SVGMatrix(Engine engine, float a, float b, float c, float d, float e, float f) {
            this.engine = engine;
            _a = a;
            _b = b;
            _c = c;
            _d = d;
            _e = e;
            _f = f;
        }

        private SVGMatrix mult(float a, float b, float c, float d, float e, float f) {
            return new SVGMatrix(
                engine,
                _a * a + _c * b,
                _b * a + _d * b,
                _a * c + _c * d,
                _b * c + _d * d,
                _a * e + _c * f + _e,
                _b * e + _d * f + _f
            );
        }

        private SVGMatrix mult(SVGMatrix m) {
            return new SVGMatrix(
                engine,
                _a * m._a + _c * m._b,
                _b * m._a + _d * m._b,
                _a * m._c + _c * m._d,
                _b * m._c + _d * m._d,
                _a * m._e + _c * m._f + _e,
                _b * m._e + _d * m._f + _f
            );
        }

        public SVGMatrix multiply(SVGMatrix? secondMatrix) {
            if (secondMatrix is null) {
                throw JsErrorUtils.InvalidValue(engine, nameof(SVGMatrix), nameof(multiply), nameof(secondMatrix), null);
            }
            return mult(secondMatrix);
        }

        public SVGMatrix translate(float x, float y) => mult(1, 0, 0, 1, x, y);

        public SVGMatrix scale(float s) => mult(s, 0, 0, s, 0, 0);

        public SVGMatrix scaleNonUniform(float x, float y) => mult(x, 0, 0, y, 0, 0);

        public SVGMatrix rotate(float degrees) {
            var rad = MathUtils.Deg2Rad(degrees);
            var c = (float)Math.Cos(rad);
            var s = (float)Math.Sin(rad);
            return mult(c, s, -s, c, 0, 0);
        }

        public SVGMatrix rotateFromVector(float x, float y) {
            var length = (float)Math.Sqrt(x * x + y * y);
            if (length < 0.000001) return this;
            x /= length;
            y /= length;
            return mult(x, y, -y, x, 0, 0);
        }

        public SVGMatrix flipX() => mult(-1, 0, 0, 1, 0, 0);

        public SVGMatrix flipY() => mult(1, 0, 0, -1, 0, 0);

        public SVGMatrix skewX(float degrees) => mult(1, 0, MathUtils.TanD(degrees), 1, 0, 0);

        public SVGMatrix skewY(float degrees) => mult(1, MathUtils.TanD(degrees), 0, 1, 0, 0);

        public SVGMatrix? inverse() {
            var det = _a * _d - _c * _b;
            if (det == 0) return null;
            return new SVGMatrix(
                engine,
                _d / det,
                -_b / det,
                -_c / det,
                _a / det,
                (_c * _f - _e * _d) / det,
                (_e * _b - _a * _f) / det
            );
        }


        #if RENDER_BACKEND_SYSTEM_DRAWING

        internal Matrix ToMatrix() => new Matrix(a, b, c, d, e, f);

        #elif RENDER_BACKEND_SKIA

        internal SKMatrix ToMatrix() => new SKMatrix { Values = new[] { a, c, e, b, d, f, 0f, 0f, 1f } };
            
        #endif

    }

}
