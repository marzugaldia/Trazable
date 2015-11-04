namespace Trazable.Engine.Base
{
    /// <summary>
    /// Class for an optical representation.
    /// </summary>
    public class OpticalRepresentation
    {
        #region - Fields -

        private AstigmaticDecomposition ad;

        private SphereCylinderAxis sca;

        private PowersAndMeridians pam;

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets or sets the astigmatic decomposition.
        /// </summary>
        /// <value>
        /// The astigmatic decomposition.
        /// </value>
        public AstigmaticDecomposition AstigmaticDecomposition
        {
            get { return this.ad; }
            set
            {
                this.SetAstigmaticDecomposition(value);
            }
        }

        /// <summary>
        /// Gets or sets the sphere cylinder axis.
        /// </summary>
        /// <value>
        /// The sphere cylinder axis.
        /// </value>
        public SphereCylinderAxis SphereCylinderAxis
        {
            get { return this.sca; }
            set
            {
                this.SetSphereCylinderAxis(value);
            }
        }

        /// <summary>
        /// Gets or sets the powers and meridians.
        /// </summary>
        /// <value>
        /// The powers and meridians.
        /// </value>
        public PowersAndMeridians PowersAndMeridians
        {
            get { return this.pam; }
            set
            {
                this.SetPowersAndMeridians(value);
            }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="OpticalRepresentation"/> class.
        /// </summary>
        public OpticalRepresentation()
            : this(0,0,0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpticalRepresentation"/> class.
        /// </summary>
        /// <param name="sphere">The sphere.</param>
        /// <param name="cylinder">The cylinder.</param>
        /// <param name="axisDegrees">The axis degrees.</param>
        public OpticalRepresentation(double sphere, double cylinder, double axisDegrees)
            : this(new SphereCylinderAxis(sphere, cylinder, axisDegrees))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpticalRepresentation"/> class.
        /// </summary>
        /// <param name="power1">The power1.</param>
        /// <param name="axis1">The axis1.</param>
        /// <param name="power2">The power2.</param>
        /// <param name="axis2">The axis2.</param>
        public OpticalRepresentation(double power1, double axis1, double power2, double axis2)
            : this(new PowersAndMeridians(power1, axis1, power2,axis2))
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpticalRepresentation"/> class.
        /// </summary>
        /// <param name="sca">The sca.</param>
        public OpticalRepresentation(SphereCylinderAxis sca)
        {
            this.SphereCylinderAxis = sca;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpticalRepresentation"/> class.
        /// </summary>
        /// <param name="pam">The pam.</param>
        public OpticalRepresentation(PowersAndMeridians pam)
        {
            this.PowersAndMeridians = pam;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpticalRepresentation"/> class.
        /// </summary>
        /// <param name="ad">The ad.</param>
        public OpticalRepresentation(AstigmaticDecomposition ad)
        {
            this.AstigmaticDecomposition = ad;
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Sets the sphere cylinder axis.
        /// </summary>
        /// <param name="sca">The sca.</param>
        private void SetSphereCylinderAxis(SphereCylinderAxis sca)
        {
            this.sca = sca;
            this.ad = sca.ToAstigmaticDecomposition();
            this.pam = sca.ToPowerAndMeridians();
        }

        /// <summary>
        /// Sets the powers and meridians.
        /// </summary>
        /// <param name="pam">The pam.</param>
        private void SetPowersAndMeridians(PowersAndMeridians pam)
        {
            this.pam = pam;
            this.ad = pam.ToAstigmaticDecomposition();
            this.sca = pam.ToSphereCylinderAxis();
        }

        /// <summary>
        /// Sets the astigmatic decomposition.
        /// </summary>
        /// <param name="ad">The ad.</param>
        private void SetAstigmaticDecomposition(AstigmaticDecomposition ad)
        {
            this.ad = ad;
            this.pam = ad.ToPowerAndMeridians();
            this.sca = ad.ToSphereCylinderAxis();
        }

        /// <summary>
        /// Propagate operation.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="n">The n.</param>
        /// <returns></returns>
        public OpticalRepresentation Propagate(double d, double n = 1)
        {
            return new OpticalRepresentation(this.ad.Propagate(d, n));
        }

        /// <summary>
        /// Inverse propagation operation.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="n">The n.</param>
        /// <returns></returns>
        public OpticalRepresentation PropagateReverse(double d, double n = 1)
        {
            return new OpticalRepresentation(this.ad.PropagateReverse(d, n));
        }

        /// <summary>
        /// Refracte operation.
        /// </summary>
        /// <param name="tgt">The TGT.</param>
        /// <returns></returns>
        public OpticalRepresentation Refracte(OpticalRepresentation tgt)
        {
            return new OpticalRepresentation(this.ad.Refracte(tgt.AstigmaticDecomposition));
        }

        /// <summary>
        /// Inverse propagation operation.
        /// </summary>
        /// <param name="tgt">The TGT.</param>
        /// <returns></returns>
        public OpticalRepresentation RefracteReverse(OpticalRepresentation tgt)
        {
            return new OpticalRepresentation(this.ad.RefracteReverse(tgt.AstigmaticDecomposition));
        }

        #endregion
    }
}
