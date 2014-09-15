using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MiLib.CoreTypes;

namespace MiLib.Collision
{
    public class Triangle : Shape
    {
        public Triangle(Vector2 point1, Vector2 point2, Vector2 point3, GraphicsDevice graphicsDevice)
            : this(point1, point2, point3, Vector2.Zero, graphicsDevice) { }

        public Triangle(Vector2 pointA, Vector2 pointB, Vector2 pointC, Vector2 origin, GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            Origin = origin;
            Segments = new Segment[]{
                new Segment(pointA, pointB, origin, graphicsDevice),
                new Segment(pointB, pointC, origin, graphicsDevice),
                new Segment(pointC, pointA, origin, graphicsDevice)
            };
        }

        public bool Intersects(Vector2 point)
        {
            return Util.PointInTriangle(point, vertices[0], vertices[1], vertices[2]);
        }

        public bool Intersects(Triangle triangle)
        {
            for (int i = 0; i < triangle.vertices.Length; i++)
            {
                if (Intersects(triangle.vertices[i])) return true;

                for (int ii = 0; ii < vertices.Length; ii++)
                {
                    if (segments[i].Intersects(triangle.segments[ii])) return true;
                }
            
            }
            return false;
        }
        public bool Intersects(Polygon poly)
        {
            for (int i = 0; i < poly.Vertices.Length; i++)
            {
                if (Intersects(vertices[i])) return true;

                for (int ii = 0; ii < vertices.Length; ii++)
                {
                    if (segments[ii].Intersects(poly.Segments[i])) return true;
                }
            }
            return false;
        }
        public bool Intersects(Segment segment)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                if (segment.Intersects(this.segments[i])) return true;
            }
            return false;
        }

        public bool Intersects(Circle circle)
        {

            Vector2? closest = Util.ClosestPoint(circle.Position, vertices);
            if (closest.HasValue)
            {
                Vector2 v = closest.Value - circle.Position;
                return Vector2.Dot(v, v) <= circle.Bounds.Width * circle.Bounds.Width;
            }
            return false;
        }
    }
}
