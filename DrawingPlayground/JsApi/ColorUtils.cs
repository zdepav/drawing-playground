using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

#if RENDER_BACKEND_SYSTEM_DRAWING
using System.Drawing;
#elif RENDER_BACKEND_SKIA
using SkiaSharp;
#endif

namespace DrawingPlayground.JsApi {

    internal class ColorUtils {
        
        #if RENDER_BACKEND_SYSTEM_DRAWING

        private static readonly Dictionary<string, Color> cssColors = new Dictionary<string, Color> {
            { "black", Color.FromArgb(0, 0, 0) },
            { "silver", Color.FromArgb(192, 192, 192) },
            { "gray", Color.FromArgb(128, 128, 128) },
            { "white", Color.FromArgb(255, 255, 255) },
            { "maroon", Color.FromArgb(128, 0, 0) },
            { "red", Color.FromArgb(255, 0, 0) },
            { "purple", Color.FromArgb(128, 0, 128) },
            { "fuchsia", Color.FromArgb(255, 0, 255) },
            { "green", Color.FromArgb(0, 128, 0) },
            { "lime", Color.FromArgb(0, 255, 0) },
            { "olive", Color.FromArgb(128, 128, 0) },
            { "yellow", Color.FromArgb(255, 255, 0) },
            { "navy", Color.FromArgb(0, 0, 128) },
            { "blue", Color.FromArgb(0, 0, 255) },
            { "teal", Color.FromArgb(0, 128, 128) },
            { "aqua", Color.FromArgb(0, 255, 255) },
            { "orange", Color.FromArgb(255, 165, 0) },
            { "aliceblue", Color.FromArgb(240, 248, 255) },
            { "antiquewhite", Color.FromArgb(250, 235, 215) },
            { "aquamarine", Color.FromArgb(127, 255, 212) },
            { "azure", Color.FromArgb(240, 255, 255) },
            { "beige", Color.FromArgb(245, 245, 220) },
            { "bisque", Color.FromArgb(255, 228, 196) },
            { "blanchedalmond", Color.FromArgb(255, 235, 205) },
            { "blueviolet", Color.FromArgb(138, 43, 226) },
            { "brown", Color.FromArgb(165, 42, 42) },
            { "burlywood", Color.FromArgb(222, 184, 135) },
            { "cadetblue", Color.FromArgb(95, 158, 160) },
            { "chartreuse", Color.FromArgb(127, 255, 0) },
            { "chocolate", Color.FromArgb(210, 105, 30) },
            { "coral", Color.FromArgb(255, 127, 80) },
            { "cornflowerblue", Color.FromArgb(100, 149, 237) },
            { "cornsilk", Color.FromArgb(255, 248, 220) },
            { "crimson", Color.FromArgb(220, 20, 60) },
            { "cyan", Color.FromArgb(0, 255, 255) },
            { "darkblue", Color.FromArgb(0, 0, 139) },
            { "darkcyan", Color.FromArgb(0, 139, 139) },
            { "darkgoldenrod", Color.FromArgb(184, 134, 11) },
            { "darkgray", Color.FromArgb(169, 169, 169) },
            { "darkgreen", Color.FromArgb(0, 100, 0) },
            { "darkgrey", Color.FromArgb(169, 169, 169) },
            { "darkkhaki", Color.FromArgb(189, 183, 107) },
            { "darkmagenta", Color.FromArgb(139, 0, 139) },
            { "darkolivegreen", Color.FromArgb(85, 107, 47) },
            { "darkorange", Color.FromArgb(255, 140, 0) },
            { "darkorchid", Color.FromArgb(153, 50, 204) },
            { "darkred", Color.FromArgb(139, 0, 0) },
            { "darksalmon", Color.FromArgb(233, 150, 122) },
            { "darkseagreen", Color.FromArgb(143, 188, 143) },
            { "darkslateblue", Color.FromArgb(72, 61, 139) },
            { "darkslategray", Color.FromArgb(47, 79, 79) },
            { "darkslategrey", Color.FromArgb(47, 79, 79) },
            { "darkturquoise", Color.FromArgb(0, 206, 209) },
            { "darkviolet", Color.FromArgb(148, 0, 211) },
            { "deeppink", Color.FromArgb(255, 20, 147) },
            { "deepskyblue", Color.FromArgb(0, 191, 255) },
            { "dimgray", Color.FromArgb(105, 105, 105) },
            { "dimgrey", Color.FromArgb(105, 105, 105) },
            { "dodgerblue", Color.FromArgb(30, 144, 255) },
            { "firebrick", Color.FromArgb(178, 34, 34) },
            { "floralwhite", Color.FromArgb(255, 250, 240) },
            { "forestgreen", Color.FromArgb(34, 139, 34) },
            { "gainsboro", Color.FromArgb(220, 220, 220) },
            { "ghostwhite", Color.FromArgb(248, 248, 255) },
            { "gold", Color.FromArgb(255, 215, 0) },
            { "goldenrod", Color.FromArgb(218, 165, 32) },
            { "greenyellow", Color.FromArgb(173, 255, 47) },
            { "grey", Color.FromArgb(128, 128, 128) },
            { "honeydew", Color.FromArgb(240, 255, 240) },
            { "hotpink", Color.FromArgb(255, 105, 180) },
            { "indianred", Color.FromArgb(205, 92, 92) },
            { "indigo", Color.FromArgb(75, 0, 130) },
            { "ivory", Color.FromArgb(255, 255, 240) },
            { "khaki", Color.FromArgb(240, 230, 140) },
            { "lavender", Color.FromArgb(230, 230, 250) },
            { "lavenderblush", Color.FromArgb(255, 240, 245) },
            { "lawngreen", Color.FromArgb(124, 252, 0) },
            { "lemonchiffon", Color.FromArgb(255, 250, 205) },
            { "lightblue", Color.FromArgb(173, 216, 230) },
            { "lightcoral", Color.FromArgb(240, 128, 128) },
            { "lightcyan", Color.FromArgb(224, 255, 255) },
            { "lightgoldenrodyellow", Color.FromArgb(250, 250, 210) },
            { "lightgray", Color.FromArgb(211, 211, 211) },
            { "lightgreen", Color.FromArgb(144, 238, 144) },
            { "lightgrey", Color.FromArgb(211, 211, 211) },
            { "lightpink", Color.FromArgb(255, 182, 193) },
            { "lightsalmon", Color.FromArgb(255, 160, 122) },
            { "lightseagreen", Color.FromArgb(32, 178, 170) },
            { "lightskyblue", Color.FromArgb(135, 206, 250) },
            { "lightslategray", Color.FromArgb(119, 136, 153) },
            { "lightslategrey", Color.FromArgb(119, 136, 153) },
            { "lightsteelblue", Color.FromArgb(176, 196, 222) },
            { "lightyellow", Color.FromArgb(255, 255, 224) },
            { "limegreen", Color.FromArgb(50, 205, 50) },
            { "linen", Color.FromArgb(250, 240, 230) },
            { "magenta", Color.FromArgb(255, 0, 255) },
            { "mediumaquamarine", Color.FromArgb(102, 205, 170) },
            { "mediumblue", Color.FromArgb(0, 0, 205) },
            { "mediumorchid", Color.FromArgb(186, 85, 211) },
            { "mediumpurple", Color.FromArgb(147, 112, 219) },
            { "mediumseagreen", Color.FromArgb(60, 179, 113) },
            { "mediumslateblue", Color.FromArgb(123, 104, 238) },
            { "mediumspringgreen", Color.FromArgb(0, 250, 154) },
            { "mediumturquoise", Color.FromArgb(72, 209, 204) },
            { "mediumvioletred", Color.FromArgb(199, 21, 133) },
            { "midnightblue", Color.FromArgb(25, 25, 112) },
            { "mintcream", Color.FromArgb(245, 255, 250) },
            { "mistyrose", Color.FromArgb(255, 228, 225) },
            { "moccasin", Color.FromArgb(255, 228, 181) },
            { "navajowhite", Color.FromArgb(255, 222, 173) },
            { "oldlace", Color.FromArgb(253, 245, 230) },
            { "olivedrab", Color.FromArgb(107, 142, 35) },
            { "orangered", Color.FromArgb(255, 69, 0) },
            { "orchid", Color.FromArgb(218, 112, 214) },
            { "palegoldenrod", Color.FromArgb(238, 232, 170) },
            { "palegreen", Color.FromArgb(152, 251, 152) },
            { "paleturquoise", Color.FromArgb(175, 238, 238) },
            { "palevioletred", Color.FromArgb(219, 112, 147) },
            { "papayawhip", Color.FromArgb(255, 239, 213) },
            { "peachpuff", Color.FromArgb(255, 218, 185) },
            { "peru", Color.FromArgb(205, 133, 63) },
            { "pink", Color.FromArgb(255, 192, 203) },
            { "plum", Color.FromArgb(221, 160, 221) },
            { "powderblue", Color.FromArgb(176, 224, 230) },
            { "rosybrown", Color.FromArgb(188, 143, 143) },
            { "royalblue", Color.FromArgb(65, 105, 225) },
            { "saddlebrown", Color.FromArgb(139, 69, 19) },
            { "salmon", Color.FromArgb(250, 128, 114) },
            { "sandybrown", Color.FromArgb(244, 164, 96) },
            { "seagreen", Color.FromArgb(46, 139, 87) },
            { "seashell", Color.FromArgb(255, 245, 238) },
            { "sienna", Color.FromArgb(160, 82, 45) },
            { "skyblue", Color.FromArgb(135, 206, 235) },
            { "slateblue", Color.FromArgb(106, 90, 205) },
            { "slategray", Color.FromArgb(112, 128, 144) },
            { "slategrey", Color.FromArgb(112, 128, 144) },
            { "snow", Color.FromArgb(255, 250, 250) },
            { "springgreen", Color.FromArgb(0, 255, 127) },
            { "steelblue", Color.FromArgb(70, 130, 180) },
            { "tan", Color.FromArgb(210, 180, 140) },
            { "thistle", Color.FromArgb(216, 191, 216) },
            { "tomato", Color.FromArgb(255, 99, 71) },
            { "turquoise", Color.FromArgb(64, 224, 208) },
            { "violet", Color.FromArgb(238, 130, 238) },
            { "wheat", Color.FromArgb(245, 222, 179) },
            { "whitesmoke", Color.FromArgb(245, 245, 245) },
            { "yellowgreen", Color.FromArgb(154, 205, 50) },
            { "rebeccapurple", Color.FromArgb(102, 51, 153) },
            { "transparent", Color.FromArgb(0, 0, 0, 0) },
        };

