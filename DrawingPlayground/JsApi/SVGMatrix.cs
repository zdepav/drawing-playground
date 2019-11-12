#nullable enable

using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint;

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
                this.a * a + this.c * b,
                this.b * a + this.d * b,
                this.a * c + this.c * d,
                this.b * c + this.d * d,
                this.a * e + this.c * f + this.e,
                this.b * e + this.d * f + this.f
            );
        }

        private SVGMatrix mult(SVGMatrix m) {
            return new SVGMatrix(
                engine,
                a * m.a + c * m.b,
                b * m.a + d * m.b,
                a * m.c + c * m.d,
                b * m.c + d * m.d,
                a * m.e + c * m.f + e,
                b * m.e + d * m.f + f
            );
        }

        public SVGMatrix multiply(SVGMatrix? secondMatrix) {
            if (secondMatrix is null) {
                throw JsErrorUtils.JsErrorInvalidValue(engine, nameof(SVGMatrix), nameof(multiply), nameof(secondMatrix), null);
            }
            return mult(secondMatrix);
        }




        /*

        # Multiplication of another Matrix
        multiply: (m) ->
        @_mult m.a, m.b, m.c, m.d, m.e, m.f

            translate: (x, y) ->
        @_mult 1, 0, 0, 1, x, y

            scale: (s) ->
        @_mult s, 0, 0, s, 0, 0

        scaleNonUniform: (x, y) ->
        @_mult x, 0, 0, y, 0, 0

        rotate: (degrees) ->
        @_rotate degrees * PI_DEG

            rotateFromVector: (x, y) ->
        @_rotate Math.atan2 y, x

            _rotate: (radians) ->
        c = Math.cos radians
        s = Math.sin radians
        @_mult c, s, -s, c, 0, 0

        flipX: ->
        @_mult -1, 0, 0, 1, 0, 0

        flipY: ->
        @_mult 1, 0, 0, -1, 0, 0

        skewX: (degrees) ->
        @_mult 1, 0, Math.tan(degrees * PI_DEG), 1, 0, 0

        skewY: (degrees) ->
        @_mult 1, Math.tan(degrees * PI_DEG), 0, 1, 0, 0

        inverse: ->
        det = @a*@d - @c*@b
        return null if det == 0
    
        new JSMatrix(
            @d / det
        -@b / det
        -@c / det
            @a / det
            (@c*@f-@e*@d) / det
            (@e*@b-@a*@f) / det
        )
            */


        #if RENDER_BACKEND_SYSTEM_DRAWING


        #elif RENDER_BACKEND_SKIA
        #endif

    }

}
