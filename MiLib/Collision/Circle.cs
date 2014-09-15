using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MiLib.CoreTypes;

namespace MiLib.Collision
{
    public class Circle : Shape
    {
        public Circle(Vector2 position, float radius, GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            Position = position;
            Origin = origin;
            Segment[] temp = new Segment[(int)radius * 2 + 2];
            Segment[] temp2 = new Segment[(int)radius * 2 + 2];
            float x = -radius;
            for(int i  = 0; i < temp.Length; i++)
            {
                float y = (float)Math.Sqrt(radius * radius - x * x);
                if (i == 0)
                {
                    temp[i] = new Segment(new Vector2(position.X + x, position.Y - y), new Vector2(position.X + x, position.Y + y), position, graphicsDevice);
                    temp2[i] = new Segment(temp[i].PointA, new Vector2(position.X + x, position.Y - y), position, graphicsDevice);
                }
                else if(i == temp.Length - 1)
                {
                    temp2[i] = new Segment(new Vector2(position.X + x, position.Y - y), new Vector2(position.X + x, position.Y + y), position, graphicsDevice);
                    temp[i] = new Segment(temp2[i].PointB, new Vector2(position.X + x, position.Y - y), position, graphicsDevice);
                }
                else
                {
                    temp[i] = new Segment(temp[i - 1].PointB, new Vector2(position.X + x, position.Y + y), position, graphicsDevice);
                    temp2[i] = new Segment(temp2[i - 1].PointB, new Vector2(position.X + x, position.Y - y), position, graphicsDevice);
                }
                x += 1f;
            }
            Segment[] total = new Segment[temp.Length + temp2.Length + 1];
            for(int i = 0, ii = 0; i < temp.Length; i++, ii+=2)
            {
                total[ii] = temp[i];
                total[ii + 1] = temp2[i];
            }
            total[total.Length - 1] = new Segment(total[total.Length - 2].PointB, total[total.Length - 3].PointB, origin, graphicsDevice);
            Segments = total;
        }

        public bool Intersects(Polygon poly)
        {

            Vector2? closest = Util.ClosestPoint(Position, poly.Vertices);
            if(closest.HasValue)
            {
                return Vector2.DistanceSquared(closest.Value, Position) <= Bounds.Width*Bounds.Width;
            }
            return false;
        }

        public bool Intersects(Circle circle)
        {
            return Vector2.DistanceSquared(Position, circle.Position) <= (Bounds.Width + circle.Bounds.Width) * (Bounds.Width + circle.Bounds.Width);
        }

        public Polygon toPolygon()
        {
            return new Polygon(segments, Position, graphicsDevice);
        }
    }
}
