using Trazable.Engine.Base;

namespace Trazable.Engine.System
{
    public class Test : OptSystem
    {
        public Test(string name)
            : base(name)
        {
        }

        public override void Initialize()
        {
            vars.AddVariable(VarDirection.Input, VarType.SCA, "spectacle", "Spectacle");
            vars.AddVariable(VarDirection.Input, VarType.PAM, "cornea", "Cornea");
            vars.AddVariable(VarDirection.Input, VarType.Double, "bvd", "BVD");
            vars.AddVariable(VarDirection.Input, VarType.Double, "ct", "CT");
            vars.AddVariable(VarDirection.Input, VarType.Double, "acd", "ACD");
            vars.AddVariable(VarDirection.Input, VarType.Double, "sf", "SF");
            vars.AddVariable(VarDirection.Output, VarType.SCA, "ev", "EV");
            vars.AddVariable(VarDirection.Output, VarType.SCA, "theoricalIOL", "Theorical IOL");
            vars.AddVariable(VarDirection.Input, VarType.SCA, "realIOL", "Real IOL");
            vars.AddVariable(VarDirection.Output, VarType.SCA, "resultSpectacle", "Result Spectacle");
        }
        public override void Execute()
        {
            OpticalRepresentation spectacle = vars.GetAs<OpticalRepresentation>("spectacle");
            OpticalRepresentation cornea = vars.GetAs<OpticalRepresentation>("cornea");
            double bvd = vars.GetAs<double>("bvd");
            double ct = vars.GetAs<double>("ct");
            double acd = vars.GetAs<double>("acd");
            double sf = vars.GetAs<double>("sf");
            OpticalRepresentation ev = null;
            OpticalRepresentation theoricalIOL = null;
            OpticalRepresentation realIOL = vars.GetAs<OpticalRepresentation>("realIOL");
            OpticalRepresentation resultSpectacle = null;
            ev = spectacle.Propagate(bvd).Refracte(cornea).Propagate(ct).Propagate(acd).Propagate(sf);
            theoricalIOL = cornea.Propagate(ct).Propagate(acd).Propagate(sf).RefracteReverse(ev);
            resultSpectacle = cornea.RefracteReverse(realIOL.RefracteReverse(ev).PropagateReverse(ct).PropagateReverse(acd).PropagateReverse(sf)).PropagateReverse(bvd);
            vars.Set("ev", ev);
            vars.Set("theoricalIOL", theoricalIOL);
            vars.Set("resultSpectacle", resultSpectacle);
        }
    }
}
