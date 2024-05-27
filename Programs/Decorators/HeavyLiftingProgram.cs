using Fitness.Core;
using Fitness.Interfaces;

namespace Fitness.Programs.Decorators
{
    abstract class HeavyLiftingProgram : IProgram
    {
        protected IProgram _trainingProgram;

        public Trainer Trainer { get; set; }
        public string Program { get; set; }

        public HeavyLiftingProgram(IProgram trainingProgram)
        {
            _trainingProgram = trainingProgram;
        }

        public void SetProgram(IProgram trainingProgram)
        {
            _trainingProgram = trainingProgram;
        }

        public virtual string GetProgram()
        {
            if (_trainingProgram != null)
            {
                return _trainingProgram.GetProgram();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}


