using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MiLib.CoreTypes;

namespace MiLib.Collision
{
    public abstract class Shape
    {
        public bool Debug;

        public ShapeType ShapeType { get; protected set; }

        protected CollisionType collisionType;
        protected Dictionary<CollisionType, Func<Shape, Shape, bool>> collisionHandlers = new Dictionary<CollisionType, Func<Shape, Shape, bool>>();

        protected Vector2 position;

        public virtual Vector2 Position
        {
            get { return position; }
            set
            {
                Vector2 oldposition = value - position;
                position = value;
                origin += oldposition;
                for (int i = 0; i < segments.Length; i++)
                {
                    segments[i].Position = value;
                    segments[i].Origin = origin;
                    vertices[i] = segments[i].PointA;
                }
                Bounds = new RectangleAABB(Bounds.X + oldposition.X, Bounds.Y + oldposition.Y, Bounds.Width, Bounds.Height);
            }
        }

        protected Rotation rotation;


        //Broken Negative Scale
        public virtual Rotation Rotation
        {
            get { return rotation; }
            set
            { 
                rotation = value;
                float width = float.MinValue;
                float height = float.MinValue;
                float x = float.MaxValue;
                float y = float.MaxValue;
                for (int i = 0; i < segments.Length; i++)
                {
                    segments[i].Rotation = value;
                    vertices[i] = segments[i].PointA;
                    x = vertices[i].X < x ? vertices[i].X : x;
                    y = vertices[i].Y < y ? vertices[i].Y : y;
                    width = vertices[i].X > width ? vertices[i].X : width;
                    height = vertices[i].Y > height ? vertices[i].Y : height;
                }
                width -= x;
                height -= y;
                Bounds = new RectangleAABB(x, y, width, height);
            }
        }

        protected Vector2 origin;

        public virtual Vector2 Origin
        {
            get { return origin; }
            set 
            {
                origin = value;
                for (int i = 0; i < segments.Length; i++)
                {
                    segments[i].Origin = value;
                }
            }
        }

        //fixes scale 0 for now
        private bool scaleZero = false;
        protected float scale;

        //Broken negative scale;
        public virtual float Scale
        {
            get
            {
                return scale;
            }
            set
            {
                if (value == 0)
                {
                    scaleZero = true;
                }
                else
                {
                    scaleZero = false;
                    scale = value;
                    float width = float.MinValue;
                    float height = float.MinValue;
                    float x = float.MaxValue;
                    float y = float.MaxValue;
                    for (int i = 0; i < segments.Length; i++)
                    {
                        segments[i].Scale = value;
                        vertices[i] = segments[i].PointA;
                        x = vertices[i].X < x ? vertices[i].X : x;
                        y = vertices[i].Y < y ? vertices[i].Y : y;
                        width = vertices[i].X > width ? vertices[i].X : width;
                        height = vertices[i].Y > height ? vertices[i].Y : height;
                    }
                    width -= x;
                    height -= y;
                    Bounds = new RectangleAABB(x, y, width, height);
                }
            }
        }

        protected Segment[] segments;

        public Segment[] Segments
        {
            get { return segments; }
            set
            {
                segments = new Segment[value.Length];
                for (int i = 0; i < segments.Length; i ++)
                {
                    segments[i] = new Segment(value[i].PointA, value[i].PointB, origin, graphicsDevice);
                }
                verticeLength = new float[value.Length];
                verticeAngles = new float[value.Length];
                float width = float.MinValue;
                float height = float.MinValue;
                float x = float.MaxValue;
                float y = float.MaxValue;
                vertices = new Vector2[value.Length];
                for(int i = 0; i < value.Length; i++)
                {
                    verticeLength[i] = (segments[i].PointA - origin).Length();
                    verticeAngles[i] = Util.VectorToAngle(segments[i].PointA - origin);
                    vertices[i] = value[i].PointA;
                    x = vertices[i].X < x ? vertices[i].X : x;
                    y = vertices[i].Y < y ? vertices[i].Y : y;
                    width = vertices[i].X > width ? vertices[i].X : width;
                    height = vertices[i].Y > height ? vertices[i].Y : height;
                }
                width -= x;
                height -= y;
                Bounds = new RectangleAABB(x, y, width, height);
            }
        }

