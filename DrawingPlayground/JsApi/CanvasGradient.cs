#nullable enable
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using Jint;
using Jint.Runtime;

namespace DrawingPlayground.JsApi {

    public class CanvasGradient {

        private readonly Engine engine;

        private readonly float x0, y0, x1, y1;

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
                firstColor = colorStops[0].color;
                lastColor = colorStops[colorStops.Count - 1].color;
            }
            colorStops.Sort((stop1, stop2) => stop1.offset.CompareTo(stop2.offset));
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
            if (color is null) {
                throw new JavaScriptException(engine.Error, "Color can't be null", new ArgumentNullException(nameof(color)));
            }
            if (offset < 0f || offset > 1f) {
                throw new JavaScriptException(engine.Error, "Offset must be between 0 and 1", new ArgumentOutOfRangeException(nameof(offset)));
            }
            if (ColorUtils.TryGetCssColor(color, out var col)) {
                colorStops.Add((offset, col));
            } else {
                throw new JavaScriptException(engine.Error, $"'{color}' is not a valid color");
            }
        }

    }

}
