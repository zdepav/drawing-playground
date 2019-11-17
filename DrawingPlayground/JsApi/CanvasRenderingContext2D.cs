#nullable enable

using System;
using System.Text;
using System.Text.RegularExpressions;
using Jint;
#if RENDER_BACKEND_SYSTEM_DRAWING
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

#elif RENDER_BACKEND_SKIA
using SkiaSharp;
#endif

namespace DrawingPlayground.JsApi {

    // https://developer.mozilla.org/en-US/docs/Web/API/CanvasRenderingContext2D

    public class CanvasRenderingContext2D : IDisposable {

        private readonly Engine engine;

        private Path2D path;

        public SVGMatrix createSVGMatrix() => new SVGMatrix(engine);

        #if RENDER_BACKEND_SYSTEM_DRAWING

        private readonly Graphics graphics;

        public CanvasRenderingContext2D(Engine engine, Graphics graphics) {
            this.engine = engine;
            this.graphics = graphics;
            _fillStyle = "#000";
            _fillStyleBrush = new SolidBrush(Color.Black);
            _strokeStyle = "#000";
            _lineWidth = 1f;
            _lineJoin = LineJoin.Miter;
            _lineDashOffset = 0f;
            _lineCap = LineCap.Flat;
            _miterLimit = 10f;
            _strokeStylePen = new Pen(Color.Black, _lineWidth) {
                LineJoin = _lineJoin,
                DashOffset = _lineDashOffset,
                StartCap = _lineCap,
                EndCap = _lineCap,
                MiterLimit = _miterLimit
            };
            _imageSmoothingEnabled = true;
            graphics.SmoothingMode = _smoothingMode = SmoothingMode.HighSpeed;
            _font = new Font(new FontFamily(GenericFontFamilies.SansSerif), 10, GraphicsUnit.Pixel);
            _stringFormat = new StringFormat();
            path = new Path2D(engine);
        }

        private void MakeNewPen(Color color) {
            _strokeStylePen.Dispose();
            _strokeStylePen = new Pen(color, _lineWidth) {
                LineJoin = _lineJoin,
                DashOffset = _lineDashOffset,
                StartCap = _lineCap,
                EndCap = _lineCap,
                MiterLimit = _miterLimit
            };
        }

        private void MakeNewPen(Brush brush) {
            _strokeStylePen.Dispose();
            _strokeStylePen = new Pen(brush, _lineWidth) {
                LineJoin = _lineJoin,
                DashOffset = _lineDashOffset,
                StartCap = _lineCap,
                EndCap = _lineCap,
                MiterLimit = _miterLimit
            };
        }

        private bool IsTextRightToLeft() {
            return _stringFormat.FormatFlags.HasFlag(StringFormatFlags.DirectionRightToLeft);
        }

        #region properties

        public object? canvas => null;

        private object _fillStyle;
        private Brush _fillStyleBrush;

        public object? fillStyle {
            get => _fillStyle;
            set {
                if (value is string s) {
                    if (ColorUtils.TryGetCssColor(s, out var color)) {
                        _fillStyleBrush.Dispose();
                        _fillStyleBrush = new SolidBrush(color);
                        _fillStyle = value;
                    }
                } else if (value is CanvasGradient g) {
                    _fillStyleBrush.Dispose();
                    _fillStyleBrush = g.MakeBrush();
                    _fillStyle = value;
                } else if (value is CanvasPattern p) {
                    _fillStyleBrush.Dispose();
                    _fillStyleBrush = p.MakeBrush();
                    _fillStyle = value;
                }
                throw JsErrorUtils.InvalidValue(engine, nameof(CanvasRenderingContext2D), nameof(fillStyle), value);
            }
        }

        private object _strokeStyle;
        private Pen _strokeStylePen;

        public object? strokeStyle {
            get => _strokeStyle;
            set {
                if (value is string s) {
                    if (ColorUtils.TryGetCssColor(s, out var color)) {
                        MakeNewPen(color);
                        _strokeStyle = value;
                        return;
                    }
                } else if (value is CanvasGradient g) {
                    MakeNewPen(g.MakeBrush());
                    _strokeStyle = value;
                    return;
                } else if (value is CanvasPattern p) {
                    MakeNewPen(p.MakeBrush());
                    _strokeStyle = value;
                    return;
                }
                throw JsErrorUtils.InvalidValue(engine, nameof(CanvasRenderingContext2D), nameof(strokeStyle), value);
            }
        }

