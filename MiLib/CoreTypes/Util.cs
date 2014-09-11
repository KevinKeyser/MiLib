using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiLib.CoreTypes
{
    public enum Order
    {
        Colinear,
        ClockWise,
        CounterClockWise
    }
    public static class Util
    {
         public static float VectorToAngle(Vector2 vector, float initialOffsetAngle = 0.0f)
        {
            return (float)Math.Atan2(vector.X, -vector.Y) + initialOffsetAngle;
        }
        public static Vector2 AngleToVector(float angle)
        {
            return new Vector2((float)Math.Sin(angle), -(float)Math.Cos(angle));
        }
        public static Order TriangleOrientation(Vector2 point1, Vector2 point2, Vector2 point3)
        {
            float val = (point2.Y - point1.Y) * (point3.X - point2.X) - (point2.X - point1.X) * (point3.Y - point2.Y);

            if (val == 0) return Order.Colinear;

            return (val > 0) ? Order.ClockWise : Order.CounterClockWise;
        }

        public static float Cross2D(Vector2 u, Vector2 v)
        {
            return u.Y * v.X - u.X * v.Y;
        }

        public static bool PointInTriangle(Vector2 point, Vector2 A, Vector2 B, Vector2 C)
        {
            if (Cross2D(point - A, B - A) > 0.0f) return false;
            if (Cross2D(point - B, C - B) > 0.0f) return false;
            if (Cross2D(point - C, A - C) > 0.0f) return false;
            return true;
        }

        public static Vector2? ClosestPoint(Vector2 point, Vector2[] points)
        {
            Vector2? closest = null;

            for(int i = 0; i < points.Length; i++)
            {
                if(!closest.HasValue)
                {
                    closest = points[i];
                }
                else
                {
                    if(Vector2.DistanceSquared(point, closest.Value) >
                        Vector2.DistanceSquared(point, points[i]))
                    {
                        closest = points[i];
                    }
                }
            }
            return closest;
        }

        public static Vector2? FurthestPoint(Vector2 point, Vector2[] points)
        {
            Vector2? furthest = null;

            for (int i = 0; i < points.Length; i++)
            {
                if (!furthest.HasValue)
                {
                    furthest = points[i];
                }
                else
                {
                    if (Vector2.DistanceSquared(point, furthest.Value) <
                        Vector2.DistanceSquared(point, points[i]))
                    {
                        furthest = points[i];
                    }
                }
            }
            return furthest;
        }

        public static bool Intersects(this Rectangle rect, Vector2 point)
        {
            if (point.X >= rect.X && point.X <= rect.X + rect.Width &&
                point.Y >= rect.Y && point.Y <= rect.Y + rect.Height)
                return true;
            return false;
        }

        public static float NextDegree(this Random random, float min, float max)
        {
            if (min >= max) { throw new ArgumentOutOfRangeException("min cannot be greater than max"); }
            return (float)(random.NextDouble() * (max - min) + min) % 360;
        }

        public static float NextRadian(this Random random, float min, float max)
        {
            if (min >= max) { throw new ArgumentOutOfRangeException("min cannot be greater than max"); }
            return (float)((random.NextDouble() * (max - min) + min) % Math.PI);
        }

        public static Vector2 NextVector2(this Random random, Vector2 min, Vector2 max)
        {
            if (min.X >= max.X) { throw new ArgumentOutOfRangeException("min.X cannot be greater than max.X"); }
            if (min.Y >= max.Y) { throw new ArgumentOutOfRangeException("min.Y cannot be greater than max.Y"); }
            return new Vector2(random.NextFloat() * (max.X - min.X) + min.X, random.NextFloat() * (max.Y - min.Y) + min.Y);
        }

        public static Vector2 Normalize(Vector2 vector2)
        {
            vector2.Normalize();
            return vector2;
        }
        /// <summary>
        /// Returns a random boolean
        /// </summary>
        /// <param name="random">Random object</param>
        /// <returns>A randomly generated boolean value</returns>
        public static bool NextBoolean(this Random random)
        {
            return random.NextDouble() > 0.5;
        }

        /// <summary>
        /// Returns a random float
        /// </summary>
        /// <param name="random"></param>
        /// <returns></returns>
        public static float NextFloat(this Random random)
        {
            return (float)random.NextDouble();
        }
        public static float NextFloat(this Random random, float min, float max)
        {
            if (min >= max)
            {
                throw new ArgumentOutOfRangeException("Min value cannot be greater than Max value");
            }
            return (float)(random.NextDouble() * (max - min) + min);
        }

        public static TimeSpan NextTimeSpan(this Random random, TimeSpan min, TimeSpan max)
        {
            if (min >= max)
            {
                throw new ArgumentOutOfRangeException("Min value cannot be greater than Max value");
            }

            return TimeSpan.FromMilliseconds(random.NextDouble() * (max - min).TotalMilliseconds + min.TotalMilliseconds);
        }
    }
}
