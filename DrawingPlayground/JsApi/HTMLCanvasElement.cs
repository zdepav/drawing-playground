#nullable enable

using System.Windows.Forms;
using Jint;

namespace DrawingPlayground.JsApi {

    public class HTMLCanvasElement {

        private readonly Engine engine;

        private readonly Control parentControl;

        private readonly CanvasRenderingContext2D renderingContext2D;

        internal HTMLCanvasElement(Engine engine, CanvasRenderingContext2D renderingContext2D, Control parentControl) {
            this.engine = engine;
            this.renderingContext2D = renderingContext2D;
            this.parentControl = parentControl;
        }
        
        /// <summary>Width of the canvas in pixels</summary>
        public int width => parentControl.Width;

        /// <summary>Height of the canvas in pixels</summary>
        public int height => parentControl.Height;

        /// <summary>
        /// Returns a drawing context on the canvas, or null if the context ID is not supported.
        /// A drawing context lets you draw on the canvas.
        /// Calling getContext with "2d" returns a CanvasRenderingContext2D object, whereas
        /// calling it with "webgl" (or "experimental-webgl") returns a WebGLRenderingContext object.
        /// This context is only available on browsers that implement WebGL.
        /// </summary>
        /// <param name="contextType">
        /// Is a DOMString containing the context identifier defining the drawing context associated to the canvas.<br/>
        /// Possible values are:
        /// <list type="table">
        /// <item>
        /// <term>"2d"</term>
        /// <description>leading to the creation of a CanvasRenderingContext2D object representing a two-dimensional rendering context.</description>
        /// </item>
        /// <item>
        /// <term>"webgl" or "experimental-webgl"</term>
        /// <description>which will create a WebGLRenderingContext object representing a three-dimensional rendering context. This context is only available on browsers that implement WebGL version 1 (OpenGL ES 2.0).</description>
        /// </item>
        /// <item>
        /// <term>"webgl2"</term>
        /// <description>which will create a WebGL2RenderingContext object representing a three-dimensional rendering context. This context is only available on browsers that implement WebGL version 2 (OpenGL ES 3.0). </description>
        /// </item>
        /// <item>
        /// <term>"bitmaprenderer"</term>
        /// <description>which will create an ImageBitmapRenderingContext which only provides functionality to replace the content of the canvas with a given ImageBitmap.</description>
        /// </item>
        /// </list>
        /// </param>
        public RenderingContext? getContext(string? contextType) {
            return contextType switch {
                "2d" => renderingContext2D,
                "webgl" => throw JsErrorUtils.Error(engine, "WebGL is not supported"),
                "webgl2" => throw JsErrorUtils.Error(engine, "WebGL is not supported"),
                "experimental-webgl" => throw JsErrorUtils.Error(engine, "WebGL is not supported"),
                "bitmaprenderer" => throw JsErrorUtils.Error(engine, "BitmapRenderer is not supported"),
                _ => throw JsErrorUtils.InvalidValue(engine, nameof(CanvasRenderingContext2D), nameof(getContext), nameof(contextType), contextType)
            };
        }
        
        /// <summary>
        /// Returns a drawing context on the canvas, or null if the context ID is not supported.
        /// A drawing context lets you draw on the canvas.
        /// Calling getContext with "2d" returns a CanvasRenderingContext2D object, whereas
        /// calling it with "webgl" (or "experimental-webgl") returns a WebGLRenderingContext object.
        /// This context is only available on browsers that implement WebGL.
        /// </summary>
        /// <param name="contextType">
        /// Is a DOMString containing the context identifier defining the drawing context associated to the canvas.<br/>
        /// Possible values are:
        /// <list type="table">
        /// <item>
        /// <term>"2d"</term>
        /// <description>leading to the creation of a CanvasRenderingContext2D object representing a two-dimensional rendering context.</description>
        /// </item>
        /// <item>
        /// <term>"webgl" or "experimental-webgl"</term>
        /// <description>which will create a WebGLRenderingContext object representing a three-dimensional rendering context. This context is only available on browsers that implement WebGL version 1 (OpenGL ES 2.0).</description>
        /// </item>
        /// <item>
        /// <term>"webgl2"</term>
        /// <description>which will create a WebGL2RenderingContext object representing a three-dimensional rendering context. This context is only available on browsers that implement WebGL version 2 (OpenGL ES 3.0). </description>
        /// </item>
        /// <item>
        /// <term>"bitmaprenderer"</term>
        /// <description>which will create an ImageBitmapRenderingContext which only provides functionality to replace the content of the canvas with a given ImageBitmap.</description>
        /// </item>
        /// </list>
        /// </param>
        public RenderingContext? getContext(string? contextType, object? contextAttributes) => getContext(contextType);
        
    }

}
