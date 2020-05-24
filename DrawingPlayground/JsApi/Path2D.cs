#nullable enable
using System;
using System.Drawing;
using Jint;
#if RENDER_BACKEND_SYSTEM_DRAWING
using System.Drawing.Drawing2D;

#elif RENDER_BACKEND_SKIA
using SkiaSharp;
#endif

namespace DrawingPlayground.JsApi {

    public class Path2D : IDisposable {

        private readonly Engine engine;

        #if RENDER_BACKEND_SYSTEM_DRAWING

        private float currentX, currentY;

        internal GraphicsPath Path { get; }

        internal Path2D(Engine engine) {
            this.engine = engine;
            currentX = currentY = 0f;
            Path = new GraphicsPath();
        }

        public Path2D(Path2D? path) {
            if (path is null) {
                throw JsErrorUtils.InvalidValue(engine, nameof(Path2D), nameof(Path2D), nameof(path), null);
            }
            engine = path.engine;
            Path = (GraphicsPath)path.Path.Clone();
        }

        private void addPath(GraphicsPath gp, Matrix transform) {
            gp.Flatten(transform);
            Path.AddPath(gp, false);
        }

        /// <summary>Adds a path to the current path.</summary>
        /// <param name="path">A Path2D path to add</param>
        /// <param name="transform">An SVGMatrix to be used as the transformation matrix for the path that is added</param>
        public void addPath(Path2D? path, SVGMatrix? transform) {
            if (path is null) {
                throw JsErrorUtils.InvalidValue(engine, nameof(Path2D), nameof(Path2D), nameof(path), null);
            }
            var gp = (GraphicsPath)path.Path.Clone();
            if (transform is null) {
                gp.Flatten();
            } else using (var t = transform.ToMatrix()) {
                gp.Flatten(t);
            }
            Path.AddPath(gp, false);
        }

        /// <summary>
        /// Causes the point of the pen to move back to the start of the current sub-path.
        /// It tries to draw a straight line from the current point to the start.
        /// If the shape has already been closed or has only one point, this function does nothing.
        /// </summary>
        public void closePath() {
            Path.CloseFigure();
        }

        /// <summary>Moves the starting point of a new sub-path to the (x, y) coordinates.</summary>
        /// <param name="x">The x-axis (horizontal) coordinate of the point</param>
        /// <param name="y">The y-axis (vertical) coordinate of the point</param>
        public void moveTo(float x, float y) {
            if (FloatUtils.InfiniteOrNaN(x, y)) return;
            (currentX, currentY) = (x, y);
        }

        /// <summary>Connects the last point in the subpath to the (x, y) coordinates with a straight line.</summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void lineTo(float x, float y) {
            if (FloatUtils.InfiniteOrNaN(x, y)) return;
            Path.AddLine(currentX, currentY, x, y);
            (currentX, currentY) = (x, y);
        }

        /// <summary>
        /// Adds a cubic Bézier curve to the path. It requires three points.
        /// The first two points are control points and the third one is the end point.
        /// The starting point is the last point in the current path,
        /// which can be changed using moveTo() before creating the Bézier curve.
        /// </summary>
        /// <param name="cp1x">The x-axis coordinate of the first control point</param>
        /// <param name="cp1y">The y-axis coordinate of the first control point</param>
        /// <param name="cp2x">The x-axis coordinate of the second control point</param>
        /// <param name="cp2y">The y-axis coordinate of the second control point</param>
        /// <param name="x">The x-axis coordinate of the end point</param>
        /// <param name="y">The y-axis coordinate of the end point</param>
        public void bezierCurveTo(float cp1x, float cp1y, float cp2x, float cp2y, float x, float y) {
            if (FloatUtils.InfiniteOrNaN(cp1x, cp1y, cp2x, cp2y, x, y)) return;
            Path.AddBezier(currentX, currentY, cp1x, cp1y, cp2x, cp2y, x, y);
            (currentX, currentY) = (x, y);
        }

        /// <summary>Adds a quadratic Bézier curve to the current path.</summary>
        /// <param name="cpx">The x-axis coordinate of the control point</param>
        /// <param name="cpy">The y-axis coordinate of the control point</param>
        /// <param name="x">The x-axis coordinate of the end point</param>
        /// <param name="y">The y-axis coordinate of the end point</param>
        public void quadraticCurveTo(float cpx, float cpy, float x, float y) {
            if (FloatUtils.InfiniteOrNaN(cpx, cpy, x, y)) return;
            Path.AddBezier(
                currentX, currentY,
                currentX + (cpx - currentX) * 0.6667, currentY + (cpy - currentY) * 0.6667,
                cpx + (x - cpx) * 0.3333, cpy + (y - cpy) * 0.3333,
                x, y
            );
            (currentX, currentY) = (x, y);
        }

        /// <summary>
        /// Adds an arc to the path which is centered at (x, y) position with radius r starting at startAngle
        /// and ending at endAngle going in the given direction by anticlockwise (defaulting to clockwise).
        /// </summary>
        /// <param name="x">The horizontal coordinate of the arc's center</param>
        /// <param name="y">The vertical coordinate of the arc's center</param>
        /// <param name="radius">The arc's radius. Must be positive</param>
        /// <param name="startAngle">The angle at which the arc starts in radians, measured from the positive x-axis</param>
        /// <param name="endAngle">The angle at which the arc ends in radians, measured from the positive x-axis</param>
        /// <param name="anticlockwise">An optional Boolean. If true, draws the arc counter-clockwise between the start and end angles. The default is false (clockwise)</param>
        public void arc(float x, float y, float radius, float startAngle, float endAngle, bool anticlockwise) {
            ellipse(x, y, radius, radius, 0f, startAngle, endAngle, anticlockwise);
        }

