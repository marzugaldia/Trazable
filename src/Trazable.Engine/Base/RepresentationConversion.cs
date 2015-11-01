using System;

namespace Trazable.Engine.Base
{
    public static class RepresentationConversion
    {
        /// <summary>
        /// Converto to power and meridians.
        /// </summary>
        /// <param name="sca">The sca.</param>
        /// <returns></returns>
        public static PowersAndMeridians ToPowerAndMeridians(this SphereCylinderAxis sca)
        {
            return new PowersAndMeridians(sca.Sphere, sca.Axis.Degree, sca.Sphere + sca.Cylinder, sca.Axis.Degree + 90);
        }

        /// <summary>
        /// Convert to power and meridians.
        /// </summary>
        /// <param name="ad">The ad.</param>
        /// <returns></returns>
        public static PowersAndMeridians ToPowerAndMeridians(this AstigmaticDecomposition ad)
        {
            return ToPowerAndMeridians(ad.ToSphereCylinderAxis());
        }

        /// <summary>
        /// Convert to sphere cylinder axis.
        /// </summary>
        /// <param name="pam">The pam.</param>
        /// <returns></returns>
        public static SphereCylinderAxis ToSphereCylinderAxis(this PowersAndMeridians pam)
        {
            return new SphereCylinderAxis(pam.Power1, pam.Power2 - pam.Power1, pam.Meridian1.Degree);
        }

        /// <summary>
        /// Convert to sphere cylinder axis.
        /// </summary>
        /// <param name="ad">The ad.</param>
        /// <param name="cminus">if set to <c>true</c> [cminus].</param>
        /// <returns></returns>
        public static SphereCylinderAxis ToSphereCylinderAxis(this AstigmaticDecomposition ad, bool cminus = true)
        {
            double cylinder = Math.Sqrt(ad.C0 * ad.C0 + ad.C45 * ad.C45);
            double sphere = ad.M - cylinder / 2.0;
            double axis = Math.Atan2(cylinder - ad.C0, ad.C45) * 180 / Math.PI;
            SphereCylinderAxis result = new SphereCylinderAxis(sphere, cylinder, axis);
            if (cminus)
            {
                result.Reverse();
            }
            return result;
        }

        /// <summary>
        /// Convert to astigmatic decomposition.
        /// </summary>
        /// <param name="sca">The sca.</param>
        /// <returns></returns>
        public static AstigmaticDecomposition ToAstigmaticDecomposition(this SphereCylinderAxis sca)
        {
            return new AstigmaticDecomposition(sca.Sphere + sca.Cylinder / 2.0, sca.Cylinder*sca.Axis.Cos2(), sca.Cylinder*sca.Axis.Sin2());
        }

        /// <summary>
        /// Covnert to astigmatic decomposition.
        /// </summary>
        /// <param name="pam">The pam.</param>
        /// <returns></returns>
        public static AstigmaticDecomposition ToAstigmaticDecomposition(this PowersAndMeridians pam)
        {
            if (double.IsInfinity(pam.Power1) || double.IsInfinity(pam.Power2))
            {
                return new AstigmaticDecomposition(double.PositiveInfinity, 0, 0);
            }
            else
            {
                return ToAstigmaticDecomposition(pam.ToSphereCylinderAxis());
            }
        }
    }
}
