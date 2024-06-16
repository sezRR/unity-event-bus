namespace _Project._Scripts.NewTasks
{
    public interface ITaskRequirement
    {
        bool IsSatisfied();
        void RegisterEvent();
        void UnregisterEvent();
    }

}