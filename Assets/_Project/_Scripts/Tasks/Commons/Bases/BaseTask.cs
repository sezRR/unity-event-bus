using System;
using System.Collections.Generic;
using _Project._Scripts.Tasks.Commons.Enums;
using _Project._Scripts.Tasks.Commons.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace _Project._Scripts.Tasks.Commons.Bases
{
    public abstract class BaseTask : ScriptableObject, ITask
    {
        [SerializeField] private TaskStatus status;
        [SerializeField] private List<BaseTaskStep> steps = new();
        public Action OnComplete { get; set; }

        public TaskStatus Status
        {
            get => status;
            set => status = value;
        }

        public List<BaseTaskStep> Steps
        {
            get => steps;
            set => steps = value;
        }

        public bool Complete()
        {
            Debug.Log("Completed");
            return true;
        }

        public bool Skip()
        {
            Debug.Log("Skipped");
            return true;
        }
    }
}