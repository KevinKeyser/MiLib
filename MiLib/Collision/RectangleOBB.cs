using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MiLib.Collision
{
    public class RectangleOBB : Shape
    {
        public RectangleOBB(float x, float y, float width, float height, GraphicsDevice graphicsDevice)
            : this(x, y, width, height, Vector2.Zero, graphicsDevice) { }
        public RectangleOBB(Vector2 position, Vector2 size, GraphicsDevice graphicsDevice)
            : this(position.X, position.Y, size.X, size.Y, Vector2.Zero, graphicsDevice) { }
        public RectangleOBB(Vector2 position, Vector2 size, Vector2 origin, GraphicsDevice graphicsDevice)
            : this(position.X, position.Y, size.X, size.Y, origin, graphicsDevice) { }
        public RectangleOBB(float x, float y, float width, float height, Vector2 origin, GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            Position = origin;
            Origin = origin;
            Segments = new Segment[]{
                new Segment(new Vector2(x, y), new Vector2(x + width, y), graphicsDevice),
                new Segment(new Vector2(x + width, y), new Vector2(x + width, y + height), graphicsDevice),
                new Segment(new Vector2(x + width, y + height), new Vector2(x, y + height), graphicsDevice),
                new Segment(new Vector2(x, y + height), new Vector2(x, y), graphicsDevice)
            };
        }
    }
}
