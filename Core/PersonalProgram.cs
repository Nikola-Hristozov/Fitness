using Fitness.Interfaces;

namespace Fitness.Core
{
    public class PersonalProgram
    {
        private IProgram Program;

        private List<IProgram> Programs = new List<IProgram>();

        public PersonalProgram(IProgram program)
        {
            Programs.Add(program);
            Program = program;
        }

        public void SetProgram(IProgram program)
        {
            Programs.Add(program);
            Program = program;
        }

        public IProgram GetProgram()
        {
            return Program;
        }

        public string GetProgramPrint()
        {
            return Program.GetProgram();
        }
    }
}