        private static bool TryGetCssColorFromName(string s, out Color c) => cssColors.TryGetValue(s, out c);

        private static readonly string hexadecimalDigits = "0123456789abcdef";

        private static readonly Regex hexColorRegex = new Regex(@"#(?:(?<RGB1>[0-9a-f]{6})(?<A1>[0-9a-f]{2})?|(?<RGB2>[0-9a-f]{3})(?<A2>[0-9a-f])?)");

        private static bool TryGetCssColorFromHex(string s, out Color c) {
            c = Color.Black;
            var m = hexColorRegex.Match(s.ToLower(CultureInfo.InvariantCulture));
            if (!m.Success) {
                return false;
            }
            if (m.Groups["RGB1"].Success) {
                var rgbStr = m.Groups["RGB1"].Value;
                var r = hexadecimalDigits.IndexOf(rgbStr[0]) * 16 + hexadecimalDigits.IndexOf(rgbStr[1]);
                var g = hexadecimalDigits.IndexOf(rgbStr[2]) * 16 + hexadecimalDigits.IndexOf(rgbStr[3]);
                var b = hexadecimalDigits.IndexOf(rgbStr[4]) * 16 + hexadecimalDigits.IndexOf(rgbStr[5]);
                var a = 255;
                if (m.Groups["A1"].Success) {
                    var aStr = m.Groups["A1"].Value;
                    a = hexadecimalDigits.IndexOf(aStr[0]) * 16 + hexadecimalDigits.IndexOf(aStr[1]);
                }
                c = Color.FromArgb(a, r, g, b);
                return true;
            } else if (m.Groups["RGB2"].Success) {
                var rgbStr = m.Groups["RGB2"].Value;
                var r = hexadecimalDigits.IndexOf(rgbStr[0]) * 17;
                var g = hexadecimalDigits.IndexOf(rgbStr[1]) * 17;
                var b = hexadecimalDigits.IndexOf(rgbStr[2]) * 17;
                var a = 255;
                if (m.Groups["A2"].Success) {
                    a = hexadecimalDigits.IndexOf(m.Groups["A2"].Value[0]) * 17;
                }
                c = Color.FromArgb(a, r, g, b);
                return true;
            } else {
                return false;
            }
        }

