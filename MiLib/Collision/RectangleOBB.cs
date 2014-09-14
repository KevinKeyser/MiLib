using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiLib.Collision
{
    public class RectangleOBB
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

        public RectangleOBB() : this(0, 0, 0, 0) { }
        public RectangleOBB(float x, float y, float width, float height)
        {
            position = new Vector2(x, y);
            size = new Vector2(width, height);
        }

        [Obsolete]
        public Rectangle ToRectangle()
        {
            return (Rectangle)this;
        }

        public static explicit operator Rectangle(RectangleOBB rect)
        {
            if (rect == null)
            {
                throw new NullReferenceException();
            }

            return new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
        }
    }
}
