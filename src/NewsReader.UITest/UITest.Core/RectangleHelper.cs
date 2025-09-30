using System.Drawing;

namespace UITest;

public static class RectangleHelper
{
    public static Point Center(this Rectangle rect) => new(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2);
}