        private static readonly string floatRegexStr = /*lang=regex*/@"\d+\+?(?:[eE]\d+)|\d*\.\d+(?:[eE]\d+)";

        private static readonly Regex[] cssFunctionRgbRegexes = {
            new Regex(string.Format( /*lang=regex*/@"rgba?\s*\(\s*(?<R>{0})\s*,\s*(?<G>{0})\s*,\s*(?<B>{0})(?:\s*,\s*(?<A>{0}%?))?\s*\)", floatRegexStr)),
            new Regex(string.Format( /*lang=regex*/@"rgba?\s*\(\s*(?<R>{0})%\s*,\s*(?<G>{0})%\s*,\s*(?<B>{0})%(?:\s*,\s*(?<A>{0}%?))?\s*\)", floatRegexStr)),
            new Regex(string.Format( /*lang=regex*/@"rgba?\s*\(\s*(?<R>{0})\s+(?<G>{0})\s+(?<B>{0})(?:\s*/\s*(?<A>{0}%?))?\s*\)", floatRegexStr)),
            new Regex(string.Format( /*lang=regex*/@"rgba?\s*\(\s*(?<R>{0})%\s+(?<G>{0})%\s+(?<B>{0})%(?:\s*/\s*(?<A>{0}%?))?\s*\)", floatRegexStr))
        };

