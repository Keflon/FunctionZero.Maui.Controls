using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionZero.Maui.Controls
{
    public class MaskViewZero : GraphicsView, IDrawable
    {
        private float _x, _y, _w, _h;
        private float _radius;
        private float _alpha;
        private Color _fillColor;
        private Color _strokeColor;
        private float _strokeThickness;
        private float _alphaMultiplier;

        public MaskViewZero()
        {
            _x = 100; _y = 100; _w = 200; _h = 70; _radius = 0.0F;
        }
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = _strokeColor;
            canvas.StrokeSize = _strokeThickness;

            PathF path = new PathF();
            //path.MoveTo(_x+_radius , _y);  2 consecutive MoveTo make crash.

            path.AppendRoundedRectangle(_x, _y, _w, _h, _radius);
            var width = dirtyRect.Width;
            var height = dirtyRect.Height;

            // Draw a box around the edges of the control.
            path.MoveTo(0, 0);
            path.LineTo(width, 0);
            path.LineTo(width, height);
            path.LineTo(0, height);
            path.LineTo(0, 0);

            path.Close();

            canvas.StrokeSize = _strokeThickness;
            canvas.StrokeLineJoin = LineJoin.Round;
            canvas.StrokeColor = _strokeColor;
            canvas.FillColor = _fillColor;
            canvas.Alpha = _alpha * _alphaMultiplier;
            canvas.FillPath(path, WindingMode.EvenOdd);
            canvas.Alpha = _alphaMultiplier;
            canvas.DrawRoundedRectangle(_x - _strokeThickness/2, _y - _strokeThickness/2, _w + _strokeThickness/2 + _strokeThickness/2, _h + _strokeThickness/2 + _strokeThickness/2, _radius);
        }

        public void Update(double x, double y, double w, double h, double roundness, double backgroundAlpha, Color fillColor, Color strokeColor, double strokeThickness, double alphaMultiplier)
        {
            _x = (float)x;
            _y = (float)y;
            _w = (float)w;
            _h = (float)h;
            _radius = (float)roundness * Math.Min(_w, _h) / 2.0F;
            _alpha = (float)backgroundAlpha;
            _fillColor = fillColor;
            _strokeColor = strokeColor;
            _strokeThickness = (float)strokeThickness;
            _alphaMultiplier = (float)alphaMultiplier;
        }
    }
}
