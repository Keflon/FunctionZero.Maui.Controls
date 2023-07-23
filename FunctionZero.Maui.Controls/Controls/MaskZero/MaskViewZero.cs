using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionZero.Maui.Controls
{
    public class MaskViewZero : GraphicsView, IDrawable
    {
        private float _x, _y, _radius;
        private float _alpha;
        private Color _fillColor;
        private Color _strokeColor;
        private float _strokeThickness;
        public MaskViewZero()
        {
            _x = 100;_y = 100;_radius = 90;
        }
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = _strokeColor;
            canvas.StrokeSize = _strokeThickness;

            PathF path = new PathF();
            //path.MoveTo(_x+_radius , _y);  2 consecutive MoveTo make crash.

            path.AppendCircle(new Point(_x, _y), (float)_radius);

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
            canvas.Alpha = _alpha;
            canvas.FillPath(path, WindingMode.EvenOdd);
            canvas.Alpha = 1F;
            canvas.DrawEllipse(_x - _radius, _y - _radius, _radius * 2, _radius * 2);
        }

        public void Update(double x, double y, double radius, double alpha, Color fillColor, Color strokeColor, double strokeThickness)
        {
            _x = (float)x;
            _y = (float)y;
            _radius = (float)radius;
            _alpha = (float)alpha;
            _fillColor = fillColor;
            _strokeColor = strokeColor;
            _strokeThickness = (float)strokeThickness;
        }
    }
}
