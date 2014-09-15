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
        RectangleAABB,
        Polygon
    }

    [Flags]
    public enum CollisionType
    {
        Circle = 1,
        WithCircle = 2,
        Triangle = 4,
        WithTriangle = 8,
        RectangleAABB = 16,
        WithRectangleAABB = 32,
        Polygon = 64,
        WithPolygon = 128
    }
}
