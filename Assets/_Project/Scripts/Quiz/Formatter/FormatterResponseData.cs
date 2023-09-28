namespace DreamQuiz
{
    public class FormatterResponseData
    {
        public bool IsDone = false;
        public bool HasError = false;
        public string ErrorMessage = string.Empty;

        public FormatterResponseData()
        {
            IsDone = false;
            HasError = false;
            ErrorMessage = string.Empty;
        }
    }
}