        private float _lineWidth;

        public float lineWidth {
            get => _lineWidth;
            set {
                if (!float.IsInfinity(value) && !float.IsNaN(value) && value > 0) {
                    _lineWidth = value;
                    _strokeStylePen.Width = _lineWidth;
                }
            }
        }

        private LineJoin _lineJoin;

        public string? lineJoin {
            get => _lineJoin switch {
                LineJoin.Round => "round",
                LineJoin.Bevel => "bevel",
                _ => "miter"
            };
            set {
                _lineJoin = value switch {
                    "round" => LineJoin.Round,
                    "bevel" => LineJoin.Bevel,
                    "miter" => LineJoin.Miter,
                    _ => throw JsErrorUtils.InvalidValue(engine, nameof(CanvasRenderingContext2D), nameof(lineJoin), value)
                };
                _strokeStylePen.LineJoin = _lineJoin;
            }
        }

        private LineCap _lineCap;

        public string? lineCap {
            get => _lineCap switch {
                LineCap.Round => "round",
                LineCap.Square => "square",
                _ => "butt"
            };
            set {
                _lineCap = value switch {
                    "square" => LineCap.Square,
                    "round" => LineCap.Round,
                    "butt" => LineCap.Flat,
                    _ => throw JsErrorUtils.InvalidValue(engine, nameof(CanvasRenderingContext2D), nameof(lineCap), value)
                };
                _strokeStylePen.StartCap = _lineCap;
                _strokeStylePen.EndCap = _lineCap;
            }
        }

        private float _lineDashOffset;

        public float lineDashOffset {
            get => _lineDashOffset;
            set {
                if (!float.IsInfinity(value) && !float.IsNaN(value) && value > 0) {
                    _strokeStylePen.DashOffset = _lineDashOffset = value;
                }
            }
        }

        private float _miterLimit;

        public float miterLimit {
            get => _miterLimit;
            set {
                if (!float.IsInfinity(value) && !float.IsNaN(value) && value > 0) {
                    _strokeStylePen.MiterLimit = _miterLimit = value;
                }
            }
        }

        private bool _imageSmoothingEnabled;

        public bool imageSmoothingEnabled {
            get => _imageSmoothingEnabled;
            set {
                _imageSmoothingEnabled = value;
                if (value) {
                    graphics.SmoothingMode = _smoothingMode;
                }
            }
        }

        private SmoothingMode _smoothingMode;

        public string? imageSmoothingQuality {
            get => _smoothingMode switch {
                SmoothingMode.HighQuality => "high",
                SmoothingMode.AntiAlias => "medium",
                _ => "low"
            };
            set {
                _smoothingMode = value switch {
                    "high" => SmoothingMode.HighQuality,
                    "medium" => SmoothingMode.AntiAlias,
                    "low" => SmoothingMode.HighSpeed,
                    _ => throw JsErrorUtils.InvalidValue(engine, nameof(CanvasRenderingContext2D), nameof(imageSmoothingQuality), value)
                };
                graphics.SmoothingMode = _smoothingMode;
            }
        }

        private static readonly Regex fontRegex = new Regex(@"(?<B>bold\s+)(?<I>italic\s+)(?<Size>\d+(?:\.\d+)?)(?<Unit>pt|px|em|%)\s+(?<Family>[-A-Za-z0-9_]+|""[-A-Za-z0-9_ ]+"")");
        private Font _font;

