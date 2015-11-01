using Trazable.Engine.Utils;

namespace Trazable.Engine.Base
{
    /// <summary>
    /// Astigmatic decomposition
    /// Read: http://openaccess.city.ac.uk/2726/1/OVS_Final_Submitted_Standardised_pats_to_assess_Rx_reproducibility.pdf
    /// Given a Sphere S, Cylinder C and Axis θ:
    ///     C0 = C*cos(2*θ)
    ///     C45 = C*sin(2*θ)
    /// So: 
    ///     C=sqrt(C0^2+C45^2)
    ///
    /// The spherical equivalent power M is the algebraic mean of the two principal powers S and (S+C) such that:
    ///     M = S + (C/2)
    /// 
    /// For any given optical prescription, the total sphero-cylindrical power can be represented by a single scalar quantity u: 
    ///     u = sqrt(M^2+C0^2+C45^2)
    /// </summary>
    public class AstigmaticDecomposition
    {
        #region - Properties -

        /// <summary>
        /// Spherical equivalent power
        /// </summary>
        /// <value>
        /// The spherical equivalent power M = S+(C/2)
        /// </value>
        public double M { get; set; }

        /// <summary>
        /// Gets or sets the c0.
        /// </summary>
        /// <value>
        /// The C0. Given the cylinder C and Axis θ, then C0 = C*cos(2*θ)
        /// </value>
        public double C0 { get; set; }

        /// <summary>
        /// Gets or sets the C45.
        /// </summary>
        /// <value>
        /// The C45. Given the cylinder C and Axis θ, then C45 = C*sin(2*θ)
        /// </value>
        public double C45 { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is infinity.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is infinity; otherwise, <c>false</c>.
        /// </value>
        public bool IsInfinity
        {
            get
            {
                return MathUtil.IsInfinity(this.M) || MathUtil.IsInfinity(this.C0) || MathUtil.IsInfinity(this.C45);
            }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="AstigmaticDecomposition"/> class.
        /// </summary>
        /// <param name="m">The m.</param>
        /// <param name="c0">The c0.</param>
        /// <param name="c45">The C45.</param>
        public AstigmaticDecomposition(double m, double c0, double c45)
        {
            this.M = m;
            this.C0 = c0;
            this.C45 = c45;
        }

        #endregion

        #region - Operators -

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static AstigmaticDecomposition operator +(AstigmaticDecomposition a, AstigmaticDecomposition b)
        {
            return new AstigmaticDecomposition(a.M + b.M, a.C0 + b.C0, a.C45 + b.C45);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static AstigmaticDecomposition operator -(AstigmaticDecomposition a, AstigmaticDecomposition b)
        {
            return new AstigmaticDecomposition(a.M - b.M, a.C0 - b.C0, a.C45 - b.C45);
        }

        /// <summary>
        /// Implements the operator * for scalar product.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static AstigmaticDecomposition operator *(AstigmaticDecomposition a, double b)
        {
            return new AstigmaticDecomposition(a.M * b, a.C0 * b, a.C45 * b);
        }

        /// <summary>
        /// Implements the operator * for scalar product.
        /// </summary>
        /// <param name="b">The b.</param>
        /// <param name="a">a.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static AstigmaticDecomposition operator *(double b, AstigmaticDecomposition a)
        {
            return a * b;
        }

        /// <summary>
        /// Implements the operator / for scalar division.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static AstigmaticDecomposition operator /(AstigmaticDecomposition a, double b)
        {
            return new AstigmaticDecomposition(a.M / b, a.C0 / b, a.C45 / b);
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Calculate the effectivity of a power over a distance.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="distance">The distance.</param>
        /// <param name="n">The n.</param>
        /// <returns></returns>
        public double Effectivity(double p, double distance, double n)
        {
            if (n == 0.0)
            {
                return double.PositiveInfinity;
            }
            double result = 1.0 - distance * p / n;
            return result == 0.0 ? double.PositiveInfinity : p / result;
        }


        /// <summary>
        /// Calculate the effectivity of this astigmatic decomposition over a distance.
        /// </summary>
        /// <param name="distance">The distance.</param>
        /// <param name="n">The n.</param>
        /// <returns></returns>
        public AstigmaticDecomposition Effectivity(double distance, double n)
        {
            PowersAndMeridians pam = this.ToPowerAndMeridians();
            pam.Power1 = this.Effectivity(pam.Power1, distance, n);
            pam.Power2 = this.Effectivity(pam.Power2, distance, n);
            return pam.ToAstigmaticDecomposition();
        }

        /// <summary>
        /// Propagates the astigmatic decomposition over a distance.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="n">The n.</param>
        /// <returns></returns>
        public AstigmaticDecomposition Propagate(double d, double n = 1.0)
        {
            if (double.IsInfinity(n))
            {
                return null;
            }
            if (d == 0.0 )
            {
                return this.Clone();
            }
            if (this.IsInfinity || double.IsInfinity(d))
            {
                return new AstigmaticDecomposition(-1000.0 * n / d, 0, 0);
            }
            return this.Effectivity(d, n * 1000.0);
        }

        /// <summary>
        /// Refractes this astigmatic decomposition with a target one.
        /// </summary>
        /// <param name="tgt">The TGT.</param>
        /// <returns></returns>
        public AstigmaticDecomposition Refracte(AstigmaticDecomposition tgt)
        {
            return this + tgt;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public AstigmaticDecomposition Clone()
        {
            return new AstigmaticDecomposition(this.M, this.C0, this.C45);
        }

        #endregion
    }
}
