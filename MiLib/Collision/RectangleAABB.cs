using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MiLib.Collision
{
    public class RectangleAABB : Shape
    {
        public RectangleAABB(GraphicsDevice graphicsDevice)
            : this(graphicsDevice, 0, 0, 0, 0, Vector2.Zero) { }
        public RectangleAABB(GraphicsDevice graphicsDevice, float x, float y, float width, float height)
            : this(graphicsDevice, x, y, width, height, Vector2.Zero) { }
        public RectangleAABB(GraphicsDevice graphicsDevice, Vector2 position, Vector2 size)
            : this(graphicsDevice, position.X, position.Y, size.X, size.Y, Vector2.Zero) { }
        public RectangleAABB(GraphicsDevice graphicsDevice, Vector2 position, Vector2 size, Vector2 origin)
            : this(graphicsDevice, position.X, position.Y, size.X, size.Y, origin) { }
        public RectangleAABB(GraphicsDevice graphicsDevice, float x, float y, float width, float height, Vector2 origin)
            : base(graphicsDevice)
        {
            Position = new Vector2(x, y);
            Origin = origin;
            Segments = new Segment[]{
                new Segment(new Vector2(x, y), new Vector2(x + width, y), graphicsDevice),
                new Segment(new Vector2(x + width, y), new Vector2(x + width, y + height), graphicsDevice),
                new Segment(new Vector2(x + width, y + height), new Vector2(x, y + height), graphicsDevice),
                new Segment(new Vector2(x, y + height), new Vector2(x, y), graphicsDevice)
            };
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Debug)
            {
                for (int i = 0; i < segments.Length; i++ )
                {
                    segments[i].Draw(spriteBatch);
                }
            }
            base.Draw(spriteBatch);
        }
    }
}
