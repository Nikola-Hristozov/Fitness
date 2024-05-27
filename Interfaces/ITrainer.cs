namespace Fitness.Interfaces
{
    public interface ITrainer
    {
        void Attach(IProgram program);

        void Detach(IProgram program);

        void Notify();
    }
}


