using BlazingQuiz.Api.Services;
using BlazingQuiz.Shared.DTOs;

namespace BlazingQuiz.Api.Endpoints
{
    public static class QuizEndpoints
    {
        public static IEndpointRouteBuilder MapQuizEndpoints(this IEndpointRouteBuilder builder)
        {
            var quizGroup = builder.MapGroup("/api/quizes").RequireAuthorization();

            quizGroup.MapPost("", async (QuizSaveDto dto, QuizService service) =>
            {
                if (dto.Questions.Count == 0)
                {
                    return Results.BadRequest("Please provide Questions");
                }
                if (dto.Questions.Count != dto.TotalQuesions)
                {
                    return Results.BadRequest("Total questions count does not match with provided questions");
                }

                return Results.Ok(await service.SaveQuizAsync(dto));
            });

            quizGroup.MapGet("", async (QuizService service) => Results.Ok(await service.GetQuizesAsync()));

            quizGroup.MapGet("{quizId:guid}/questions", async (Guid quizId, QuizService service) =>
            {
                return Results.Ok(await service.GetQuizQuestions(quizId));
            });

            return builder;
        }

    }
}