        public string? font {
            get {
                var sb = new StringBuilder();
                if (_font.Bold) sb.Append("bold ");
                if (_font.Italic) sb.Append("italic ");
                sb.Append($"{_font.SizeInPoints}pt ");
                sb.Append(_font.Name);
                return sb.ToString();
            }
            set {
                if (value == null) {
                    throw JsErrorUtils.InvalidValue(engine, nameof(CanvasRenderingContext2D), nameof(font), null);
                }
                var m = fontRegex.Match(value);
                if (m.Success) {
                    var b = m.Groups["B"].Success;
                    var i = m.Groups["I"].Success;
                    var fontStyle = b ? FontStyle.Bold : i ? FontStyle.Italic : FontStyle.Regular;
                    if (b && i) {
                        fontStyle |= FontStyle.Italic;
                    }
                    var size = float.Parse(m.Groups["Size"].Value);
                    var unit = m.Groups["Unit"].Value switch {
                        "pt" => GraphicsUnit.Point,
                        "px" => GraphicsUnit.Pixel,
                        _ => throw JsErrorUtils.Error(engine, $"Size unit '{m.Groups["Unit"].Value}' is not supported for {nameof(CanvasRenderingContext2D)}.{nameof(font)}")
                    };
                    var name = m.Groups["Family"].Value;
                    if (name[0] == '"') {
                        name = name.Substring(1, name.Length - 2);
                    }
                    var family = name.ToLower() switch {
                        "serif" => new FontFamily(GenericFontFamilies.Serif),
                        "sans-serif" => new FontFamily(GenericFontFamilies.SansSerif),
                        "monospace" => new FontFamily(GenericFontFamilies.Monospace),
                        _ => new FontFamily(name)
                    };
                    _font = new Font(family, size, fontStyle, unit);
                }
                throw JsErrorUtils.InvalidValue(engine, nameof(CanvasRenderingContext2D), nameof(font), value);
            }
        }

        public float globalAlpha {
            get => throw JsErrorUtils.NotSupported(engine, nameof(CanvasRenderingContext2D), nameof(globalAlpha));
            set => throw JsErrorUtils.NotSupported(engine, nameof(CanvasRenderingContext2D), nameof(globalAlpha));
        }

        public string? globalCompositeOperation {
            get => throw JsErrorUtils.NotSupported(engine, nameof(CanvasRenderingContext2D), nameof(globalCompositeOperation));
            set => throw JsErrorUtils.NotSupported(engine, nameof(CanvasRenderingContext2D), nameof(globalCompositeOperation));
        }

        public object? currentTransform {
            get => throw JsErrorUtils.NotSupported(engine, nameof(CanvasRenderingContext2D), nameof(currentTransform));
            set => throw JsErrorUtils.NotSupported(engine, nameof(CanvasRenderingContext2D), nameof(currentTransform));
        }

        public string? direction {
            get => IsTextRightToLeft() ? "rtl" : "ltr";
            set {
                switch (value) {
                    case "rtl":
                        if (!IsTextRightToLeft()) {
                            _stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
                            if (TextAlignLeftRight) {
                                _stringFormat.Alignment = _stringFormat.Alignment == StringAlignment.Far
                                    ? StringAlignment.Near
                                    : StringAlignment.Far;
                            }
                        }
                        return;
                    case "ltr":
                    case "inherit":
                        if (IsTextRightToLeft()) {
                            _stringFormat.FormatFlags ^= StringFormatFlags.DirectionRightToLeft;
                            if (TextAlignLeftRight) {
                                _stringFormat.Alignment = _stringFormat.Alignment == StringAlignment.Far
                                    ? StringAlignment.Near
                                    : StringAlignment.Far;
                            }
                        }
                        return;
                    default:
                        throw JsErrorUtils.InvalidValue(engine, nameof(CanvasRenderingContext2D), nameof(direction), value);
                }
            }
        }

        public string? filter {
            get => throw JsErrorUtils.NotSupported(engine, nameof(CanvasRenderingContext2D), nameof(filter));
            set => throw JsErrorUtils.NotSupported(engine, nameof(CanvasRenderingContext2D), nameof(filter));
        }

        public float shadowBlur {
            get => throw JsErrorUtils.NotSupported(engine, nameof(CanvasRenderingContext2D), nameof(shadowBlur));
            set => throw JsErrorUtils.NotSupported(engine, nameof(CanvasRenderingContext2D), nameof(shadowBlur));
        }

        public string? shadowColor {
            get => throw JsErrorUtils.NotSupported(engine, nameof(CanvasRenderingContext2D), nameof(shadowColor));
            set => throw JsErrorUtils.NotSupported(engine, nameof(CanvasRenderingContext2D), nameof(shadowColor));
        }

        public float shadowOffsetX {
            get => throw JsErrorUtils.NotSupported(engine, nameof(CanvasRenderingContext2D), nameof(shadowOffsetX));
            set => throw JsErrorUtils.NotSupported(engine, nameof(CanvasRenderingContext2D), nameof(shadowOffsetX));
        }

