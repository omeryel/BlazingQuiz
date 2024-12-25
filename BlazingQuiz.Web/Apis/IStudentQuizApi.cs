using BlazingQuiz.Shared.DTOs;
using Refit;
using System.Runtime;

namespace BlazingQuiz.Web.Apis
{

    [Headers("Authorization: Bearer ")]
    public interface IStudentQuizApi
    {

        [Get("/api/student/available-quizes")]
        Task<QuizListDto[]> GetActiveQuizesAsync(int categoryId);

    }
}
