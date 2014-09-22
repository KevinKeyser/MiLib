using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiLib.Collision
{
    public class RectangleAABB
    {
        private Vector2 position;
        public float X
        {
            get { return position.X; }
            set { position.X = value; }
        }

        public float Y
        {
            get { return position.Y; }
            set { position.Y = value; }
        }

        private Vector2 size;

        public float Width
        {
            get { return size.X; }
            set { size.X = value; }
        }

        public float Height
        {
            get { return size.Y; }
            set { size.Y = value; }
        }

        private Vector2 center;
        public virtual Vector2 Center
        {
            get
            {
                return center;
            }
            protected set
            {
                center = value;
            }
        }
        public RectangleAABB() : this(0, 0, 0, 0) { }
        public RectangleAABB(float x, float y, float width, float height)
        {
            position = new Vector2(x, y);
            size = new Vector2(width, height);
            center = new Vector2(x + width / 2, y + height / 2);
        }

        public bool Intersects(RectangleAABB rect)
        {
            if (X + Width < rect.X) return false;
            if (Y + Height < rect.Y) return false;
            if (rect.X + rect.Width < X) return false;
            if (rect.Y + rect.Height < Y) return false;
            return true;
        }

        [Obsolete]
        public Rectangle ToRectangle()
        {
            return (Rectangle)this;
        }

        public static explicit operator Rectangle(RectangleAABB rect)
        {
            if (rect == null)
            {
                throw new NullReferenceException();
            }

            return new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
        }
    }
}