        private static bool TryGetAlpha(bool rgba, Group g, out int a) {
            a = 255;
            if (rgba && !g.Success) {
                return false;
            }
            return !g.Success || TryGetIntValueBetween0And255(g.Value, out a);
        }

        private static bool TryGetIntValueBetween0And255(string str, out int v) {
            v = 255;
            if (str.EndsWith("%")) {
                // percentage
                if (double.TryParse(str.Substring(0, str.Length - 1), out var value)) {
                    v = value < 0.0 ? 0 : value > 100.0 ? 255 : (int)Math.Round(value * 2.55);
                    return true;
                } else {
                    return false;
                }
            } else {
                if (double.TryParse(str, out var value)) {
                    v = value < 0.0 ? 0 : value > 255.0 ? 255 : (int)Math.Round(value);
                    return true;
                } else {
                    return false;
                }
            }
        }

        private static bool TryGetCssColorFromRgbFunction(string str, out Color c) {
            c = Color.Black;
            var m = cssFunctionRgbRegexes.Select(regex => regex.Match(str))
                                         .FirstOrDefault(match => match.Success);
            if (m == null) {
                return false;
            }
            if (TryGetIntValueBetween0And255(m.Groups["R"].Value, out var r) &&
                TryGetIntValueBetween0And255(m.Groups["G"].Value, out var g) &&
                TryGetIntValueBetween0And255(m.Groups["B"].Value, out var b) &&
                TryGetAlpha(str[4] == 'a', m.Groups["A"], out var a)
            ) {
                c = Color.FromArgb(a, r, g, b);
                return true;
            } else {
                return false;
            }
        }
        
        private static readonly Regex[] cssFunctionHslRegexes = {
            new Regex(string.Format( /*lang=regex*/@"hsla?\s*\(\s*(?<H>{0})(?<Htype>deg|g?rad|turn)?\s*,\s*(?<S>{0})%\s*,\s*(?<L>{0})%(?:\s*,\s*(?<A>{0}%?))?\s*\)", floatRegexStr)),
            new Regex(string.Format( /*lang=regex*/@"hsla?\s*\(\s*(?<H>{0})(?<Htype>deg|g?rad|turn)?\s+(?<S>{0})%\s+(?<L>{0})%(?:\s*/\s*(?<A>{0}%?))?\s*\)", floatRegexStr))
        };

        private static bool TryGetAngleDegrees(string str, Group type, out double angle) {
            if (double.TryParse(str, out var value)) {
                switch (type.Success ? type.Value : "rad") {
                    case "deg":
                        angle = value;
                        return true;
                    case "rad":
                        angle = value * 180.0 / Math.PI;
                        return true;
                    case "grad":
                        angle = value * Math.PI / 200.0;
                        return true;
                    case "turn":
                        angle = value * 360.0;
                        return true;
                }
            }
            angle = 0.0;
            return false;
        }

        private static bool TryGetValueBetween0And100(string str, out double v) {
            v = 100.0;
            if (double.TryParse(str, out var value)) {
                v = value < 0.0 ? 0.0 : value > 100.0 ? 100.0 : value;
                return true;
            } else {
                return false;
            }
        }

        private static int FloatToByte(double d) {
            var b = (int)Math.Round(d * 255.0);
            return b < 0 ? 0 : b > 255 ? 255 : b;
        }

        private static bool TryGetCssColorFromHslFunction(string str, out Color c) {
            c = Color.Black;
            var m = cssFunctionHslRegexes.Select(regex => regex.Match(str))
                                         .FirstOrDefault(match => match.Success);
            if (m == null) {
                return false;
            }
            if (TryGetAngleDegrees(m.Groups["H"].Value, m.Groups["Htype"], out var h) &&
                TryGetValueBetween0And100(m.Groups["S"].Value, out var s) &&
                TryGetValueBetween0And100(m.Groups["L"].Value, out var l) &&
                TryGetAlpha(str[4] == 'a', m.Groups["A"], out var a)
            ) {
                h = (h < 0 ? 360 - ((-h) % 360) : h) % 360;
                var y = (1 - Math.Abs(2.0 * l - 1.0)) * s;
                var x = y * (1 - Math.Abs(h / 60.0 % 2 - 1));
                var n = l - y / 2.0;
                var (r, g, b) = ((int)h / 60) switch {
                    0 => (y, x, 0.0),
                    1 => (x, y, 0.0),
                    2 => (0.0, y, x),
                    3 => (0.0, x, y),
                    4 => (x, 0.0, y),
                    5 => (y, 0.0, x),
                    _ => (0.0, 0.0, 0.0)
                };
                c = Color.FromArgb(a, FloatToByte(r + n), FloatToByte(g + n), FloatToByte(b + n));
                return true;
            } else {
                return false;
            }
        }