        protected Vector2[] vertices;

        public Vector2[] Vertices
        {
            get { return vertices; }
        }

        protected float[] verticeAngles;

        public float[] VerticeAngles
        {
            get { return verticeAngles; }
        }

        protected float[] verticeLength;

        public float[] VerticeLength
        {
            get { return verticeLength; }
        }

        protected RectangleAABB bounds;

        public virtual RectangleAABB Bounds
        {
            get { return bounds; }
            private set
            {
                bounds = value;
                boundsSegments[0] = new Segment(new Vector2(value.X, value.Y), new Vector2(value.X + value.Width, value.Y), origin, graphicsDevice);
                boundsSegments[1] = new Segment(new Vector2(value.X + value.Width, value.Y), new Vector2(value.X + value.Width, value.Y + value.Height), origin, graphicsDevice);
                boundsSegments[2] = new Segment(new Vector2(value.X + value.Width, value.Y + value.Height), new Vector2(value.X, value.Y + value.Height),  origin, graphicsDevice);
                boundsSegments[3] = new Segment(new Vector2(value.X, value.Y + value.Height), new Vector2(value.X, value.Y), origin, graphicsDevice);
            }
        }

        private Segment[] boundsSegments;
        protected GraphicsDevice graphicsDevice;
        protected Shape(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            segments = new Segment[0];
            vertices = new Vector2[segments.Length];
            boundsSegments = new Segment[4];
            scale = 1f;
            for (int i = 0; i < boundsSegments.Length; i++)
            {
                boundsSegments[i] = new Segment(Vector2.Zero, Vector2.Zero, Vector2.Zero, graphicsDevice);
            }
            Origin = Vector2.Zero;
            rotation = new CoreTypes.Rotation(0);
            bounds = new RectangleAABB();
            Debug = false;

            string shapeType = this.GetType().Name;
            ShapeType = (ShapeType)Enum.Parse(typeof(ShapeType), shapeType);
            collisionType = (CollisionType)Enum.Parse(typeof(CollisionType), shapeType);


            //Circles
            collisionHandlers.Add(CollisionType.Circle | CollisionType.WithCircle, new Func<Shape, Shape, bool>((s, o) => intersects((Circle)s, (Circle)o)));
            collisionHandlers.Add(CollisionType.Circle | CollisionType.WithTriangle, new Func<Shape, Shape, bool>((s, o) => intersects((Circle)s, (Triangle)o)));
            collisionHandlers.Add(CollisionType.Triangle | CollisionType.WithCircle, new Func<Shape, Shape, bool>((s, o) => intersects((Circle)o, (Triangle)s)));
            collisionHandlers.Add(CollisionType.Circle | CollisionType.WithPolygon, new Func<Shape, Shape, bool>((s, o) => intersects((Circle)s, (Polygon)o)));
            collisionHandlers.Add(CollisionType.Polygon | CollisionType.WithCircle, new Func<Shape, Shape, bool>((s, o) => intersects((Circle)o, (Polygon)s)));
            collisionHandlers.Add(CollisionType.Circle | CollisionType.WithRectangleOBB, new Func<Shape, Shape, bool>((s, o) => intersects((Circle)s, (RectangleOBB)o)));
            collisionHandlers.Add(CollisionType.RectangleOBB | CollisionType.WithCircle, new Func<Shape, Shape, bool>((s, o) => intersects((Circle)o, (RectangleOBB)s)));

            //Triangles
            collisionHandlers.Add(CollisionType.Triangle | CollisionType.WithTriangle, new Func<Shape, Shape, bool>((s, o) => intersects((Triangle)s, (Triangle)o)));
            collisionHandlers.Add(CollisionType.Triangle | CollisionType.WithRectangleOBB, new Func<Shape, Shape, bool>((s, o) => intersects((Triangle)s, (RectangleOBB)o)));
            collisionHandlers.Add(CollisionType.RectangleOBB | CollisionType.WithTriangle, new Func<Shape, Shape, bool>((s, o) => intersects((Triangle)o, (RectangleOBB)s)));
            collisionHandlers.Add(CollisionType.Triangle | CollisionType.WithPolygon, new Func<Shape, Shape, bool>((s, o) => intersects((Triangle)s, (Polygon)o)));
            collisionHandlers.Add(CollisionType.Polygon | CollisionType.WithTriangle, new Func<Shape, Shape, bool>((s, o) => intersects((Triangle)o, (Polygon)s)));

            //Polygon
            collisionHandlers.Add(CollisionType.Polygon | CollisionType.WithPolygon, new Func<Shape, Shape, bool>((s, o) => intersects((Polygon)s, (Polygon)o)));
            collisionHandlers.Add(CollisionType.Polygon | CollisionType.WithRectangleOBB, new Func<Shape, Shape, bool>((s, o) => intersects((Polygon)s, (RectangleOBB)o)));
            collisionHandlers.Add(CollisionType.RectangleOBB | CollisionType.WithPolygon, new Func<Shape, Shape, bool>((s, o) => intersects((Polygon)o, (RectangleOBB)s)));
        
            //RectangleAABB
            collisionHandlers.Add(CollisionType.RectangleOBB | CollisionType.WithRectangleOBB, new Func<Shape,Shape,bool>((s, o) => intersects((RectangleOBB)s, (RectangleOBB)o)));
        }

