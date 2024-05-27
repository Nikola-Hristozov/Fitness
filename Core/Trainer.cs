using Fitness.Interfaces;
using Fitness.Programs;
using Fitness.Programs.BaseProgramDecorators;
using Fitness.Programs.IntensiveProgramDecorators;

namespace Fitness.Core
{
    public sealed class Trainer : ITrainer
    {
        private static Trainer _trainer;

        private List<IProgram> ProgramCourses = new List<IProgram>();

        public new Dictionary<Type, (string ProgramInfo, Type? Decoratee)> TrainingPrograms { get; set; }

        private Trainer()
        {
            TrainingPrograms = new Dictionary<Type, (string ProgramInfo, Type? Decoratee)>
            {
                { typeof(BaseProgram), (nameof(BaseProgram), null) },
                { typeof(IntensiveProgram), (nameof(IntensiveProgram), null) },
                { typeof(RunningProgram), (nameof(RunningProgram), typeof(BaseProgram)) },
                { typeof(CyclingProgram), (nameof(CyclingProgram), typeof(BaseProgram)) },
                { typeof(BodyBuilderProgram), (nameof(BodyBuilderProgram), typeof(IntensiveProgram)) },
                { typeof(StrongCoreProgram), (nameof(StrongCoreProgram), typeof(IntensiveProgram)) },
            };
        }

        private Trainer(Dictionary<Type, (string ProgramInfo, Type? Decoratee)> initialTrainingPrograms)
        {
            TrainingPrograms = initialTrainingPrograms;
        }

        public static Trainer GetTrainer()
        {
            if (_trainer == null)
            {
                _trainer = new Trainer();
            }
            return _trainer;
        }

        public IProgram GetProgram(string name, IProgram? program = null)
        {
            IProgram programResult = program is null ?
                name switch
                {
                    nameof(BaseProgram) => new BaseProgram(TrainingPrograms.GetValueOrDefault(typeof(BaseProgram)).ProgramInfo),
                    nameof(IntensiveProgram) => new IntensiveProgram(TrainingPrograms.GetValueOrDefault(typeof(IntensiveProgram)).ProgramInfo),
                    _ => throw new ApplicationException(),
                }
                :
                (name, program.GetType().Name) switch
                {
                    (nameof(RunningProgram), nameof(BaseProgram)) => new RunningProgram(program as BaseProgram, TrainingPrograms.GetValueOrDefault(typeof(RunningProgram)).ProgramInfo),
                    (nameof(CyclingProgram), nameof(BaseProgram)) => new CyclingProgram(program as BaseProgram, TrainingPrograms.GetValueOrDefault(typeof(CyclingProgram)).ProgramInfo),
                    (nameof(BodyBuilderProgram), nameof(IntensiveProgram)) => new BodyBuilderProgram(program as IntensiveProgram, TrainingPrograms.GetValueOrDefault(typeof(BodyBuilderProgram)).ProgramInfo),
                    (nameof(StrongCoreProgram), nameof(IntensiveProgram)) => new StrongCoreProgram(program as IntensiveProgram, TrainingPrograms.GetValueOrDefault(typeof(StrongCoreProgram)).ProgramInfo),
                    _ => throw new ApplicationException(),
                };

            programResult.Trainer = this;
            Attach(programResult);
            return programResult;

        }

        public Dictionary<Type, (string ProgramInfo, Type? Decoratee)> GetFullPrograms() =>
            TrainingPrograms
            .Where(x => (x.Key.BaseType.Equals(typeof(object)) ?
                    x.Key.GetInterfaces() :
                    x.Key.GetInterfaces().Except(x.Key.BaseType.GetInterfaces()))
                .Contains(typeof(IProgram)))
            .ToDictionary(x => x.Key, y => y.Value);

        public Dictionary<Type, (string ProgramInfo, Type? Decoratee)> GetPossibleExtensions(Type fullProgram) =>
            TrainingPrograms
            .Where(x => x.Value.Decoratee == fullProgram)
            .ToDictionary(x => x.Key, y => y.Value);

        public void PrintProgram(PersonalProgram program)
        {
            Console.WriteLine("RESULT: \n" + program.GetProgramPrint());
        }

        public void Attach(IProgram program)
        {
            ProgramCourses.Add(program);
        }

        public void Detach(IProgram program)
        {
            ProgramCourses.Remove(program);
        }

        public void UpdateProgram(Type program, string info)
        {
            var oldValue = TrainingPrograms.GetValueOrDefault(program);
            TrainingPrograms.Remove(program);
            TrainingPrograms.Add(program, (ProgramInfo: info, oldValue.Decoratee));
            Notify();
        }

        public void Notify()
        {
            ProgramCourses.ForEach(x => x.Update());
        }
    }
}


