using System;
using Trazable.Engine.Utils;

namespace Trazable.Engine.Base
{
    /// <summary>
    /// Class for an angle.
    /// </summary>
    public class Angle
    {
        #region - Fields -

        private double degree = 0.0;

        private double grad = 0.0;

        private double radian = 0.0;

        private double normalization = 0.0;

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets or sets the angle as degrees.
        /// </summary>
        /// <value>
        /// The degree.
        /// </value>
        public double Degree
        {
            get { return this.degree; }
            set
            {
                this.degree = MathUtil.NormalizeDegree(value, this.normalization);
                this.grad = MathUtil.DegreeToGrad(this.degree);
                this.radian = MathUtil.DegreeToRadian(this.degree);
            }
        }

        /// <summary>
        /// Gets or sets the angle as grads.
        /// </summary>
        /// <value>
        /// The grad.
        /// </value>
        public double Grad
        {
            get { return this.grad; }
            set
            {
                this.Degree = MathUtil.GradToDegree(value);
            }
        }

        /// <summary>
        /// Gets or sets the angle as radians.
        /// </summary>
        /// <value>
        /// The radian.
        /// </value>
        public double Radian
        {
            get { return this.radian; }
            set
            {
                this.Degree = MathUtil.RadianToDegree(value);
            }
        }

        /// <summary>
        /// Gets or sets the normalization (max value over 0 degrees). The value 0 means no normalization.
        /// </summary>
        /// <value>
        /// The normalization.
        /// </value>
        public double Normalization
        {
            get { return this.normalization; }
            set
            {
                this.normalization = value;
                if (this.normalization > 0)
                {
                    this.Degree = MathUtil.NormalizeDegree(degree, this.normalization);
                }
            }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="Angle"/> class.
        /// </summary>
        /// <param name="degree">The degree.</param>
        /// <param name="normalization">The normalization.</param>
        public Angle(double degree, double normalization = 0.0)
        {
            this.normalization = normalization;
            this.Degree = degree;
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Cosine of the angle
        /// </summary>
        /// <returns></returns>
        public double Cos()
        {
            return Math.Cos(this.radian);
        }

        /// <summary>
        /// Cosine of the double of the angle.
        /// </summary>
        /// <returns></returns>
        public double Cos2()
        {
            return Math.Cos(this.radian * 2);
        }

        /// <summary>
        /// Sine of the angle
        /// </summary>
        /// <returns></returns>
        public double Sin()
        {
            return Math.Sin(this.radian);
        }

        /// <summary>
        /// Sine of the double of the angle.
        /// </summary>
        /// <returns></returns>
        public double Sin2()
        {
            return Math.Sin(this.radian * 2);
        }

        /// <summary>
        /// Hyperbolic cosine of the angle.
        /// </summary>
        /// <returns></returns>
        public double Cosh()
        {
            return Math.Cosh(this.radian);
        }

        /// <summary>
        /// Hyperbolic sine of the angle.
        /// </summary>
        /// <returns></returns>
        public double Sinh()
        {
            return Math.Sinh(this.radian);
        }

        /// <summary>
        /// Tangent of the angle.
        /// </summary>
        /// <returns></returns>
        public double Tan()
        {
            return Math.Tan(this.radian);
        }

        /// <summary>
        /// Hyperbolic tangen of the angle.
        /// </summary>
        /// <returns></returns>
        public double Tanh()
        {
            return Math.Tanh(this.radian);
        }

        /// <summary>
        /// Rotates the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="inverse">if set to <c>true</c> [inverse].</param>
        public void Rotate(double value, bool inverse = false)
        {
            if (inverse)
            {
                value = -value;
            }
            this.Degree += value;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public Angle Clone()
        {
            return new Angle(this.Degree, this.Normalization);
        }

        #endregion
    }
}