        public float shadowOffsetY {
            get => throw JsErrorUtils.NotSupported(engine, nameof(CanvasRenderingContext2D), nameof(shadowOffsetY));
            set => throw JsErrorUtils.NotSupported(engine, nameof(CanvasRenderingContext2D), nameof(shadowOffsetY));
        }

        private readonly StringFormat _stringFormat;
        private bool TextAlignLeftRight;

        public string? textAlign {
            get {
                switch (_stringFormat.Alignment) {
                    case StringAlignment.Far:
                        if (TextAlignLeftRight) {
                            return IsTextRightToLeft() ? "left" : "right";
                        } else return "end";
                    case StringAlignment.Near:
                        if (TextAlignLeftRight) {
                            return IsTextRightToLeft() ? "right" : "left";
                        } else return "start";
                    default:
                        return "center";
                }
            }
            set {
                switch (value) {
                    case "center":
                        _stringFormat.Alignment = StringAlignment.Center;
                        TextAlignLeftRight = false;
                        return;
                    case "start":
                        _stringFormat.Alignment = StringAlignment.Near;
                        TextAlignLeftRight = false;
                        return;
                    case "end":
                        _stringFormat.Alignment = StringAlignment.Far;
                        TextAlignLeftRight = false;
                        return;
                    case "left":
                        _stringFormat.Alignment = IsTextRightToLeft()
                            ? StringAlignment.Far
                            : StringAlignment.Near;
                        TextAlignLeftRight = true;
                        return;
                    case "right":
                        _stringFormat.Alignment = IsTextRightToLeft()
                            ? StringAlignment.Near
                            : StringAlignment.Far;
                        TextAlignLeftRight = true;
                        return;
                    default:
                        throw JsErrorUtils.InvalidValue(engine, nameof(CanvasRenderingContext2D), nameof(textAlign), value);
                }
            }
        }

        public float textBaseline {
            get => throw JsErrorUtils.NotSupported(engine, nameof(CanvasRenderingContext2D), nameof(textBaseline));
            set => throw JsErrorUtils.NotSupported(engine, nameof(CanvasRenderingContext2D), nameof(textBaseline));
        }

        #endregion

        #region methods

        public CanvasGradient createLinearGradient(float x0, float y0, float x1, float y1) {
            return new CanvasGradient(engine, x0, y0, x1, y1);
        }

        public CanvasGradient createRadialGradient(float x0, float y0, float r0, float x1, float y1, float r1) {
            throw JsErrorUtils.NotSupported(engine, nameof(CanvasRenderingContext2D), nameof(createRadialGradient));
        }

        public CanvasPattern createPattern(Image? image, string? repetition) {
            if (image is null) {
                throw JsErrorUtils.InvalidValue(engine, nameof(CanvasRenderingContext2D), nameof(createPattern), nameof(image), null);
            }
            var wrap = repetition switch {
                null => WrapMode.Tile,
                "" => WrapMode.Tile,
                "repeat" => WrapMode.Tile,
                "repeat-x" => WrapMode.Tile,
                "repeat-y" => WrapMode.Tile,
                "no-repeat" => WrapMode.Clamp,
                _ => throw JsErrorUtils.InvalidValue(engine, nameof(CanvasRenderingContext2D), nameof(createPattern), nameof(repetition), repetition)
            };
            return new CanvasPattern(engine, image, wrap);
        }

        public void fillRect(float x, float y, float width, float height) {
            graphics.FillRectangle(_fillStyleBrush, x, y, width, height);
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
            path.ellipse(x, y, radiusX, radiusY, rotation, startAngle, endAngle, anticlockwise);
        }

        public void ellipse(float x, float y, float radiusX, float radiusY, float rotation, float startAngle, float endAngle) {
            path.ellipse(x, y, radiusX, radiusY, rotation, startAngle, endAngle, false);
        }

        #endregion

        public void Dispose() {
            _fillStyleBrush.Dispose();
            _strokeStylePen.Dispose();

        }

        #elif RENDER_BACKEND_SKIA
        private readonly SKCanvas canv;

        public CanvasRenderingContext2D(Engine engine, SKCanvas canv) {
            this.engine = engine;
            this.canv = canv;
            _fillStylePaint = new SKPaint { Style = SKPaintStyle.Fill };
            _strokeStylePaint = new SKPaint { Style = SKPaintStyle.Stroke };
        }
        
        #region properties

