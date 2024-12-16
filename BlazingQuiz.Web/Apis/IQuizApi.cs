using BlazingQuiz.Shared.DTOs;
using Refit;
using System.Threading.Tasks;

namespace BlazingQuiz.Web.Apis
{
    [Headers("Authorization: Bearer")]
    public interface IQuizApi
    {
        [Post("/api/quizes")]
        Task<QuizApiResponse> SaveQuizAsync(QuizSaveDto dto);


        [Get("/api/quizes")]
        Task<QuizListDto[]> GetQuizesAsync();


        [Get("/api/quizes/{quizId}/questions")]
        Task<QuestionDto[]> GetQuizQuestions(Guid quizId);


        [Get("/api/quizes/{quizId}")]
        Task<QuizSaveDto?> GetQuizToEditAsync(Guid quizId);

    }
}
