using Fitness.Programs;
using Fitness.Programs.Decorators;

namespace Fitness.Programs.IntensiveProgramDecorators
{
    class BodyBuilderProgram : HeavyLiftingProgram
    {
        public BodyBuilderProgram(IntensiveProgram comp, string program = "BodyBuilderProgram") : base(comp)
        {
            Program = program;
        }

        public override string GetProgram()
        {
            return $"{base.GetProgram()}\n{Program}";
        }
    }
}


