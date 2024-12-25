using BlazingQuiz.Shared.DTOs;

namespace BlazingQuiz.Web
{
    public class QuizState
    {
        public QuizListDto? Quiz {  get; private set; }

        public void SetQuiz(QuizListDto? quiz) => Quiz = quiz;
    }
}