        public static bool TryGetCssColor(string s, out Color c) {
            s = s.Trim();
            return TryGetCssColorFromName(s, out c) ||
                   TryGetCssColorFromHex(s, out c) ||
                   TryGetCssColorFromRgbFunction(s, out c) ||
                   TryGetCssColorFromHslFunction(s, out c);
        }


        #elif RENDER_BACKEND_SKIA


        private static readonly Dictionary<string, SKColor> cssColors = new Dictionary<string, SKColor> {
            { "black", new SKColor(0, 0, 0) },
            { "silver", new SKColor(192, 192, 192) },
            { "gray", new SKColor(128, 128, 128) },
            { "white", new SKColor(255, 255, 255) },
            { "maroon", new SKColor(128, 0, 0) },
            { "red", new SKColor(255, 0, 0) },
            { "purple", new SKColor(128, 0, 128) },
            { "fuchsia", new SKColor(255, 0, 255) },
            { "green", new SKColor(0, 128, 0) },
            { "lime", new SKColor(0, 255, 0) },
            { "olive", new SKColor(128, 128, 0) },
            { "yellow", new SKColor(255, 255, 0) },
            { "navy", new SKColor(0, 0, 128) },
            { "blue", new SKColor(0, 0, 255) },
            { "teal", new SKColor(0, 128, 128) },
            { "aqua", new SKColor(0, 255, 255) },
            { "orange", new SKColor(255, 165, 0) },
            { "aliceblue", new SKColor(240, 248, 255) },
            { "antiquewhite", new SKColor(250, 235, 215) },
            { "aquamarine", new SKColor(127, 255, 212) },
            { "azure", new SKColor(240, 255, 255) },
            { "beige", new SKColor(245, 245, 220) },
            { "bisque", new SKColor(255, 228, 196) },
            { "blanchedalmond", new SKColor(255, 235, 205) },
            { "blueviolet", new SKColor(138, 43, 226) },
            { "brown", new SKColor(165, 42, 42) },
            { "burlywood", new SKColor(222, 184, 135) },
            { "cadetblue", new SKColor(95, 158, 160) },
            { "chartreuse", new SKColor(127, 255, 0) },
            { "chocolate", new SKColor(210, 105, 30) },
            { "coral", new SKColor(255, 127, 80) },
            { "cornflowerblue", new SKColor(100, 149, 237) },
            { "cornsilk", new SKColor(255, 248, 220) },
            { "crimson", new SKColor(220, 20, 60) },
            { "cyan", new SKColor(0, 255, 255) },
            { "darkblue", new SKColor(0, 0, 139) },
            { "darkcyan", new SKColor(0, 139, 139) },
            { "darkgoldenrod", new SKColor(184, 134, 11) },
            { "darkgray", new SKColor(169, 169, 169) },
            { "darkgreen", new SKColor(0, 100, 0) },
            { "darkgrey", new SKColor(169, 169, 169) },
            { "darkkhaki", new SKColor(189, 183, 107) },
            { "darkmagenta", new SKColor(139, 0, 139) },
            { "darkolivegreen", new SKColor(85, 107, 47) },
            { "darkorange", new SKColor(255, 140, 0) },
            { "darkorchid", new SKColor(153, 50, 204) },
            { "darkred", new SKColor(139, 0, 0) },
            { "darksalmon", new SKColor(233, 150, 122) },
            { "darkseagreen", new SKColor(143, 188, 143) },
            { "darkslateblue", new SKColor(72, 61, 139) },
            { "darkslategray", new SKColor(47, 79, 79) },
            { "darkslategrey", new SKColor(47, 79, 79) },
            { "darkturquoise", new SKColor(0, 206, 209) },
            { "darkviolet", new SKColor(148, 0, 211) },
            { "deeppink", new SKColor(255, 20, 147) },
            { "deepskyblue", new SKColor(0, 191, 255) },
            { "dimgray", new SKColor(105, 105, 105) },
            { "dimgrey", new SKColor(105, 105, 105) },
            { "dodgerblue", new SKColor(30, 144, 255) },
            { "firebrick", new SKColor(178, 34, 34) },
            { "floralwhite", new SKColor(255, 250, 240) },
            { "forestgreen", new SKColor(34, 139, 34) },
            { "gainsboro", new SKColor(220, 220, 220) },
            { "ghostwhite", new SKColor(248, 248, 255) },
            { "gold", new SKColor(255, 215, 0) },
            { "goldenrod", new SKColor(218, 165, 32) },
            { "greenyellow", new SKColor(173, 255, 47) },
            { "grey", new SKColor(128, 128, 128) },
            { "honeydew", new SKColor(240, 255, 240) },
            { "hotpink", new SKColor(255, 105, 180) },
            { "indianred", new SKColor(205, 92, 92) },
            { "indigo", new SKColor(75, 0, 130) },
            { "ivory", new SKColor(255, 255, 240) },
            { "khaki", new SKColor(240, 230, 140) },
            { "lavender", new SKColor(230, 230, 250) },
            { "lavenderblush", new SKColor(255, 240, 245) },
            { "lawngreen", new SKColor(124, 252, 0) },
            { "lemonchiffon", new SKColor(255, 250, 205) },
            { "lightblue", new SKColor(173, 216, 230) },
            { "lightcoral", new SKColor(240, 128, 128) },
            { "lightcyan", new SKColor(224, 255, 255) },
            { "lightgoldenrodyellow", new SKColor(250, 250, 210) },
            { "lightgray", new SKColor(211, 211, 211) },
            { "lightgreen", new SKColor(144, 238, 144) },
            { "lightgrey", new SKColor(211, 211, 211) },
            { "lightpink", new SKColor(255, 182, 193) },
            { "lightsalmon", new SKColor(255, 160, 122) },
            { "lightseagreen", new SKColor(32, 178, 170) },
            { "lightskyblue", new SKColor(135, 206, 250) },
            { "lightslategray", new SKColor(119, 136, 153) },
            { "lightslategrey", new SKColor(119, 136, 153) },
            { "lightsteelblue", new SKColor(176, 196, 222) },
            { "lightyellow", new SKColor(255, 255, 224) },
            { "limegreen", new SKColor(50, 205, 50) },
            { "linen", new SKColor(250, 240, 230) },
            { "magenta", new SKColor(255, 0, 255) },
            { "mediumaquamarine", new SKColor(102, 205, 170) },
            { "mediumblue", new SKColor(0, 0, 205) },
            { "mediumorchid", new SKColor(186, 85, 211) },
            { "mediumpurple", new SKColor(147, 112, 219) },
            { "mediumseagreen", new SKColor(60, 179, 113) },
            { "mediumslateblue", new SKColor(123, 104, 238) },
            { "mediumspringgreen", new SKColor(0, 250, 154) },
            { "mediumturquoise", new SKColor(72, 209, 204) },
            { "mediumvioletred", new SKColor(199, 21, 133) },
            { "midnightblue", new SKColor(25, 25, 112) },
            { "mintcream", new SKColor(245, 255, 250) },
            { "mistyrose", new SKColor(255, 228, 225) },
            { "moccasin", new SKColor(255, 228, 181) },
            { "navajowhite", new SKColor(255, 222, 173) },
            { "oldlace", new SKColor(253, 245, 230) },
            { "olivedrab", new SKColor(107, 142, 35) },
            { "orangered", new SKColor(255, 69, 0) },
            { "orchid", new SKColor(218, 112, 214) },
            { "palegoldenrod", new SKColor(238, 232, 170) },
            { "palegreen", new SKColor(152, 251, 152) },
            { "paleturquoise", new SKColor(175, 238, 238) },
            { "palevioletred", new SKColor(219, 112, 147) },
            { "papayawhip", new SKColor(255, 239, 213) },
            { "peachpuff", new SKColor(255, 218, 185) },
            { "peru", new SKColor(205, 133, 63) },
            { "pink", new SKColor(255, 192, 203) },
            { "plum", new SKColor(221, 160, 221) },
            { "powderblue", new SKColor(176, 224, 230) },
            { "rosybrown", new SKColor(188, 143, 143) },
            { "royalblue", new SKColor(65, 105, 225) },
            { "saddlebrown", new SKColor(139, 69, 19) },
            { "salmon", new SKColor(250, 128, 114) },
            { "sandybrown", new SKColor(244, 164, 96) },
            { "seagreen", new SKColor(46, 139, 87) },
            { "seashell", new SKColor(255, 245, 238) },
            { "sienna", new SKColor(160, 82, 45) },
            { "skyblue", new SKColor(135, 206, 235) },
            { "slateblue", new SKColor(106, 90, 205) },
            { "slategray", new SKColor(112, 128, 144) },
            { "slategrey", new SKColor(112, 128, 144) },
            { "snow", new SKColor(255, 250, 250) },
            { "springgreen", new SKColor(0, 255, 127) },
            { "steelblue", new SKColor(70, 130, 180) },
            { "tan", new SKColor(210, 180, 140) },
            { "thistle", new SKColor(216, 191, 216) },
            { "tomato", new SKColor(255, 99, 71) },
            { "turquoise", new SKColor(64, 224, 208) },
            { "violet", new SKColor(238, 130, 238) },
            { "wheat", new SKColor(245, 222, 179) },
            { "whitesmoke", new SKColor(245, 245, 245) },
            { "yellowgreen", new SKColor(154, 205, 50) },
            { "rebeccapurple", new SKColor(102, 51, 153) },
            { "transparent", new SKColor(0, 0, 0, 0) },
        };

