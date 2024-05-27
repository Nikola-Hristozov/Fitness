using Fitness.Programs;
using Fitness.Programs.Decorators;

namespace Fitness.Programs.BaseProgramDecorators
{
    class CyclingProgram : CarrdioProgram
    {
        public CyclingProgram(BaseProgram comp, string program = "CyclingProgram") : base(comp)
        {
            Program = program;
        }

        public override string GetProgram()
        {
            return $"{base.GetProgram()}\n{Program}";
        }
    }
}


