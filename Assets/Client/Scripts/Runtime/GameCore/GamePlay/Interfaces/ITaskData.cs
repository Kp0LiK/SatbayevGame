namespace Client
{
    public interface ITaskData
    {
        string GetQuestionText();
        TaskType GetTaskType();
        bool ValidateAnswer(object answer);
    }
}