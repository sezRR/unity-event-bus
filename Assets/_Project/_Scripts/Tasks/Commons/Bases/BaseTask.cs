using System;
using _Project._Scripts.Tasks.Commons.Enums;
using _Project._Scripts.Tasks.Commons.Interfaces;
using UnityEngine;

namespace _Project._Scripts.Tasks.Commons.Bases
{
    [Serializable]
    [CreateAssetMenu(fileName = "NewTask", menuName = "Tasks/Task")]
    public class BaseTask : ScriptableObject, ITask
    {
        public string taskName;
        public string description;
        public BaseTaskRequirement[] requirements;
        public BaseTask[] subTasks;

        [SerializeField]
        private TaskStatus status = TaskStatus.NotStarted;
        public TaskStatus Status => status;

        public bool IsCompleted => CheckCompletion();

        private bool CheckCompletion()
        {
            // Check if all requirements are satisfied
            foreach (var requirement in requirements)
            {
                if (!requirement.IsSatisfied())
                {
                    status = TaskStatus.InProgress;
                    return false;
                }
            }

            // Check if all sub-tasks are completed
            foreach (var subTask in subTasks)
            {
                if (!subTask.IsCompleted)
                {
                    status = TaskStatus.InProgress;
                    return false;
                }
            }

            status = TaskStatus.Completed;
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

            UpdateStatus();
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
        
        public void ResetProgress()
        {
            foreach (var requirement in requirements)
            {
                if (requirement is BaseTaskRequirement baseTaskRequirement)
                {
                    baseTaskRequirement.ResetProgress();
                }
            }

            foreach (var subTask in subTasks)
            {
                subTask.ResetProgress();
            }

            status = TaskStatus.NotStarted;
        }

        private void UpdateStatus()
        {
            if (IsCompleted)
            {
                status = TaskStatus.Completed;
            }
            else
            {
                bool anyInProgress = false;
                bool allNotStarted = true;

                foreach (var requirement in requirements)
                {
                    if (requirement.IsSatisfied())
                    {
                        anyInProgress = true;
                        allNotStarted = false;
                        break;
                    }
                }

                foreach (var subTask in subTasks)
                {
                    if (subTask.Status != TaskStatus.NotStarted)
                    {
                        anyInProgress = true;
                        allNotStarted = false;
                        break;
                    }
                }

                if (allNotStarted)
                {
                    status = TaskStatus.NotStarted;
                }
                else if (anyInProgress)
                {
                    status = TaskStatus.InProgress;
                }
            }
        }
    }
}
