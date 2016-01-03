using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MiLib.Collision
{
    public class Triangle : Shape
    {
        public Triangle(Vector2 point1, Vector2 point2, Vector2 point3, GraphicsDevice graphicsDevice)
            : this(point1, point2, point3, Vector2.Zero, graphicsDevice) { }

        public Triangle(Vector2 pointA, Vector2 pointB, Vector2 pointC, Vector2 origin, GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            Position = origin;
            Origin = origin;
            Segments = new Segment[]{
                new Segment(pointA, pointB, origin, graphicsDevice),
                new Segment(pointB, pointC, origin, graphicsDevice),
                new Segment(pointC, pointA, origin, graphicsDevice)
            };
        }
    }
}
