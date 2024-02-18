using Microsoft.Maui.Layouts;

namespace FunctionZero.Maui.Controls
{
    internal class MultiViewZeroLayoutManager : ILayoutManager
    {
        private MultiViewZero _multiView;

        public MultiViewZeroLayoutManager(MultiViewZero multiView)
        {
            _multiView = multiView;
        }

        public Size ArrangeChildren(Rect bounds)
        {
            var padding = _multiView.Padding;

            double left = padding.Left + bounds.X;
            double top = padding.Top + bounds.Y;
            double width = bounds.Width - padding.HorizontalThickness;
            double height = bounds.Height - padding.VerticalThickness;

            // Place all visible children to fill the bounds.
            // Any animations will be done using transforms that affect only the render pass.
            var destination = new Rect(left, top, width, height);

            foreach (var item in _multiView.Children)
                if (item.Visibility != Visibility.Collapsed)
                    item.Arrange(destination);

            return new Size(width, height);
        }

        public Size Measure(double widthConstraint, double heightConstraint)
        {
            var padding = _multiView.Padding;

            var size = new Size(widthConstraint - padding.Left - padding.Right, heightConstraint - padding.Top - padding.Bottom);

            foreach (var item in _multiView.Children)
                item.Measure(size.Width, size.Height);

            return new Size(widthConstraint - padding.Left - padding.Right, heightConstraint - padding.Top - padding.Bottom);
        }
    }
}