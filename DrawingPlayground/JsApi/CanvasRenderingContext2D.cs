#nullable enable
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Text;
using System.Text.RegularExpressions;
using Jint;
using Jint.Runtime;

namespace DrawingPlayground.JsApi {

    // https://developer.mozilla.org/en-US/docs/Web/API/CanvasRenderingContext2D

    public class CanvasRenderingContext2D : IDisposable {

        private readonly Engine engine;

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

        private JavaScriptException JsError(string message) {
            return new JavaScriptException(engine.Error, message);
        }

        private JavaScriptException JsErrorNotSupported(string memberName) {
            return new JavaScriptException(engine.Error, $"CanvasRenderingContext2D.{memberName} is not supported");
        }

        private JavaScriptException JsErrorInvalidValue(string propertyName, object? value) {
            return new JavaScriptException(
                engine.Error,
                $"{(value == null ? "null" : $"'{value}'")} is not a valid value for CanvasRenderingContext2D.{propertyName}"
            );
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
                        return;
                    }
                } else if (value is CanvasGradient g) {
                    _fillStyleBrush.Dispose();
                    _fillStyleBrush = g.MakeBrush();
                    _fillStyle = value;
                    return;
                }
                throw JsErrorInvalidValue("fillStyle", value);
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
                }
                throw JsErrorInvalidValue("strokeStyle", value);
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
                    _ => throw JsErrorInvalidValue("lineJoin", value)
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
                    _ => throw JsErrorInvalidValue("lineCap", value)
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
                    _ => throw JsErrorInvalidValue("imageSmoothingQuality", value)
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
                    throw JsErrorInvalidValue("font", null);
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
                        _ => throw JsError($"Size unit '{m.Groups["Unit"].Value}' is not supported for CanvasRenderingContext2D.font")
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
                throw JsErrorInvalidValue("font", value);
            }
        }

        public float globalAlpha {
            get => throw JsErrorNotSupported("globalAlpha");
            set => throw JsErrorNotSupported("globalAlpha");
        }

        public string? globalCompositeOperation {
            get => throw JsErrorNotSupported("globalCompositeOperation");
            set => throw JsErrorNotSupported("globalCompositeOperation");
        }

        public object? currentTransform {
            get => throw JsErrorNotSupported("currentTransform");
            set => throw JsErrorNotSupported("currentTransform");
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
                        throw JsErrorInvalidValue("direction", value);
                }
            }
        }

        public string? filter {
            get => throw JsErrorNotSupported("filter");
            set => throw JsErrorNotSupported("filter");
        }

        public float shadowBlur {
            get => throw JsErrorNotSupported("shadowBlur");
            set => throw JsErrorNotSupported("shadowBlur");
        }

        public string? shadowColor {
            get => throw JsErrorNotSupported("shadowColor");
            set => throw JsErrorNotSupported("shadowColor");
        }

        public float shadowOffsetX {
            get => throw JsErrorNotSupported("shadowOffsetX");
            set => throw JsErrorNotSupported("shadowOffsetX");
        }

        public float shadowOffsetY {
            get => throw JsErrorNotSupported("shadowOffsetY");
            set => throw JsErrorNotSupported("shadowOffsetY");
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
                        throw JsErrorInvalidValue("textAlign", value);
                }
            }
        }

        public float textBaseline {
            get => throw JsErrorNotSupported("textBaseline");
            set => throw JsErrorNotSupported("textBaseline");
        }

        #endregion

        #region methods

        public CanvasGradient createLinearGradient(float x0, float y0, float x1, float y1) {
            return new CanvasGradient(engine, x0, y0, x1, y1);
        }

        public CanvasGradient createRadialGradient(float x0, float y0, float r0, float x1, float y1, float r1) {
            throw JsErrorNotSupported("createRadialGradient");
        }

        public object createPattern(object? image, string? repetition) {
            throw JsErrorNotSupported("createPattern");
        }

        #endregion

        public void Dispose() {
            _fillStyleBrush.Dispose();
            _strokeStylePen.Dispose();

        }

    }

}
