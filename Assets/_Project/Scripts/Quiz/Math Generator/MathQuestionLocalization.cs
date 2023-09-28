public static class MathQuestionLocalization
{
    public static string GetTranslatedQuestionTitle(string question)
    {
        string returnStr = question;

        if (PlayerProgress.QuizLanguage != QuizLanguageType.pt)
        {
            return returnStr;
        }

        switch (question)
        {
            case "What is the median of ":
                returnStr = "Quanto é a mediana de ";
                break;
            case "What is ":
                returnStr = "Quanto é ";
                break;
            case "What is the mean of ":
                returnStr = "Quanto é a média de ";
                break;
            case "What is the quotient and the remainder from ":
                returnStr = "Qual é o quociente e o resto de ";
                break;
            case " is the quotient and ":
                returnStr = " é o quociente e ";
                break;
            case " is the remainder.":
                returnStr = " é o resto.";
                break;
        }

        return returnStr;
    }
}