        public bool Intersects(Shape other)
        {
            if (Bounds.Intersects(other.Bounds))
            {
                CollisionType currentCollision = collisionType;

                switch (other.ShapeType)
                {
                    case ShapeType.Circle:
                        currentCollision |= CollisionType.WithCircle;
                        break;

                    case ShapeType.Triangle:
                        currentCollision |= CollisionType.WithTriangle;
                        break;

                    case ShapeType.RectangleOBB:
                        currentCollision |= CollisionType.WithRectangleOBB;
                        break;

                    case ShapeType.Polygon:
                        currentCollision |= CollisionType.WithPolygon;
                        break;
                }

                return collisionHandlers[currentCollision](this, other);
            }
            return false;
        }

        //Must fix segment to circle
        #region Circle Checking
        private bool intersects(Circle circle, Circle other)
        {
            return Vector2.DistanceSquared(other.Position, circle.Position) <= (other.Bounds.Width/2 + circle.Bounds.Width/2) * (other.Bounds.Width/2 + circle.Bounds.Width/2);
        }

        private bool intersects(Circle circle, Triangle triangle)
        {
            float SqrRadius = (circle.Bounds.Width / 2) * (circle.Bounds.Width / 2);
            Vector2? closest = Util.ClosestPoint(circle.Position, triangle.Vertices);
            if (closest.HasValue)
            {
                Vector2 v = closest.Value - circle.Position;
                if(Vector2.Dot(v, v) <= SqrRadius)
                {
                    return true;
                }
            }
            if (Util.PointInTriangle(circle.Position, triangle.Vertices[0], triangle.Vertices[1], triangle.Vertices[2]))
            {
                return true;
            }
            for (int i = 0; i < triangle.Segments.Length; i++)
            {
            }
            return false;
        }

        private bool intersects(Circle circle, RectangleOBB rect)
        {
            Vector2 d = circle.Position - rect.Bounds.Center;
            // Start result at center of box; make steps from there
            Vector2 closest = rect.Bounds.Center;
            // For each OBB axis...
           /* for (int i = 0; i < 2; i++)
            {
                // ...project d onto that axis to get the distance
                // along the axis of d from the box center
                float dist = Vector2.Dot(d, new Vector2(rect.Bounds.X, rect.Bounds.Width + rect.Bounds.X));
              
                // If distance farther than the box extents, clamp to the box if (dist > b.e[i]) dist = b.e[i];
                if (dist < -b.e[i]) dist = -b.e[i];
                // Step that distance along the axis to get world coordinate
                q += dist * b.u[i];
            }*/
            return false;
        }

