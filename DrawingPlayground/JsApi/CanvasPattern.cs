#nullable enable

using Jint;

#if RENDER_BACKEND_SYSTEM_DRAWING
using System.Drawing;
using System.Drawing.Drawing2D;
#elif RENDER_BACKEND_SKIA
using SkiaSharp;
#endif

namespace DrawingPlayground.JsApi {

    public class CanvasPattern {

        private readonly Engine engine;

        #if RENDER_BACKEND_SYSTEM_DRAWING

        private readonly Image image;

        private readonly WrapMode wrapMode;

        private Matrix matrix;

        internal CanvasPattern(Engine engine, Image image, WrapMode wrapMode) {
            this.engine = engine;
            this.image = image;
            this.wrapMode = wrapMode;
            matrix = new Matrix();
        }

        public void setTransform(Matrix? matrix) {
            this.matrix = matrix ?? throw JsErrorUtils.JsErrorInvalidValue(engine, nameof(CanvasPattern), nameof(setTransform), nameof(matrix), null);
        }

        internal Brush MakeBrush() {
            return new TextureBrush(image, wrapMode) { Transform = matrix };
        }

        #elif RENDER_BACKEND_SKIA


        
        #endif

    }

}
