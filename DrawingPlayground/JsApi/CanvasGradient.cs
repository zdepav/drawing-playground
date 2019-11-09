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

    public class CanvasGradient {

        private readonly Engine engine;

        private readonly float x0, y0, x1, y1;

        #if RENDER_BACKEND_SYSTEM_DRAWING

        private readonly List<(float offset, Color color)> colorStops;

        internal CanvasGradient(Engine engine, float x0, float y0, float x1, float y1) {
            colorStops = new List<(float offset, Color color)>();
            this.engine = engine;
            this.x0 = x0;
            this.y0 = y0;
            this.x1 = x1;
            this.y1 = y1;
        }

        internal Brush MakeBrush() {
            var firstColor = Color.Black;
            var lastColor = Color.Black;
            if (colorStops.Count > 0) {
                colorStops.Sort((stop1, stop2) => stop1.offset.CompareTo(stop2.offset));
                firstColor = colorStops[0].color;
                lastColor = colorStops[colorStops.Count - 1].color;
            }
            var b = new LinearGradientBrush(new PointF(x0, y0), new PointF(x1, y1), firstColor, lastColor) {
                WrapMode = WrapMode.Clamp
            };
            var colorBlend = new ColorBlend(colorStops.Count);
            for (var i = 0; i < colorStops.Count; ++i) {
                colorBlend.Positions[i] = colorStops[i].offset;
                colorBlend.Colors[i] = colorStops[i].color;
            }
            b.InterpolationColors = colorBlend;
            return b;
        }
        
        public void addColorStop(float offset, string? color) {
            if (offset < 0f || offset > 1f) {
                throw JsErrorUtils.JsErrorInvalidValue(engine, nameof(CanvasGradient), nameof(addColorStop), nameof(offset), offset);
            }
            if (color is null) {
                throw JsErrorUtils.JsErrorInvalidValue(engine, nameof(CanvasGradient), nameof(addColorStop), nameof(color), null);
            } else if (ColorUtils.TryGetCssColor(color, out var col)) {
                colorStops.Add((offset, col));
            } else {
                throw JsErrorUtils.JsErrorInvalidValue(engine, nameof(CanvasGradient), nameof(addColorStop), nameof(color), color);
            }
        }

        #elif RENDER_BACKEND_SKIA

        private readonly float r0, r1;

        private readonly bool isRadial;
        
        private readonly List<(float offset, SKColor color)> colorStops;

        internal CanvasGradient(Engine engine, float x0, float y0, float x1, float y1) {
            colorStops = new List<(float offset, SKColor color)>();
            this.engine = engine;
            this.x0 = x0;
            this.y0 = y0;
            this.x1 = x1;
            this.y1 = y1;
            r0 = 0;
            r1 = 0;
            isRadial = false;
        }

        internal CanvasGradient(Engine engine, float x0, float y0, float r0, float x1, float y1, float r1) {
            colorStops = new List<(float offset, SKColor color)>();
            this.engine = engine;
            this.x0 = x0;
            this.y0 = y0;
            this.r0 = r0;
            this.x1 = x1;
            this.y1 = y1;
            this.r1 = r1;
            isRadial = true;
        }

        public SKShader GetShader() {
            colorStops.Sort((stop1, stop2) => stop1.offset.CompareTo(stop2.offset));
            var colors = new SKColor[colorStops.Count];
            var positions = new float[colorStops.Count];
            for (var i = 0; i < colorStops.Count; ++i) {
                (positions[i], colors[i]) = colorStops[i];
            }
            if (isRadial) {
                return  SKShader.CreateTwoPointConicalGradient(
                    new SKPoint(x0, y0),
                    r0,
                    new SKPoint(x1, y1),
                    r1,
                    colors,
                    positions,
                    SKShaderTileMode.Clamp
                );
            } else {
                return  SKShader.CreateLinearGradient(
                    new SKPoint(x0, y0),
                    new SKPoint(x1, y1),
                    colors,
                    positions,
                    SKShaderTileMode.Clamp
                );
            }
        }
        
        public void addColorStop(float offset, string? color) {
            if (offset < 0f || offset > 1f) {
                throw JsErrorUtils.JsErrorInvalidValue(engine, nameof(CanvasGradient), nameof(addColorStop), nameof(offset), offset);
            }
            if (color is null) {
                throw JsErrorUtils.JsErrorInvalidValue(engine, nameof(CanvasGradient), nameof(addColorStop), nameof(color), null);
            } else if (ColorUtils.TryGetCssColor(color, out var col)) {
                colorStops.Add((offset, col));
            } else {
                throw JsErrorUtils.JsErrorInvalidValue(engine, nameof(CanvasGradient), nameof(addColorStop), nameof(color), color);
            }
        }
        
        #endif

    }

}