        private static bool TryGetCssColorFromName(string s, out SKColor c) => cssColors.TryGetValue(s, out c);

        private static readonly string hexadecimalDigits = "0123456789abcdef";

        private static readonly Regex hexColorRegex = new Regex(@"#(?:(?<RGB1>[0-9a-f]{6})(?<A1>[0-9a-f]{2})?|(?<RGB2>[0-9a-f]{3})(?<A2>[0-9a-f])?)");

        private static bool TryGetCssColorFromHex(string s, out SKColor c) {
            c = SKColors.Black;
            var m = hexColorRegex.Match(s.ToLower(CultureInfo.InvariantCulture));
            if (!m.Success) {
                return false;
            }
            if (m.Groups["RGB1"].Success) {
                var rgbStr = m.Groups["RGB1"].Value;
                var r = hexadecimalDigits.IndexOf(rgbStr[0]) * 16 + hexadecimalDigits.IndexOf(rgbStr[1]);
                var g = hexadecimalDigits.IndexOf(rgbStr[2]) * 16 + hexadecimalDigits.IndexOf(rgbStr[3]);
                var b = hexadecimalDigits.IndexOf(rgbStr[4]) * 16 + hexadecimalDigits.IndexOf(rgbStr[5]);
                var a = 255;
                if (m.Groups["A1"].Success) {
                    var aStr = m.Groups["A1"].Value;
                    a = hexadecimalDigits.IndexOf(aStr[0]) * 16 + hexadecimalDigits.IndexOf(aStr[1]);
                }
                c = new SKColor((byte)r, (byte)g, (byte)b, (byte)a);
                return true;
            } else if (m.Groups["RGB2"].Success) {
                var rgbStr = m.Groups["RGB2"].Value;
                var r = hexadecimalDigits.IndexOf(rgbStr[0]) * 17;
                var g = hexadecimalDigits.IndexOf(rgbStr[1]) * 17;
                var b = hexadecimalDigits.IndexOf(rgbStr[2]) * 17;
                var a = 255;
                if (m.Groups["A2"].Success) {
                    a = hexadecimalDigits.IndexOf(m.Groups["A2"].Value[0]) * 17;
                }
                c = new SKColor((byte)r, (byte)g, (byte)b, (byte)a);
                return true;
            } else {
                return false;
            }
        }

