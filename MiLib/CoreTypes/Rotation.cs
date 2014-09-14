using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiLib.CoreTypes
{
    public enum AngleMeasure
    {
        Degrees,
        Radians
    }
    public struct Rotation : IEquatable<Rotation>
    {
        public float Angle;

        private AngleMeasure measurements;

        public AngleMeasure Measurements
        {
            get { return measurements; }
            set
            {
                if (measurements != value)
                {
                    measurements = value;
                    Angle = measurements == AngleMeasure.Degrees ? MathHelper.ToDegrees(this.Angle) : MathHelper.ToRadians(this.Angle);
                }
            }
        }
        
        public Rotation(float angle) : this(angle, AngleMeasure.Radians) { }
        public Rotation(float angle, AngleMeasure measurements)
        {
            this.measurements = measurements;
            Angle = angle;
        }

        public float AsDegrees()
        {
            return measurements == AngleMeasure.Degrees ? Angle : MathHelper.ToDegrees(Angle);
        }

        public float AsRadians()
        {
            return measurements == AngleMeasure.Radians ? Angle : MathHelper.ToRadians(Angle);
        }
        
        public static Rotation operator +(Rotation rotation, float addition)
        {
            return new Rotation(rotation.Angle + addition);
        }

        public static Rotation operator -(Rotation rotation, float subtraction)
        {
            return new Rotation(rotation.Angle - subtraction);
        }

        public static Rotation operator *(Rotation rotation, float multiply)
        {
            return new Rotation(rotation.Angle * multiply);
        }

        public static Rotation operator /(Rotation rotation, float divide)
        {
            return new Rotation(rotation.Angle * divide);
        }

        public static Rotation operator +(Rotation rotation1, Rotation rotation2)
        {
            return new Rotation(rotation1.Angle + (rotation1.Measurements == AngleMeasure.Degrees ? rotation2.AsDegrees() : rotation2.AsRadians()));
        }

        public static Rotation operator -(Rotation rotation1, Rotation rotation2)
        {
            return new Rotation(rotation1.Angle - (rotation1.Measurements == AngleMeasure.Degrees ? rotation2.AsDegrees() : rotation2.AsRadians()));
        }

        public static Rotation operator *(Rotation rotation1, Rotation rotation2)
        {
            return new Rotation(rotation1.Angle * (rotation1.Measurements == AngleMeasure.Degrees ? rotation2.AsDegrees() : rotation2.AsRadians()));
        }

        public static Rotation operator /(Rotation rotation1, Rotation rotation2)
        {
            return new Rotation(rotation1.Angle * (rotation1.Measurements == AngleMeasure.Degrees ? rotation2.AsDegrees() : rotation2.AsRadians()));
        }
        
        public static bool operator ==(Rotation rotation1, Rotation rotation2)
        {
            return rotation1.AsRadians() == rotation2.AsRadians();
        }

        public static bool operator !=(Rotation rotation1, Rotation rotation2)
        {
            return rotation1.AsRadians() != rotation2.AsRadians();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return Measurements.ToString() + ": " + Angle.ToString();
        }

        public bool Equals(Rotation other)
        {
            if (other == this) return true;
            else return false;
        }
    }
}
