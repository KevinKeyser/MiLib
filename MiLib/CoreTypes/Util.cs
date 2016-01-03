using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

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
        public static Vector2 ToVector2(this Vector3 vector3)
        {
            return new Vector2(vector3.X, vector3.Y);
        }

        public static Vector3 ToVector3(this Vector2 vector2, float z = 0)
        {
            return new Vector3(vector2.X, vector2.Y, z);
        }

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

        public static bool PointInOBB(Vector2 point, Vector2 A, Vector2 B, Vector2 C, Vector2 D)
        {
            if (Cross2D(point - A, B - A) > 0.0f) return false;
            if (Cross2D(point - B, C - B) > 0.0f) return false;
            if (Cross2D(point - C, D - C) > 0.0f) return false;
            if (Cross2D(point - D, A - D) > 0.0f) return false;
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

        public static bool Contains(this Rectangle rect, Vector2 point)
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
            return (float)((random.NextDouble() * (max - min) + min) % (Math.PI * 2));
        }

        public static Vector2 NextVector2(this Random random, Vector2 min, Vector2 max)
        {
            if (min.X >= max.X) { throw new ArgumentOutOfRangeException("min.X cannot be greater than max.X"); }
            if (min.Y >= max.Y) { throw new ArgumentOutOfRangeException("min.Y cannot be greater than max.Y"); }
            return new Vector2(random.NextFloat() * (max.X - min.X) + min.X, random.NextFloat() * (max.Y - min.Y) + min.Y);
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

        public static Vector2 Normalize(Vector2 vector2)
        {
            Vector2 normal = new Vector2(vector2.X, vector2.Y);
            normal.Normalize();
            return normal;
        }

        private static Dictionary<int, Vector2[]> circlePositions = new Dictionary<int, Vector2[]>();

        public static Vector2[] GetCirclePositions(int circumferencePoints)
        {
            Vector2[] points;
            if(!circlePositions.ContainsKey(circumferencePoints))
            {
                points = new Vector2[circumferencePoints];
                float rotAdd = (float)Math.PI / points.Length * 2;
                for (int i = 0; i < points.Length; i++)
                {
                    points[i] = new Vector2((float)Math.Sin(rotAdd * i), (float)Math.Cos(rotAdd * i));
                }
                circlePositions.Add(circumferencePoints, points);
                return points;
            }
            return circlePositions[circumferencePoints];
        }

    }
}
