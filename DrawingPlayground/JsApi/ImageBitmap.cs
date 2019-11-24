#if RENDER_BACKEND_SYSTEM_DRAWING
using System.Drawing;
#elif RENDER_BACKEND_SKIA
using SkiaSharp;
#endif

namespace DrawingPlayground.JsApi {

    public class ImageBitmap {
        
        #if RENDER_BACKEND_SYSTEM_DRAWING

        public Bitmap Bitmap { get; internal set; }

        #elif RENDER_BACKEND_SKIA

        #endif
    }

}
