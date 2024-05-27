using Fitness.Core;
using Fitness.Interfaces;

namespace Fitness.Programs
{
    class IntensiveProgram : IProgram
    {
        public Trainer Trainer { get; set; }
        public string Program { get; set; }

        public IntensiveProgram(string program = "IntensiveProgram")
        {
            Program = program;
        }
        public string GetProgram()
        {
            return Program;
        }
    }
}