        private static readonly string floatRegexStr = /*lang=regex*/@"\d+\+?(?:[eE]\d+)|\d*\.\d+(?:[eE]\d+)";

        private static readonly Regex[] cssFunctionRgbRegexes = {
            new Regex(string.Format( /*lang=regex*/@"rgba?\s*\(\s*(?<R>{0})\s*,\s*(?<G>{0})\s*,\s*(?<B>{0})(?:\s*,\s*(?<A>{0}%?))?\s*\)", floatRegexStr)),
            new Regex(string.Format( /*lang=regex*/@"rgba?\s*\(\s*(?<R>{0})%\s*,\s*(?<G>{0})%\s*,\s*(?<B>{0})%(?:\s*,\s*(?<A>{0}%?))?\s*\)", floatRegexStr)),
            new Regex(string.Format( /*lang=regex*/@"rgba?\s*\(\s*(?<R>{0})\s+(?<G>{0})\s+(?<B>{0})(?:\s*/\s*(?<A>{0}%?))?\s*\)", floatRegexStr)),
            new Regex(string.Format( /*lang=regex*/@"rgba?\s*\(\s*(?<R>{0})%\s+(?<G>{0})%\s+(?<B>{0})%(?:\s*/\s*(?<A>{0}%?))?\s*\)", floatRegexStr))
        };

        private static bool TryGetAlpha(bool rgba, Group g, out int a) {
            a = 255;
            if (rgba && !g.Success) {
                return false;
            }
            return !g.Success || TryGetIntValueBetween0And255(g.Value, out a);
        }

