using Fitness.Programs;
using Fitness.Programs.Decorators;

namespace Fitness.Programs.IntensiveProgramDecorators
{
    class StrongCoreProgram : HeavyLiftingProgram
    {
        public StrongCoreProgram(IntensiveProgram comp, string program = "StrongCoreProgram") : base(comp)
        {
            Program = program;
        }

        public override string GetProgram()
        {
            return $"{base.GetProgram()}\n{Program}";
        }
    }
}


