using System;
using _Project._Scripts.Tasks.Commons.Enums;
using _Project._Scripts.Tasks.Commons.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace _Project._Scripts.Tasks.Commons.Bases
{
    [Serializable]
    public abstract class BaseTaskStep : ITaskStep
    {
        [SerializeField] private int id;
        [SerializeField] private TaskStatus status;
        [SerializeField] private int stepIndex;
        [SerializeField] private UnityEvent onComplete;

        public int Id
        {
            get => id;
            set => id = value;
        }

        public TaskStatus Status
        {
            get => status;
            set => status = value;
        }

        public int StepIndex
        {
            get => stepIndex;
            set => stepIndex = value;
        }

        public UnityEvent OnComplete
        {
            get => onComplete;
            set => onComplete = value;
        }
    }
}