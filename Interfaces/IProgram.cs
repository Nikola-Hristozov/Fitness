using Fitness.Core;

namespace Fitness.Interfaces
{
    public interface IProgram
    {
        public string Program { get; set; }

        public Trainer Trainer { get; set; }

        public string GetProgram();

        public void Update()
        {
            Program = Trainer.TrainingPrograms.GetValueOrDefault(GetType()).ProgramInfo;
        }
    }
}


