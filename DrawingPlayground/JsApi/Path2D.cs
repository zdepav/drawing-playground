#nullable enable
using Jint;

#if RENDER_BACKEND_SYSTEM_DRAWING
using System.Drawing.Drawing2D;
#elif RENDER_BACKEND_SKIA
using SkiaSharp;
#endif

namespace DrawingPlayground.JsApi {

    public class Path2D {

        private readonly Engine engine;

        #if RENDER_BACKEND_SYSTEM_DRAWING

        internal GraphicsPath Path { get; }

        internal Path2D(Engine engine) {
            this.engine = engine;
            Path = new GraphicsPath();
        }

        public Path2D(Path2D? path) {
            if (path is null) {
                throw JsErrorUtils.InvalidValue(engine, nameof(Path2D), nameof(Path2D), nameof(path), null);
            }
            engine = path.engine;
            Path = (GraphicsPath)path.Path.Clone();
        }

        private void addPath(GraphicsPath gp, Matrix transform) {
            gp.Flatten(transform);
            Path.AddPath(gp, false);
        }

        public void addPath(Path2D? path, Matrix? transform) {
            if (path is null) {
                throw JsErrorUtils.InvalidValue(engine, nameof(Path2D), nameof(Path2D), nameof(path), null);
            }
            if (transform is null) {
                // TODO: check if correct
                throw JsErrorUtils.InvalidValue(engine, nameof(Path2D), nameof(Path2D), nameof(transform), null);
            }
            var gp = (GraphicsPath)path.Path.Clone();
            gp.Flatten(transform);
            Path.AddPath(gp, false);
        }

        public void ellipse(
            float x,
            float y,
            float radiusX,
            float radiusY,
            float rotation,
            float startAngle,
            float endAngle,
            bool anticlockwise
        ) {
            var gp = new GraphicsPath();
            if (anticlockwise) {
                gp.AddArc(
                    x, y,
                    radiusX, radiusY,
                    -MathUtils.Deg2Rad(startAngle),
                    -MathUtils.Deg2Rad(endAngle - startAngle)
                );
            } else {
                gp.AddArc(
                    x, y,
                    radiusX, radiusY,
                    MathUtils.Deg2Rad(startAngle),
                    MathUtils.Deg2Rad(endAngle - startAngle)
                );
            }
            var matrix = new Matrix();
            matrix.Rotate(rotation);
            addPath(gp, matrix);
        }

        #elif RENDER_BACKEND_SKIA

        
        #endif

    }

}
