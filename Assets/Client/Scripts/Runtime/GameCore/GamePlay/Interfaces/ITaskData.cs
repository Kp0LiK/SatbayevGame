namespace Client
{
    public interface ITaskData
    {
        string GetQuestionText();
        bool ValidateAnswer(object answer);
    }
}