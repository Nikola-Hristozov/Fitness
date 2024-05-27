using Fitness.Core;
using Fitness.Interfaces;

namespace Fitness.Programs
{
    class BaseProgram : IProgram
    {
        public Trainer Trainer { get; set; }
        public string Program { get; set; }

        public BaseProgram(string program = "BaseProgram")
        {
            Program = program;
        }
        public string GetProgram()
        {
            return Program;
        }

    }
}


