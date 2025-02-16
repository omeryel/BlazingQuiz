using BlazingQuiz.Api.Services;
using BlazingQuiz.Shared;
using BlazingQuiz.Shared.DTOs;
using System.Security.Claims;

namespace BlazingQuiz.Api.Endpoints
{
    public static class StudentQuizEndpoints
    {
        public static int GetStudentId(this ClaimsPrincipal principal) =>
            Convert.ToInt32(principal.FindFirstValue(ClaimTypes.NameIdentifier));

        public static IEndpointRouteBuilder MapStudentQuizEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/student").RequireAuthorization(x => x.RequireRole(nameof(UserRole.Student)));

            group.MapGet("available-quizes", async (int categoryId, StudentQuizService service) =>
                Results.Ok(await service.GetActiveQuizesAsync(categoryId)));

            group.MapGet("/my-quizes", async (int startIndex, int pageSize, StudentQuizService service, ClaimsPrincipal claimsPrincipal) =>
            Results.Ok(await service.GetStudentQuizesAsync(claimsPrincipal.GetStudentId(), startIndex, pageSize)));

            var quizGroup = group.MapGroup("/quiz");

            quizGroup.MapPost("/{quizId:guid}/start", async (Guid quizId, ClaimsPrincipal principal, StudentQuizService service) =>
            {
                var studentId = GetStudentId(principal);
                var response = await service.StartQuizAsync(studentId, quizId);
                return Results.Ok(response);
            });

            quizGroup.MapGet("/{studentQuizId:int}/next-question", async (int studentQuizId, ClaimsPrincipal principal, StudentQuizService service) =>
            {

                var studentId = GetStudentId(principal);
                var response = await service.GetNextQuestionForQuizAsync(studentQuizId, studentId);
                return Results.Ok(response);
            });

            quizGroup.MapPost("/{studentQuizId:int}/save-response", async (int studentQuizId, StudentQuizQuestionResponseDto dto, ClaimsPrincipal principal, StudentQuizService service) =>
            {
                var studentId = GetStudentId(principal);
                if (studentQuizId != dto.StudentQuizId)
                {
                    return Results.Unauthorized();
                }

                var response = await service.SaveQuestionResponseAsync(dto, studentId);
                return Results.Ok(response);
            });

            quizGroup.MapPost("/{studentQuizId:int}/submit", async (int studentQuizId, ClaimsPrincipal principal, StudentQuizService service) =>
            {
                var response = await service.SubmitQuizAsync(studentQuizId, principal.GetStudentId());
                return Results.Ok(response);
            });

            quizGroup.MapPost("/{studentQuizId:int}/auto-submit", async (int studentQuizId, ClaimsPrincipal principal, StudentQuizService service) =>
            {
                var response = await service.AutoSubmitQuizAsync(studentQuizId, principal.GetStudentId());
                return Results.Ok(response);
            });


            quizGroup.MapPost("/{studentQuizId:int}/exit", async (int studentQuizId, ClaimsPrincipal principal, StudentQuizService service) =>
            {
                var response = await service.ExitQuizAsync(studentQuizId, principal.GetStudentId());
                return Results.Ok(response);
            });

            return app;
        }

    }
}
