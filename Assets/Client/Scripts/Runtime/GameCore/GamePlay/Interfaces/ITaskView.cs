using UnityEngine;

namespace Client
{
    public interface ITaskView
    {
        void Initialize(ITaskData taskData);
        void Show();
        void Hide();
        void OnAnswerSelected(object answer);
    }
} 