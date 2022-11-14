using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ComputerGraphics.Helpers
{
    static public class VectorExtensions
    {
        public static Vector Rotate(this Vector start, double angle)
        {
            float newX = (float)(start.X * Math.Cos(angle) - start.Y * Math.Sin(angle));
            float newY = (float)(start.X * Math.Sin(angle) + start.Y * Math.Cos(angle));
            return new Vector(newX, newY);
        }
        public static double Angle(this Vector first) => Math.Atan2(first.Y, first.X);

        public static bool IsSamePoint(this Vector point, Vector otherPoint, double precision)
        {
            double diffX = point.X - otherPoint.X;
            double diffY = point.Y - otherPoint.Y;
            Func<double, bool> IsDiffAroundZero = (diff) => diff > -precision && diff < precision;
            return IsDiffAroundZero(diffX) && IsDiffAroundZero(diffY);
        }
    }
}
