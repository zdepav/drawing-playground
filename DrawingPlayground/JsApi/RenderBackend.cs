#nullable enable

namespace DrawingPlayground.JsApi {

    public static class RenderBackend {

        #if RENDER_BACKEND_SYSTEM_DRAWING

        public static string Name => "System.Drawing"; 

        #elif RENDER_BACKEND_SKIA

        public static string Name => "SkiaSharp";

        #else
        #error
        #endif

    }
}
