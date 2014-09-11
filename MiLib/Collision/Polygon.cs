using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MiLib.CoreTypes;

namespace MiLib.Collision
{
    /* Todo:
     * add scaling corresponding to origin
     */
    class Polygon
    {
        public Vector2[] vertices;
        public Segment[] segments;
        public float[] verticeAngles;
        public float[] verticeLength;
        public float rotTotal = 0;
        public Vector2 origin;
        Rectangle bounds;

        Triangle[] triangles = new Triangle[0];

        public Polygon(Vector2[] vertices, GraphicsDevice graphics)
        : this(vertices, Vector2.Zero, graphics) {}

        public Polygon(Vector2[] vertices, Vector2 origin, GraphicsDevice graphics)
        {
            segments = new Segment[vertices.Length];
            verticeAngles = new float[vertices.Length];
            verticeLength = new float[vertices.Length];
            this.vertices = vertices;
            bounds = new Rectangle(int.MaxValue, int.MaxValue, 0, 0);
            foreach (Vector2 vertice in vertices)
            {
                if (bounds.X > vertice.X) bounds.X = (int)vertice.X;
                if (bounds.Y > vertice.Y) bounds.Y = (int)vertice.Y;
                if (bounds.Width < vertice.X) bounds.Width = (int)vertice.X;
                if (bounds.Height < vertice.X) bounds.Height = (int)vertice.Y;
            }
            bounds.Height -= bounds.Y;
            bounds.Width -= bounds.X;
            this.origin = origin;
            Triangulate(graphics, origin);
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
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] += translation;
                segments[i].Move(translation);
            }
            for (int i = 0; i < triangles.Length; i++)
            {
                triangles[i].Move(translation);
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
            for (int i = 0; i < triangles.Length; i++)
            {
                triangles[i].Rotation(degrees);   
            }
        }

        public bool Intersects(Vector2 point)
        {
            int high = vertices.Length;
            int low = 0;
            do {
                int mid = (low + high) / 2;
                if (Util.TriangleOrientation(vertices[0], vertices[mid], point) == Order.CounterClockWise)
                    low = mid;
                else
                    high = mid;
            } while (low + 1 < high);
            if (low == 0 || high == vertices.Length) return false;
            return (Util.TriangleOrientation(vertices[low], vertices[high], point) == Order.CounterClockWise) ? true : false;
        }

        public bool Intersects(Circle circle)
        {

            Vector2? closest = Util.ClosestPoint(circle.Position, vertices);
            if (closest.HasValue)
            {
                return Vector2.DistanceSquared(closest.Value, circle.Position) <= circle.Radius * circle.Radius;
            }
            return false;
        }

        public bool Intersects(Triangle triangle)
        {
            for (int i = 0; i < triangle.vertices.Length; i++)
            {
                    if (Intersects(triangle.vertices[i])) return true;
                for (int ii = 0; ii < vertices.Length; ii++)
                {
                    if (segments[ii].Intersects(triangle.segments[i])) return true;
                }
            }
            return false;
        }
       
        public bool Intersects(Polygon poly)
        {
            for (int i = 0; i < triangles.Length; i++)
            {
                for(int ii = 0; ii < poly.triangles.Length; ii++)
                {
                    if (triangles[i].Intersects(poly.triangles[ii]))
                        return true;
                }
            }
            return false;
        }
       void Triangulate(GraphicsDevice graphics, Vector2 origin)
       {
           int[] prev = new int[vertices.Length];
           int[] next =  new int[vertices.Length];
           for (int ii = 0; ii < vertices.Length; ii++) {
               prev[ii] = ii - 1;
               next[ii] = ii + 1;
           }
           prev[0] = vertices.Length - 1;
           next[vertices.Length - 1] = 0;
           
            int n = vertices.Length;
            int i = 0;
            while (n > 3)
            {
                int isEar = 1;
                if (Util.TriangleOrientation(vertices[prev[i]], vertices[i], vertices[next[i]]) == Order.ClockWise)
                {
                    int k = next[next[i]];
                    do
                    {
                        if (Util.PointInTriangle(vertices[k], vertices[prev[i]], vertices[i], vertices[next[i]]))
                        {
                            isEar = 0;
                            break;
                        }
                        k = next[k];
                    } while (k != prev[i]);
                }
                else
                {
                    isEar = 0;
                }
                if (isEar == 1)
                {
                    Triangle[] temp = triangles;
                    triangles = new Triangle[triangles.Length + 1];
                    for (int ii = 0; ii < temp.Length; ii++)
                    {
                        triangles[ii] = temp[ii];
                    }
                    triangles[triangles.Length - 1] = new Triangle(vertices[prev[i]], vertices[i], vertices[next[i]], origin, graphics);
                    next[prev[i]] = next[i];
                    prev[next[i]] = prev[i];
                    
                    n--;
                    i = prev[i];
                }
                else
                {
                    i = next[i];
                }
            }
            Triangle[] temp1 = triangles;
            triangles = new Triangle[triangles.Length + 1];
            for (int ii = 0; ii < temp1.Length; ii++)
            {
                triangles[ii] = temp1[ii];
            }
            triangles[triangles.Length - 1] = new Triangle(vertices[prev[i]], vertices[i], vertices[next[i]], origin, graphics);
            next[prev[i]] = next[i];
            prev[next[i]] = prev[i];
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                segments[i].Draw(spriteBatch);
            }
            for (int i = 0; i < triangles.Length; i++)
            {
                triangles[i].Draw(spriteBatch);
            }
        }
    }
}
