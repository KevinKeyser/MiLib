using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiLib.CoreTypes;

namespace MiLib.Collision
{
    public class Segment
    {
        private Vector2 pointA;
        public Vector2 PointA
        {
            get
            {
                return pointA;
            }
            set
            {
                pointA = value;
                lengthOA = (origin - pointA).Length();
                lengthAB = (pointB - pointA).Length();
                angleOA = Util.VectorToAngle(origin - pointA) - MathHelper.PiOver2;
                angleAB = Util.VectorToAngle(pointB - pointA) - MathHelper.PiOver2;
            }
        }
        private Vector2 pointB;
        public Vector2 PointB
        {
            get
            {
                return pointB;
            }
            set
            {
                pointB = value;
                lengthOB = (origin - pointB).Length();
                lengthAB = (pointB - pointA).Length();
                angleOB = Util.VectorToAngle(origin - pointB) - MathHelper.PiOver2;
                angleAB = Util.VectorToAngle(pointB - pointA) - MathHelper.PiOver2;
            }
        }
        private Vector2 origin;
        public Vector2 Origin
        {
            get
            {
                return origin;
            }
            set
            {
                origin = value;
                lengthOA = (origin - pointA).Length();
                lengthOB = (origin - pointB).Length();
                angleOA = Util.VectorToAngle(origin - pointA) - MathHelper.PiOver2;
                angleOB = Util.VectorToAngle(origin - pointB) - MathHelper.PiOver2;
            }
        }

        public Vector2 Position
        {
            get
            {
                return origin;
            }
            set
            {
                Vector2 oldorigin = value - origin;
                origin = value;
                PointA += oldorigin;
                PointB += oldorigin;
            }
        }
        private float scale;

        public float Scale
        {
            get
            {
                return scale;
            }
            set
            {
                lengthOA /= scale;
                lengthOB /= scale;
                scale = value;
                lengthOA *= value;
                lengthOB *= value;
                Vector2 angleVector = Util.AngleToVector(angleOA + MathHelper.PiOver2);
                angleVector.Normalize();
                pointA = origin - angleVector * lengthOA;

                angleVector = Util.AngleToVector(angleOB + MathHelper.PiOver2);
                angleVector.Normalize();
                pointB = origin - angleVector * lengthOB;
                lengthAB = (pointB - pointA).Length();
                lineScale = new Vector2(lengthAB, 1f);
            }
        }

        private float rotation;

        public float Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                float rot = rotation;
                rotation = value;
                angleOA += (value - rot);
                angleOB += (value - rot);
                angleAB += (value - rot);

                Vector2 angleVector = Util.AngleToVector(angleOA + MathHelper.PiOver2);
                angleVector.Normalize();
                pointA = origin - angleVector * lengthOA;

                angleVector = Util.AngleToVector(angleOB + MathHelper.PiOver2);
                angleVector.Normalize();
                pointB = origin - angleVector * lengthOB;
            }
        }

        private float angleOA;
        private float angleOB;
        private float angleAB;
        private float lengthOA;
        private float lengthOB;
        private float lengthAB;
        private Vector2 lineScale;
        private Texture2D texture;

        public Segment(Vector2 pointA, Vector2 pointB, GraphicsDevice graphics)
        : this(pointA, pointB, pointA, graphics) {}
        public Segment(Vector2 pointA, Vector2 pointB, Vector2 origin, GraphicsDevice graphics)
        {
            texture = new Texture2D(graphics, 1, 1);
            texture.SetData<Color>(new Color[] { Color.White });
            this.pointA = pointA;
            this.pointB = pointB;
            this.origin = origin;
            scale = 1f;
            lengthOA = (origin - pointA).Length();
            lengthOB = (origin - pointB).Length();
            lengthAB = (pointB - pointA).Length();
            angleOA = Util.VectorToAngle(origin - pointA) - MathHelper.PiOver2;
            angleOB = Util.VectorToAngle(origin - pointB) - MathHelper.PiOver2;
            angleAB = Util.VectorToAngle(pointB - pointA) - MathHelper.PiOver2;
            lineScale = new Vector2(lengthAB, 1f);
        }

        private bool colinearOnSegment(Vector2 p, Vector2 q, Vector2 r)
        {
            if (q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min(p.X, r.X) &&
                q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y))
                return true;

            return false;
        }
        public bool Intersects(Segment seg)
        {
            Order o1 = Util.TriangleOrientation(pointA, pointB, seg.pointA);
            Order o2 = Util.TriangleOrientation(pointA, pointB, seg.pointB);
            Order o3 = Util.TriangleOrientation(seg.pointA, seg.pointB, pointA);
            Order o4 = Util.TriangleOrientation(seg.pointA, seg.pointB, pointB);

            if (o1 != o2 && o3 != o4 ||
                o1 == Order.Colinear && colinearOnSegment(pointA, seg.pointA, pointB) ||
                o2 == Order.Colinear && colinearOnSegment(pointA, seg.pointB, pointB) ||
                o3 == Order.Colinear && colinearOnSegment(seg.pointA, pointA, seg.pointB) ||
                o4 == Order.Colinear && colinearOnSegment(seg.pointA, pointB, seg.pointB)) return true;
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, pointA, null, Color.Red, angleAB, Vector2.Zero, lineScale, SpriteEffects.None, 0f);
            spriteBatch.Draw(texture, pointA, Color.Blue);
            spriteBatch.Draw(texture, pointB, Color.Blue);
            spriteBatch.Draw(texture, origin, Color.Green);
        }
    }
}