        private bool intersects(Circle circle, Polygon poly)
        {
            float SqrRadius = (circle.Bounds.Width / 2) * (circle.Bounds.Width / 2);
            Vector2? closest = Util.ClosestPoint(circle.Position, poly.Vertices);
            if (closest.HasValue)
            {
                if(Vector2.DistanceSquared(closest.Value, circle.Position) <= SqrRadius)
                {
                    return true;
                }
            }
            for (int i = 0; i < poly.Triangles.Length; i++ )
            {
                if (Util.PointInTriangle(circle.Position, poly.Triangles[i].Vertices[0], poly.Triangles[i].Vertices[1], poly.Triangles[i].Vertices[2]))
                {
                    return true;
                }
                for (int ii = 0; ii < poly.Triangles[i].Segments.Length; ii++)
                {
                    //Problems Detecting Segments on circle;
                }
            }
            return false;
        }
        #endregion

        #region Triangle Checking
        private bool intersects(Triangle triangle, Triangle other)
        {
            for (int i = 0; i < triangle.Vertices.Length; i++)
            {
                if (Util.PointInTriangle(triangle.Vertices[i], other.Vertices[0], other.Vertices[1], other.Vertices[2])) return true;

                for (int ii = 0; ii < other.Vertices.Length; ii++)
                {
                    if (other.Segments[i].Intersects(triangle.Segments[ii])) return true;
                }

            }
            return false;
        }

        private bool intersects(Triangle triangle, RectangleOBB rect)
        {
            for(int i = 0; i < triangle.Segments.Length; i++)
            {
                for(int ii = 0; ii < rect.Segments.Length; ii++)
                {
                    if (triangle.Segments[i].Intersects(rect.Segments[ii]))
                    {
                        return true;
                    }
                    if(Util.PointInTriangle(rect.Segments[ii].PointA, triangle.Vertices[0], triangle.Vertices[1], triangle.Vertices[2]))
                    {
                        return true;
                    }
                }
                if(Util.PointInOBB(triangle.Segments[i].PointA, rect.Vertices[0], rect.Vertices[1], rect.Vertices[2], rect.Vertices[3]))
                {
                    return true;
                }
            }
            return false;
        }

        private bool intersects(Triangle triangle, Polygon poly)
        {
            for (int i = 0; i < poly.Triangles.Length; i++)
            {
                if(intersects(triangle, poly.Triangles[i]))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Polygon Checking
        private bool intersects(Polygon poly, Polygon other)
        {
            for (int i = 0; i < poly.Triangles.Length; i++)
            {
                for (int ii = 0; ii < other.Triangles.Length; ii++)
                {
                    if (poly.Triangles[i].Intersects(other.Triangles[ii]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool intersects(Polygon poly, RectangleOBB rect)
        {
            for(int i = 0; i < poly.Triangles.Length; i++)
            {
                if(intersects(poly.Triangles[i], rect))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region RectangleOBB
        private bool intersects(RectangleOBB rect, RectangleOBB other)
        {
            if (Util.PointInOBB(rect.Bounds.Center, other.Vertices[0], other.Vertices[1], other.Vertices[2], other.Vertices[3]))
            {
                return true;
            }
            if (Util.PointInOBB(other.Bounds.Center, rect.Vertices[0], rect.Vertices[1], rect.Vertices[2], rect.Vertices[3]))
            {
                return true;
            }
            for (int i = 0; i < rect.Vertices.Length; i++)
            {
                for(int ii = 0; ii < other.Segments.Length; ii++)
                {
                    if(rect.Segments[i].Intersects(other.Segments[ii]))
                    {
                        return true;
                    }
                }
            }
                return false;
        }
        #endregion

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Debug)
            {
                for (int i = 0; i < boundsSegments.Length; i++)
                {
                    boundsSegments[i].Draw(spriteBatch);
                }
                if (!scaleZero)
                {
                    for (int i = 0; i < segments.Length; i++)
                    {
                        segments[i].Draw(spriteBatch);
                    }
                }
            }
        }
    }
}
