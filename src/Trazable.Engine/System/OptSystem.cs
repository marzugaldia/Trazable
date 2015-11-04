namespace Trazable.Engine.System
{
    public abstract class OptSystem
    {
        protected VarSystem vars;

        public VarSystem Vars
        {
            get { return this.vars; }
        }

        public OptSystem(string name)
        {
            this.vars = new VarSystem(name);
        }

        public abstract void Initialize();

        public abstract void Execute();
    }
}
