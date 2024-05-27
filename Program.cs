using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Fitness
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
            .Where(x => (x.Key.BaseType.Equals(typeof(Object)) ?
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

    public interface ITrainer
    {
        void Attach(IProgram program);

        void Detach(IProgram program);

        void Notify();
    }

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

    public interface IProgram
    {
        public string Program { get; set; }

        public Trainer Trainer { get; set; }

        public string GetProgram();

        public void Update()
        {
            Program = Trainer.TrainingPrograms.GetValueOrDefault(this.GetType()).ProgramInfo;
        }
    }

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

    abstract class CarrdioProgram : IProgram
    {
        protected IProgram _trainingProgram;

        public Trainer Trainer { get; set; }
        public string Program { get; set; }

        public CarrdioProgram(IProgram trainingProgram)
        {
            this._trainingProgram = trainingProgram;
        }

        public void SetProgram(IProgram trainingProgram)
        {
            this._trainingProgram = trainingProgram;
        }

        public virtual string GetProgram()
        {
            if (this._trainingProgram != null)
            {
                return this._trainingProgram.GetProgram();
            }
            else
            {
                return string.Empty;
            }
        }
    }

    abstract class HeavyLiftingProgram : IProgram
    {
        protected IProgram _trainingProgram;

        public Trainer Trainer { get; set; }
        public string Program { get; set; }

        public HeavyLiftingProgram(IProgram trainingProgram)
        {
            this._trainingProgram = trainingProgram;
        }

        public void SetProgram(IProgram trainingProgram)
        {
            this._trainingProgram = trainingProgram;
        }

        public virtual string GetProgram()
        {
            if (this._trainingProgram != null)
            {
                return this._trainingProgram.GetProgram();
            }
            else
            {
                return string.Empty;
            }
        }
    }

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

    class Program
    {
        static void Main(string[] args)
        {
            var trainer = Trainer.GetTrainer();

            var rnd = new Random();

            var flag = rnd.Next(2);

            var fullPrograms = trainer.GetFullPrograms();
            var currentProgram = fullPrograms.ElementAt(flag);

            var myProgram = new PersonalProgram(trainer.GetProgram(currentProgram.Value.ProgramInfo));

            currentProgram = fullPrograms.ElementAt(flag == 0 ? 1 : 0);

            myProgram.SetProgram(trainer.GetProgram(currentProgram.Value.ProgramInfo));

            var decorators = trainer.GetPossibleExtensions(currentProgram.Key);

            var currentDecorator = decorators.ElementAt(rnd.Next(decorators.Count));

            myProgram.SetProgram(trainer.GetProgram(currentDecorator.Value.ProgramInfo, myProgram.GetProgram()));

            flag = rnd.Next(2);
            var toUpdate = flag == 0 ? currentProgram : currentDecorator;

            trainer.UpdateProgram(toUpdate.Key, toUpdate.Value.ProgramInfo + " UPDATED");

            Console.WriteLine(myProgram.GetProgramPrint());
        }
    }
}


