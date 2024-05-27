using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Fitness.Core;

namespace Fitness
{
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


