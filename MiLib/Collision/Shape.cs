﻿using System;
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

        protected Vector2 position;

        public virtual Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                float width = float.MinValue;
                float height = float.MinValue;
                float x = float.MaxValue;
                float y = float.MaxValue;
                for (int i = 0; i < segments.Length; i++)
                {
                    segments[i].Position = value;
                    vertices[i] = segments[i].PointA;
                    x = vertices[i].X < x ? vertices[i].X : x;
                    y = vertices[i].Y < y ? vertices[i].Y : y;
                    width = vertices[i].X > width ? vertices[i].X : width;
                    height = vertices[i].Y > height ? vertices[i].Y : height;
                }
                new RectangleOBB(x, y, width, height);
            }
        }

        protected Rotation rotation;

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
                    segments[i].Rotation = rotation;
                    vertices[i] = segments[i].PointA;
                    x = vertices[i].X < x ? vertices[i].X : x;
                    y = vertices[i].Y < y ? vertices[i].Y : y;
                    width = vertices[i].X > width ? vertices[i].X : width;
                    height = vertices[i].Y > height ? vertices[i].Y : height;
                }
                width -= x;
                height -= y;
                Bounds = new RectangleOBB(x, y, width, height);
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

        protected float scale;
        //0 Scale breaks draw
        public virtual float Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
                float width = float.MinValue;
                float height = float.MinValue;
                float x = float.MaxValue;
                float y = float.MaxValue;
                for(int i = 0; i < segments.Length; i++)
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
                Bounds = new RectangleOBB(x, y, width, height);
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
                Bounds = new RectangleOBB(x, y, width, height);
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

        protected RectangleOBB bounds;

        public virtual RectangleOBB Bounds
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
        public Shape(GraphicsDevice graphicsDevice)
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
            bounds = new RectangleOBB();
            Debug = false;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if(Debug)
            {
                for(int i = 0; i < boundsSegments.Length; i++)
                {
                    boundsSegments[i].Draw(spriteBatch);
                }

                for(int i = 0; i < segments.Length; i++)
                {
                    if(segments[i] != null)
                    {
                        segments[i].Draw(spriteBatch);
                    }
                }
            }
        }
    }
}
