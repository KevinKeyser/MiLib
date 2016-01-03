using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiLib.CoreTypes;

namespace MiLib.Collision
{
    public class Polygon : Shape
    {

        private Triangle[] triangles = new Triangle[0];

        public Triangle[] Triangles
        {
            get { return triangles; }
            protected set { triangles = value; }
        }

        public override Vector2 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                base.Position = value;
                for (int i = 0; i < triangles.Length; i++)
                {
                    triangles[i].Position = value;
                }
            }
        }

        public override float Rotation
        {
            get
            {
                return base.Rotation;
            }
            set
            {
                base.Rotation = value;
                for (int i = 0; i < triangles.Length; i++)
                {
                    triangles[i].Rotation = value;
                }
            }
        }

        public override float Scale
        {
            get
            {
                return base.Scale;
            }
            set
            {
                base.Scale = value;
                for (int i = 0; i < triangles.Length; i++)
                {
                    triangles[i].Scale = value;
                }
            }
        }

        public Polygon(Vector2[] vertices, GraphicsDevice graphicsDevice)
            : this(vertices, Vector2.Zero, graphicsDevice) {}

        public Polygon(Vector2[] vertices, Vector2 origin, GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            Position = origin;
            Origin = origin;
            Segment[] temp = new Segment[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                if (i != vertices.Length - 1)
                {
                    temp[i] = new Segment(vertices[i], vertices[i + 1], origin, graphicsDevice);
                }
                else
                {
                    temp[i] = new Segment(vertices[i], vertices[0], origin, graphicsDevice);
                }
            }
            Segments = temp;
            Triangulate(graphicsDevice, origin);
        }

        public Polygon(Segment[] segments, Vector2 origin, GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            Origin = origin;
            Segments = segments;
            Triangulate(graphicsDevice, origin);
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
       

        //Gets stuck in Triangulation if format isnt proper
        private void Triangulate(GraphicsDevice graphics, Vector2 origin)
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Debug)
            {
                for (int i = 0; i < triangles.Length; i++)
                {
                    triangles[i].Draw(spriteBatch);
                }
            }
            base.Draw(spriteBatch);
        }
    }
}
