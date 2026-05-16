using System.Drawing;

namespace UITest;

public static class RectangleHelper
{
    extension(Rectangle rect)
    {
        public Point Center => new(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2);
    }
}
