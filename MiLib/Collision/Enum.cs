using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiLib.Collision
{
    public enum ShapeType
    {
        Circle,
        Triangle,
        RectangleOBB,
        Polygon
    }

    [Flags]
    public enum CollisionType
    {
        Circle = 1,
        WithCircle = 2,
        Triangle = 4,
        WithTriangle = 8,
        RectangleOBB = 16,
        WithRectangleOBB = 32,
        Polygon = 64,
        WithPolygon = 128
    }
}
