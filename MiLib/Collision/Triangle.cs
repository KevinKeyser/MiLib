using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MiLib.CoreTypes;

namespace MiLib.Collision
{
    class Triangle
    {
        public Vector2[] vertices = new Vector2[3];
        public Segment[] segments = new Segment[3];
        public float[] verticeAngles = new float[3];
        public float[] verticeLength = new float[3];
        public float rotTotal = 0;
        Rectangle bounds;
        public Vector2 origin;

        public Triangle(Vector2 point1, Vector2 point2, Vector2 point3, GraphicsDevice graphics)
            : this(point1, point2, point3, Vector2.Zero, graphics) { }

        public Triangle(Vector2 pointA, Vector2 pointB, Vector2 pointC, Vector2 origin, GraphicsDevice graphics)
        {
            vertices[0] = pointA;
            vertices[1] = pointB;
            vertices[2] = pointC;
            bounds = new Rectangle((int)Math.Min(Math.Min(pointA.X, pointB.X), pointC.X),
                (int)Math.Min(Math.Min(pointA.Y, pointB.Y), pointC.Y),
                (int)Math.Max(Math.Max(pointA.X, pointB.X), pointC.X) - (int)Math.Min(Math.Min(pointA.X, pointB.X), pointC.X),
                (int)Math.Max(Math.Max(pointA.Y, pointB.Y), pointC.Y) - (int)Math.Min(Math.Min(pointA.Y, pointB.Y), pointC.Y));
            this.origin = origin;
            for (int i = 0; i < vertices.Length; i++)
            {
                if (i != vertices.Length - 1)
                {
                    segments[i] = new Segment(vertices[i], vertices[i + 1], origin, graphics);
                }
                else
                {
                    segments[i] = new Segment(vertices[i], vertices[0], origin, graphics);
                }
                verticeLength[i] = (vertices[i] - origin).Length();
                verticeAngles[i] = Util.VectorToAngle(vertices[i] - origin);
            }
        }

        public void Move(Vector2 translation)
        {
            for(int i = 0; i < vertices.Length; i++)
            {
                vertices[i] += translation;
                segments[i].Move(translation);
            }
            origin += translation;
            bounds.X += (int)translation.X;
            bounds.Y += (int)translation.Y;
        }

        public void Rotation(float degrees)
        {
            rotTotal += degrees;
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector2 angleVector = Util.AngleToVector(rotTotal + verticeAngles[i]);
                angleVector.Normalize();
                vertices[i] = origin + angleVector * verticeLength[i];
                segments[i].PointA = vertices[i];
                segments[i].PointB = (i == vertices.Length - 1) ? vertices[0] : vertices[i + 1];
            }
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
            for (int i = 0; i < poly.vertices.Length; i++)
            {
                if (Intersects(vertices[i])) return true;

                for (int ii = 0; ii < vertices.Length; ii++)
                {
                    if (segments[ii].Intersects(poly.segments[i])) return true;
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
                return Vector2.Dot(v, v) <= circle.Radius * circle.Radius;
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                segments[i].Draw(spriteBatch);
            }
        }
    }
}