        /// <summary>
        /// Adds an arc to the path which is centered at (x, y) position with radius r starting at startAngle
        /// and ending at endAngle going in the given direction by anticlockwise (defaulting to clockwise).
        /// </summary>
        /// <param name="x">The horizontal coordinate of the arc's center</param>
        /// <param name="y">The vertical coordinate of the arc's center</param>
        /// <param name="radius">The arc's radius. Must be positive</param>
        /// <param name="startAngle">The angle at which the arc starts in radians, measured from the positive x-axis</param>
        /// <param name="endAngle">The angle at which the arc ends in radians, measured from the positive x-axis</param>
        public void arc(float x, float y, float radius, float startAngle, float endAngle) {
            ellipse(x, y, radius, radius, 0f, startAngle, endAngle, false);
        }

        /// <summary>Adds a circular arc to the path with the given control points and radius, connected to the previous point by a straight line.</summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="radius"></param>
        public void arcTo(float x1, float y1, float x2, float y2, float radius) {
            throw new NotImplementedException("arcTo not yet implemented");
        }

        /// <summary>
        /// Adds an elliptical arc to the path which is centered at (x, y) position with the radii radiusX and radiusY starting
        /// at startAngle and ending at endAngle going in the given direction by anticlockwise (defaulting to clockwise).
        /// </summary>
        /// <param name="x">The x-axis (horizontal) coordinate of the ellipse's center</param>
        /// <param name="y">The y-axis (vertical) coordinate of the ellipse's center</param>
        /// <param name="radiusX">The ellipse's major-axis radius. Must be non-negative</param>
        /// <param name="radiusY">The ellipse's minor-axis radius. Must be non-negative</param>
        /// <param name="rotation">The rotation of the ellipse, expressed in radians</param>
        /// <param name="startAngle">The angle at which the ellipse starts, measured clockwise from the positive x-axis and expressed in radians</param>
        /// <param name="endAngle">The angle at which the ellipse ends, measured clockwise from the positive x-axis and expressed in radians</param>
        /// <param name="anticlockwise">An optional Boolean which, if true, draws the ellipse anticlockwise (counter-clockwise). The default value is false (clockwise)</param>
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
            if (FloatUtils.InfiniteOrNaN(x, y, rotation, startAngle, endAngle)) return;
            if (FloatUtils.InfiniteZeroOrNaN(radiusX, radiusY)) return;
            if (radiusX < 0) {
                throw JsErrorUtils.InvalidValue(engine, nameof(Path2D), nameof(ellipse), nameof(radiusX), radiusX);
            }
            if (radiusY < 0) {
                throw JsErrorUtils.InvalidValue(engine, nameof(Path2D), nameof(ellipse), nameof(radiusY), radiusY);
            }
            var gp = new GraphicsPath();
            startAngle = MathUtils.WrapAngleDeg(MathUtils.Rad2Deg(startAngle));
            endAngle = MathUtils.WrapAngleDeg(MathUtils.Rad2Deg(endAngle));
            var (from, to) = anticlockwise
                ? (Math.Max(startAngle, endAngle), Math.Min(startAngle, endAngle) + 360)
                : (Math.Min(startAngle, endAngle), Math.Max(startAngle, endAngle));
            gp.AddArc(x - radiusX, y - radiusY, radiusX * 2, radiusY * 2, from, to - from);
            var matrix = new Matrix();
            matrix.Rotate(rotation);
            addPath(gp, matrix);
        }

        /// <summary>Adds an elliptical arc to the path which is centered at (x, y) position with the radii radiusX and radiusY starting at startAngle and ending at endAngle going in the given direction by anticlockwise (defaulting to clockwise).</summary>
        /// <param name="x">The x-axis (horizontal) coordinate of the ellipse's center</param>
        /// <param name="y">The y-axis (vertical) coordinate of the ellipse's center</param>
        /// <param name="radiusX">The ellipse's major-axis radius. Must be non-negative</param>
        /// <param name="radiusY">The ellipse's minor-axis radius. Must be non-negative</param>
        /// <param name="rotation">The rotation of the ellipse, expressed in radians</param>
        /// <param name="startAngle">The angle at which the ellipse starts, measured clockwise from the positive x-axis and expressed in radians</param>
        /// <param name="endAngle">The angle at which the ellipse ends, measured clockwise from the positive x-axis and expressed in radians</param>
        public void ellipse(
            float x,
            float y,
            float radiusX,
            float radiusY,
            float rotation,
            float startAngle,
            float endAngle
        ) {
            ellipse(x, y, radiusX, radiusY, rotation, startAngle, endAngle, false);
        }

        /// <summary>Creates a path for a rectangle at position (x, y) with a size that is determined by width and height.</summary>
        /// <param name="x">The x-axis coordinate of the rectangle's starting point</param>
        /// <param name="y">The y-axis coordinate of the rectangle's starting point</param>
        /// <param name="width">The rectangle's width. Positive values are to the right, and negative to the left</param>
        /// <param name="height">The rectangle's height. Positive values are down, and negative are up</param>
        public void rect(float x, float y, float width, float height) {
            if (FloatUtils.InfiniteOrNaN(x, y)) return;
            if (FloatUtils.InfiniteZeroOrNaN(width, height)) return;
            if (width < 0) {
                x = width;
                width = -width;
            }
            if (height < 0) {
                y = height;
                height = -height;
            }
            Path.AddRectangle(new RectangleF(x, y, width, height));
        }

        public void Dispose() {
            Path.Dispose();
        }

#elif RENDER_BACKEND_SKIA
#endif

    }

}
