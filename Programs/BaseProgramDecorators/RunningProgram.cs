using Fitness.Programs;
using Fitness.Programs.Decorators;

namespace Fitness.Programs.BaseProgramDecorators
{
    class RunningProgram : CarrdioProgram
    {
        public RunningProgram(BaseProgram comp, string program = "RunningProgram") : base(comp)
        {
            Program = program;
        }

        public override string GetProgram()
        {
            return $"{base.GetProgram()}\n{Program}";
        }
    }
}