        public object? canvas => null;

        private object _fillStyle;
        private readonly SKPaint _fillStylePaint;
        public object? fillStyle {
            get => _fillStyle;
            set {
                if (value is string s) {
                    if (ColorUtils.TryGetCssColor(s, out var color)) {
                        _fillStylePaint.Color = color;
                        _fillStyle = value;
                        return;
                    }
                } else if (value is CanvasGradient g) {
                    _fillStylePaint.Color = SKColors.White;
                    _fillStylePaint.Shader = g.GetShader();
                    _fillStyle = value;
                    return;
                }
                throw JsErrorUtils.JsErrorInvalidValue(engine, nameof(CanvasRenderingContext2D), nameof(fillStyle), value);
            }
        }

        private object _strokeStyle;
        private readonly SKPaint _strokeStylePaint;
        public object? strokeStyle {
            get => _strokeStyle;
            set {
                if (value is string s) {
                    if (ColorUtils.TryGetCssColor(s, out var color)) {
                        MakeNewPen(color);
                        _strokeStyle = value;
                        return;
                    }
                } else if (value is CanvasGradient g) {
                    MakeNewPen(g.MakeBrush());
                    _strokeStyle = value;
                    return;
                }
                throw JsErrorUtils.JsErrorInvalidValue(engine, nameof(CanvasRenderingContext2D), nameof(strokeStyle), value);
            }
        }

        private float _lineWidth;

        public float lineWidth {
            get => _lineWidth;
            set {
                if (!float.IsInfinity(value) && !float.IsNaN(value) && value > 0) {
                    _lineWidth = value;
                    _strokeStylePen.Width = _lineWidth;
                }
            }
        }

        private LineJoin _lineJoin;

        public string? lineJoin {
            get => _lineJoin switch {
                LineJoin.Round => "round",
                LineJoin.Bevel => "bevel",
                _ => "miter"
            };
            set {
                _lineJoin = value switch {
                    "round" => LineJoin.Round,
                    "bevel" => LineJoin.Bevel,
                    "miter" => LineJoin.Miter,
                    _ => throw JsErrorUtils.JsErrorInvalidValue(engine, nameof(CanvasRenderingContext2D), nameof(lineJoin), value)
                };
                _strokeStylePen.LineJoin = _lineJoin;
            }
        }

        private LineCap _lineCap;

        public string? lineCap {
            get => _lineCap switch {
                LineCap.Round => "round",
                LineCap.Square => "square",
                _ => "butt"
            };
            set {
                _lineCap = value switch {
                    "square" => LineCap.Square,
                    "round" => LineCap.Round,
                    "butt" => LineCap.Flat,
                    _ => throw JsErrorUtils.JsErrorInvalidValue(engine, nameof(CanvasRenderingContext2D), nameof(lineCap), value)
                };
                _strokeStylePen.StartCap = _lineCap;
                _strokeStylePen.EndCap = _lineCap;
            }
        }

        private float _lineDashOffset;

        public float lineDashOffset {
            get => _lineDashOffset;
            set {
                if (!float.IsInfinity(value) && !float.IsNaN(value) && value > 0) {
                    _strokeStylePen.DashOffset = _lineDashOffset = value;
                }
            }
        }

        private float _miterLimit;

        public float miterLimit {
            get => _miterLimit;
            set {
                if (!float.IsInfinity(value) && !float.IsNaN(value) && value > 0) {
                    _strokeStylePen.MiterLimit = _miterLimit = value;
                }
            }
        }

        private bool _imageSmoothingEnabled;

        public bool imageSmoothingEnabled {
            get => _imageSmoothingEnabled;
            set {
                _imageSmoothingEnabled = value;
                if (value) {
                    graphics.SmoothingMode = _smoothingMode;
                }
            }
        }

        private SmoothingMode _smoothingMode;

        public string? imageSmoothingQuality {
            get => _smoothingMode switch {
                SmoothingMode.HighQuality => "high",
                SmoothingMode.AntiAlias => "medium",
                _ => "low"
            };
            set {
                _smoothingMode = value switch {
                    "high" => SmoothingMode.HighQuality,
                    "medium" => SmoothingMode.AntiAlias,
                    "low" => SmoothingMode.HighSpeed,
                    _ => throw JsErrorUtils.JsErrorInvalidValue(engine, nameof(CanvasRenderingContext2D), nameof(imageSmoothingQuality), value)
                };
                graphics.SmoothingMode = _smoothingMode;
            }
        }

