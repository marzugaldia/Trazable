using System;

namespace Trazable.Engine.Utils
{
    /// <summary>
    /// Math utilities.
    /// </summary>
    public static class MathUtil
    {
        #region - Fields -

        private const double DegreeToGradFactor = 10.0 / 9.0;

        private const double GradToDegreeFactor = 9.0 / 10.0;

        private const double DegreeToRadianFactor = Math.PI / 180.0;

        private const double RadianToDegreeFactor = 180.0 / Math.PI;

        private const double GradToRadianFactor = Math.PI / 200.0;

        private const double RadianToGradFactor = 200.0 / Math.PI;

        private const int MaxDecimals = 12;

        #endregion

        #region - Methods -

        /// <summary>
        /// Removes decimal part from the given decimal position.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="maxDecimals">The maximum decimals.</param>
        /// <returns></returns>
        public static double RemoveDecimals(double value, int maxDecimals = MaxDecimals)
        {
            return Math.Round(value, maxDecimals, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Converts degrees to gradians.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static double DegreeToGrad(double value)
        {
            return RemoveDecimals(value * DegreeToGradFactor);
        }

        /// <summary>
        /// Converts gradians to degrees.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static double GradToDegree(double value)
        {
            return RemoveDecimals(value * GradToDegreeFactor);
        }

        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static double DegreeToRadian(double value)
        {
            return RemoveDecimals(value * DegreeToRadianFactor);
        }

        /// <summary>
        /// Converts radians to degrees.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static double RadianToDegree(double value)
        {
            return RemoveDecimals(value * RadianToDegreeFactor);
        }

        /// <summary>
        /// Converts gradians to radians.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static double GradToRadian(double value)
        {
            return RemoveDecimals(value * GradToRadianFactor);
        }

        /// <summary>
        /// Converts radians to gradians.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static double RadianToGrad(double value)
        {
            return RemoveDecimals(value * RadianToGradFactor);
        }

        /// <summary>
        /// Normalizes degrees.
        /// </summary>
        /// <param name="degree">The degree.</param>
        /// <param name="max">The maximum.</param>
        /// <returns></returns>
        public static double NormalizeDegree(double degree, double max = 360.0)
        {
            double value = Math.IEEERemainder(degree, 360.0);
            if (max <= 0)
            {
                return degree;
            }
            while (value <= 0.0)
            {
                value += max;
            }
            while (value > max)
            {
                value -= max;
            }
            return value;
        }

        /// <summary>
        /// Indicates if two numbers are almost equal, using the given decimal places.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <param name="decimalPlaces">The decimal places.</param>
        /// <returns></returns>
        public static bool AlmostEqual(this double a, double b, int decimalPlaces= MaxDecimals)
        {
            if (double.IsNaN(a) || double.IsNaN(b))
            {
                return false;
            }
            if (double.IsInfinity(a) || double.IsInfinity(b))
            {
                return a == b;
            }
            return Math.Abs(a - b) < Math.Pow(10, -decimalPlaces) / 2d;
        }

        /// <summary>
        /// Determines whether the specified a is infinity.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <returns></returns>
        public static bool IsInfinity(double a, double maxValue = 100000.0)
        {
            return double.IsInfinity(a) || Math.Abs(a) > maxValue;
        }

        #endregion
    }
}
