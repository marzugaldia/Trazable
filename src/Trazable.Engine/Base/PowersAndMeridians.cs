namespace Trazable.Engine.Base
{
    /// <summary>
    /// Class representing the powers and meridians information of a lens.
    /// </summary>
    public class PowersAndMeridians
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the first power.
        /// </summary>
        /// <value>
        /// The power1.
        /// </value>
        public double Power1 { get; set; }

        /// <summary>
        /// Gets or sets the first meridian.
        /// </summary>
        /// <value>
        /// The meridian1.
        /// </value>
        public Angle Meridian1 { get; set; }

        /// <summary>
        /// Gets or sets the second power.
        /// </summary>
        /// <value>
        /// The power2.
        /// </value>
        public double Power2 { get; set; }

        /// <summary>
        /// Gets or sets the second meridian.
        /// </summary>
        /// <value>
        /// The meridian2.
        /// </value>
        public Angle Meridian2 { get; set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="PowersAndMeridians"/> class.
        /// </summary>
        /// <param name="power1">The power1.</param>
        /// <param name="meridian1">The meridian1.</param>
        /// <param name="power2">The power2.</param>
        /// <param name="meridian2">The meridian2.</param>
        public PowersAndMeridians(double power1, Angle meridian1, double power2, Angle meridian2)
        {
            this.Power1 = power1;
            this.Meridian1 = meridian1;
            this.Power2 = power2;
            this.Meridian2 = meridian2;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PowersAndMeridians"/> class.
        /// </summary>
        /// <param name="power1">The power1.</param>
        /// <param name="meridianDegree1">The meridian degree1.</param>
        /// <param name="power2">The power2.</param>
        /// <param name="meridianDegree2">The meridian degree2.</param>
        public PowersAndMeridians(double power1, double meridianDegree1, double power2, double meridianDegree2)
            : this(power1, new Angle(meridianDegree1, 180), power2, new Angle(meridianDegree2, 180))
        {
        }

        #endregion
    }
}
