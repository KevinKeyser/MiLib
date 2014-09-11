using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MiLib.CoreTypes;

namespace MiLib.Collision
{
    class Circle
    {
        public Vector2 Position;
        public float Radius;
        private Texture2D texture;
        private Vector2[] points;

        public Circle(Vector2 position, float radius, GraphicsDevice graphics)
        {
            int textureSize = 2;
            texture = new Texture2D(graphics, textureSize, textureSize, false, SurfaceFormat.Color);
            texture.SetData<Color>(Enumerable.Repeat<Color>(Color.White, textureSize * textureSize).ToArray<Color>());
            
            Position = position;
            Radius = radius;
            points = new Vector2[(int)(radius*20)];
            float x = -Radius;
            for(int i  = 0; i < points.Length; i++)
            {
                points[i] = new Vector2(x, (float)Math.Sqrt(Radius*Radius - x * x));
                x += .1f;
            }
        }

        public bool Intersects(Polygon poly)
        {

            Vector2? closest = Util.ClosestPoint(Position, poly.vertices);
            if(closest.HasValue)
            {
                return Vector2.DistanceSquared(closest.Value, Position) <= Radius*Radius;
            }
            return false;
        }

        public bool Intersects(Circle circle)
        {
            return Vector2.DistanceSquared(Position, circle.Position) <= (Radius + circle.Radius) * (Radius + circle.Radius);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, Color.Green);
            for(int i = 0; i < points.Length; i++)
            {
                spriteBatch.Draw(texture, new Vector2(Position.X + points[i].X, Position.Y + points[i].Y), Color.Red);
                spriteBatch.Draw(texture, new Vector2(Position.X + points[i].X, Position.Y - points[i].Y), Color.Red);
            }
        }

        public Polygon toPolygon()
        {
            return new Polygon(points, Position, texture.GraphicsDevice);
        }
    }
}