        private static readonly Regex fontRegex = new Regex(@"(?<B>bold\s+)(?<I>italic\s+)(?<Size>\d+(?:\.\d+)?)(?<Unit>pt|px|em|%)\s+(?<Family>[-A-Za-z0-9_]+|""[-A-Za-z0-9_ ]+"")");
        private Font _font;

        public string? font {
            get {
                var sb = new StringBuilder();
                if (_font.Bold) sb.Append("bold ");
                if (_font.Italic) sb.Append("italic ");
                sb.Append($"{_font.SizeInPoints}pt ");
                sb.Append(_font.Name);
                return sb.ToString();
            }
            set {
                if (value == null) {
                    throw JsErrorUtils.JsErrorInvalidValue(engine, nameof(CanvasRenderingContext2D), nameof(font), null);
                }
                var m = fontRegex.Match(value);
                if (m.Success) {
                    var b = m.Groups["B"].Success;
                    var i = m.Groups["I"].Success;
                    var fontStyle = b ? FontStyle.Bold : i ? FontStyle.Italic : FontStyle.Regular;
                    if (b && i) {
                        fontStyle |= FontStyle.Italic;
                    }
                    var size = float.Parse(m.Groups["Size"].Value);
                    var unit = m.Groups["Unit"].Value switch {
                        "pt" => GraphicsUnit.Point,
                        "px" => GraphicsUnit.Pixel,
                        _ => throw JsError(engine, $"Size unit '{m.Groups["Unit"].Value}' is not supported for CanvasRenderingContext2D.font")
                    };
                    var name = m.Groups["Family"].Value;
                    if (name[0] == '"') {
                        name = name.Substring(1, name.Length - 2);
                    }
                    var family = name.ToLower() switch {
                        "serif" => new FontFamily(GenericFontFamilies.Serif),
                        "sans-serif" => new FontFamily(GenericFontFamilies.SansSerif),
                        "monospace" => new FontFamily(GenericFontFamilies.Monospace),
                        _ => new FontFamily(name)
                    };
                    _font = new Font(family, size, fontStyle, unit);
                }
                throw JsErrorUtils.JsErrorInvalidValue(engine, nameof(CanvasRenderingContext2D), nameof(font), value);
            }
        }

        public float globalAlpha {
            get => throw JsErrorUtils.JsErrorNotSupported(engine, nameof(CanvasRenderingContext2D), nameof(globalAlpha));
            set => throw JsErrorUtils.JsErrorNotSupported(engine, nameof(CanvasRenderingContext2D), nameof(globalAlpha));
        }

        public string? globalCompositeOperation {
            get => throw JsErrorUtils.JsErrorNotSupported(engine, nameof(CanvasRenderingContext2D), nameof(globalCompositeOperation));
            set => throw JsErrorUtils.JsErrorNotSupported(engine, nameof(CanvasRenderingContext2D), nameof(globalCompositeOperation));
        }

        public object? currentTransform {
            get => throw JsErrorUtils.JsErrorNotSupported(engine, nameof(CanvasRenderingContext2D), nameof(currentTransform));
            set => throw JsErrorUtils.JsErrorNotSupported(engine, nameof(CanvasRenderingContext2D), nameof(currentTransform));
        }

        public string? direction {
            get => IsTextRightToLeft() ? "rtl" : "ltr";
            set {
                switch (value) {
                    case "rtl":
                        if (!IsTextRightToLeft()) {
                            _stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
                            if (TextAlignLeftRight) {
                                _stringFormat.Alignment = _stringFormat.Alignment == StringAlignment.Far
                                    ? StringAlignment.Near
                                    : StringAlignment.Far;
                            }
                        }
                        return;
                    case "ltr":
                    case "inherit":
                        if (IsTextRightToLeft()) {
                            _stringFormat.FormatFlags ^= StringFormatFlags.DirectionRightToLeft;
                            if (TextAlignLeftRight) {
                                _stringFormat.Alignment = _stringFormat.Alignment == StringAlignment.Far
                                    ? StringAlignment.Near
                                    : StringAlignment.Far;
                            }
                        }
                        return;
                    default:
                        throw JsErrorUtils.JsErrorInvalidValue(engine, nameof(CanvasRenderingContext2D), nameof(direction), value);
                }
            }
        }

