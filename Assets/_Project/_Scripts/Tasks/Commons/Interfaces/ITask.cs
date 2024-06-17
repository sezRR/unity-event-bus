using _Project._Scripts.Events.Commons.Interfaces;
using _Project._Scripts.Tasks.Commons.Enums;

namespace _Project._Scripts.Tasks.Commons.Interfaces
{
    public interface ITask : IEventRegistrar
    { 
        TaskStatus Status { get; }
        bool IsCompleted { get; }
        void ResetProgress();
    }
}