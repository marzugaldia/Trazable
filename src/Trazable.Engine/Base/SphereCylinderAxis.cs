namespace Trazable.Engine.Base
{
    /// <summary>
    /// Representation as a Sphere Cylinder Axis
    /// </summary>
    public class SphereCylinderAxis
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the sphere.
        /// </summary>
        /// <value>
        /// The sphere.
        /// </value>
        public double Sphere { get; set; }

        /// <summary>
        /// Gets or sets the cylinder.
        /// </summary>
        /// <value>
        /// The cylinder.
        /// </value>
        public double Cylinder { get; set; }

        /// <summary>
        /// Gets or sets the axis.
        /// </summary>
        /// <value>
        /// The axis.
        /// </value>
        public Angle Axis { get; set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="SphereCylinderAxis"/> class.
        /// </summary>
        /// <param name="sphere">The sphere.</param>
        /// <param name="cylinder">The cylinder.</param>
        /// <param name="axis">The axis.</param>
        public SphereCylinderAxis(double sphere, double cylinder, Angle axis)
        {
            this.Sphere = sphere;
            this.Cylinder = cylinder;
            this.Axis = axis;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SphereCylinderAxis"/> class.
        /// </summary>
        /// <param name="sphere">The sphere.</param>
        /// <param name="cylinder">The cylinder.</param>
        /// <param name="degree">The degree.</param>
        public SphereCylinderAxis(double sphere, double cylinder, double degree)
            : this(sphere, cylinder, new Angle(degree, 180))
        {

        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Reverses this instance.
        /// </summary>
        public void Reverse()
        {
            this.Sphere += this.Cylinder;
            this.Cylinder = -this.Cylinder;
            this.Axis.Rotate(90);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}   {1}   {2}", this.Sphere, this.Cylinder, this.Axis.Degree);
        }

        #endregion
    }
}
