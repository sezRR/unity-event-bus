namespace _Project._Scripts.NewTasks
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewTask", menuName = "Tasks/Task")]
    public class Task : ScriptableObject
    {
        public string taskName;
        public string description;
        public BaseTaskRequirement[] requirements;
        public Task[] subTasks;

        public bool IsCompleted => CheckCompletion();

        private bool CheckCompletion()
        {
            // Check if all requirements are satisfied
            foreach (var requirement in requirements)
            {
                if (!requirement.IsSatisfied())
                    return false;
            }

            // Check if all sub-tasks are completed
            foreach (var subTask in subTasks)
            {
                if (!subTask.IsCompleted)
                    return false;
            }

            return true;
        }

        public void RegisterEvents()
        {
            foreach (var requirement in requirements)
            {
                requirement.RegisterEvent();
            }

            foreach (var subTask in subTasks)
            {
                subTask.RegisterEvents();
            }
        }

        public void UnregisterEvents()
        {
            foreach (var requirement in requirements)
            {
                requirement.UnregisterEvent();
            }

            foreach (var subTask in subTasks)
            {
                subTask.UnregisterEvents();
            }
        }
    }

}