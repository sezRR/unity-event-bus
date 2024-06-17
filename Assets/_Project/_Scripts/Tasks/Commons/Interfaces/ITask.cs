using _Project._Scripts.Events.Commons.Interfaces;

namespace _Project._Scripts.Tasks.Commons.Interfaces
{
    public interface ITask : IEventRegistrar
    { 
        bool IsCompleted { get; }
        void ResetProgress();
    }
}