        private static bool TryGetIntValueBetween0And255(string str, out int v) {
            v = 255;
            if (str.EndsWith("%")) {
                // percentage
                if (double.TryParse(str.Substring(0, str.Length - 1), out var value)) {
                    v = value < 0.0 ? 0 : value > 100.0 ? 255 : (int)Math.Round(value * 2.55);
                    return true;
                } else {
                    return false;
                }
            } else {
                if (double.TryParse(str, out var value)) {
                    v = value < 0.0 ? 0 : value > 255.0 ? 255 : (int)Math.Round(value);
                    return true;
                } else {
                    return false;
                }
            }
        }

        private static bool TryGetCssColorFromRgbFunction(string str, out SKColor c) {
            c = SKColors.Black;
            var m = cssFunctionRgbRegexes.Select(regex => regex.Match(str))
                                         .FirstOrDefault(match => match.Success);
            if (m == null) {
                return false;
            }
            if (TryGetIntValueBetween0And255(m.Groups["R"].Value, out var r) &&
                TryGetIntValueBetween0And255(m.Groups["G"].Value, out var g) &&
                TryGetIntValueBetween0And255(m.Groups["B"].Value, out var b) &&
                TryGetAlpha(str[4] == 'a', m.Groups["A"], out var a)
            ) {
                c = new SKColor((byte)r, (byte)g, (byte)b, (byte)a);
                return true;
            } else {
                return false;
            }
        }
        
        private static readonly Regex[] cssFunctionHslRegexes = {
            new Regex(string.Format( /*lang=regex*/@"hsla?\s*\(\s*(?<H>{0})(?<Htype>deg|g?rad|turn)?\s*,\s*(?<S>{0})%\s*,\s*(?<L>{0})%(?:\s*,\s*(?<A>{0}%?))?\s*\)", floatRegexStr)),
            new Regex(string.Format( /*lang=regex*/@"hsla?\s*\(\s*(?<H>{0})(?<Htype>deg|g?rad|turn)?\s+(?<S>{0})%\s+(?<L>{0})%(?:\s*/\s*(?<A>{0}%?))?\s*\)", floatRegexStr))
        };

        private static bool TryGetAngleDegrees(string str, Group type, out double angle) {
            if (double.TryParse(str, out var value)) {
                switch (type.Success ? type.Value : "rad") {
                    case "deg":
                        angle = value;
                        return true;
                    case "rad":
                        angle = value * 180.0 / Math.PI;
                        return true;
                    case "grad":
                        angle = value * Math.PI / 200.0;
                        return true;
                    case "turn":
                        angle = value * 360.0;
                        return true;
                }
            }
            angle = 0.0;
            return false;
        }

        private static bool TryGetValueBetween0And100(string str, out double v) {
            v = 100.0;
            if (double.TryParse(str, out var value)) {
                v = value < 0.0 ? 0.0 : value > 100.0 ? 100.0 : value;
                return true;
            } else {
                return false;
            }
        }

        private static byte FloatToByte(double d) {
            var b = (int)Math.Round(d * 255.0);
            return (byte)(b < 0 ? 0 : b > 255 ? 255 : b);
        }

        private static bool TryGetCssColorFromHslFunction(string str, out SKColor c) {
            c = SKColors.Black;
            var m = cssFunctionHslRegexes.Select(regex => regex.Match(str))
                                         .FirstOrDefault(match => match.Success);
            if (m == null) {
                return false;
            }
            if (TryGetAngleDegrees(m.Groups["H"].Value, m.Groups["Htype"], out var h) &&
                TryGetValueBetween0And100(m.Groups["S"].Value, out var s) &&
                TryGetValueBetween0And100(m.Groups["L"].Value, out var l) &&
                TryGetAlpha(str[4] == 'a', m.Groups["A"], out var a)
            ) {
                h = (h < 0 ? 360 - ((-h) % 360) : h) % 360;
                var y = (1 - Math.Abs(2.0 * l - 1.0)) * s;
                var x = y * (1 - Math.Abs(h / 60.0 % 2 - 1));
                var n = l - y / 2.0;
                var (r, g, b) = ((int)h / 60) switch {
                    0 => (y, x, 0.0),
                    1 => (x, y, 0.0),
                    2 => (0.0, y, x),
                    3 => (0.0, x, y),
                    4 => (x, 0.0, y),
                    5 => (y, 0.0, x),
                    _ => (0.0, 0.0, 0.0)
                };
                c = new SKColor(FloatToByte(r + n), FloatToByte(g + n), FloatToByte(b + n), (byte)a);
                return true;
            } else {
                return false;
            }
        }

        public static bool TryGetCssColor(string s, out SKColor c) {
            s = s.Trim();
            return TryGetCssColorFromName(s, out c) ||
                   TryGetCssColorFromHex(s, out c) ||
                   TryGetCssColorFromRgbFunction(s, out c) ||
                   TryGetCssColorFromHslFunction(s, out c);
        }

        #endif
        
    }

}
