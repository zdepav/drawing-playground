#nullable enable
using System.Collections.Generic;
using Jint;

#if RENDER_BACKEND_SYSTEM_DRAWING
using System.Drawing;
using System.Drawing.Drawing2D;
#elif RENDER_BACKEND_SKIA
using System.Security.Cryptography;
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

        public void addPath(Path2D? path, Matrix? transform) {
            if (path is null) {
                throw JsErrorUtils.InvalidValue(engine, nameof(Path2D), nameof(Path2D), nameof(path), null);
            }
            if (transform is null) {
                throw JsErrorUtils.InvalidValue(engine, nameof(Path2D), nameof(Path2D), nameof(transform), null);
            }
            var gp = (GraphicsPath)path.Path.Clone();
            gp.Flatten(transform);
            Path.AddPath(gp, false);
        }

        #elif RENDER_BACKEND_SKIA

        
        #endif

    }

}
