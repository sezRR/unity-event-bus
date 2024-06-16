namespace _Project._Scripts.Tasks.Commons.Interfaces
{
    public interface ITask
    {
        bool Complete();
        bool Skip();
    }
}