        public string? filter {
            get => throw JsErrorUtils.JsErrorNotSupported(engine, nameof(CanvasRenderingContext2D), nameof(filter));
            set => throw JsErrorUtils.JsErrorNotSupported(engine, nameof(CanvasRenderingContext2D), nameof(filter));
        }

        public float shadowBlur {
            get => throw JsErrorUtils.JsErrorNotSupported(engine, nameof(CanvasRenderingContext2D), nameof(shadowBlur));
            set => throw JsErrorUtils.JsErrorNotSupported(engine, nameof(CanvasRenderingContext2D), nameof(shadowBlur));
        }

        public string? shadowColor {
            get => throw JsErrorUtils.JsErrorNotSupported(engine, nameof(CanvasRenderingContext2D), nameof(shadowColor));
            set => throw JsErrorUtils.JsErrorNotSupported(engine, nameof(CanvasRenderingContext2D), nameof(shadowColor));
        }

        public float shadowOffsetX {
            get => throw JsErrorUtils.JsErrorNotSupported(engine, nameof(CanvasRenderingContext2D), nameof(shadowOffsetX));
            set => throw JsErrorUtils.JsErrorNotSupported(engine, nameof(CanvasRenderingContext2D), nameof(shadowOffsetX));
        }

        public float shadowOffsetY {
            get => throw JsErrorUtils.JsErrorNotSupported(engine, nameof(CanvasRenderingContext2D), nameof(shadowOffsetY));
            set => throw JsErrorUtils.JsErrorNotSupported(engine, nameof(CanvasRenderingContext2D), nameof(shadowOffsetY));
        }

        private readonly StringFormat _stringFormat;
        private bool TextAlignLeftRight;
        public string? textAlign {
            get {
                switch (_stringFormat.Alignment) {
                    case StringAlignment.Far:
                        if (TextAlignLeftRight) {
                            return IsTextRightToLeft() ? "left" : "right";
                        } else return "end";
                    case StringAlignment.Near:
                        if (TextAlignLeftRight) {
                            return IsTextRightToLeft() ? "right" : "left";
                        } else return "start";
                    default:
                        return "center";
                }
            }
            set {
                switch (value) {
                    case "center":
                        _stringFormat.Alignment = StringAlignment.Center;
                        TextAlignLeftRight = false;
                        return;
                    case "start":
                        _stringFormat.Alignment = StringAlignment.Near;
                        TextAlignLeftRight = false;
                        return;
                    case "end":
                        _stringFormat.Alignment = StringAlignment.Far;
                        TextAlignLeftRight = false;
                        return;
                    case "left":
                        _stringFormat.Alignment = IsTextRightToLeft()
                            ? StringAlignment.Far
                            : StringAlignment.Near;
                        TextAlignLeftRight = true;
                        return;
                    case "right":
                        _stringFormat.Alignment = IsTextRightToLeft()
                            ? StringAlignment.Near
                            : StringAlignment.Far;
                        TextAlignLeftRight = true;
                        return;
                    default:
                        throw JsErrorUtils.JsErrorInvalidValue(engine, nameof(CanvasRenderingContext2D), nameof(textAlign), value);
                }
            }
        }

        public float textBaseline {
            get => throw JsErrorUtils.JsErrorNotSupported(engine, nameof(CanvasRenderingContext2D), nameof(textBaseline));
            set => throw JsErrorUtils.JsErrorNotSupported(engine, nameof(CanvasRenderingContext2D), nameof(textBaseline));
        }

        #endregion

        #region methods

        public CanvasGradient createLinearGradient(float x0, float y0, float x1, float y1) {
            return new CanvasGradient(engine, x0, y0, x1, y1);
        }

        public CanvasGradient createRadialGradient(float x0, float y0, float r0, float x1, float y1, float r1) {
            return new CanvasGradient(engine, x0, y0, r0, x1, y1, r1);
        }

        public object createPattern(object? image, string? repetition) {
            throw JsErrorUtils.JsErrorNotSupported(engine, nameof(CanvasRenderingContext2D), nameof(createPattern));
        }

        #endregion

        public void Dispose() {
            _fillStylePaint.Dispose();
            _strokeStylePaint.Dispose();
        }

        #endif

    }

}
