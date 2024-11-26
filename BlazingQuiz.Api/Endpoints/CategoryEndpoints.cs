using BlazingQuiz.Api.Services;
using BlazingQuiz.Shared;
using BlazingQuiz.Shared.DTOs;

namespace BlazingQuiz.Api.Endpoints
{
    public static class CategoryEndpoints
    {
        public static IEndpointRouteBuilder MapCategoryEndpoints(this IEndpointRouteBuilder app)
        {
            var categoryGroup = app.MapGroup("/api/categories").RequireAuthorization();

            categoryGroup.MapGet("", async (CategoryService categoryService) =>
            Results.Ok(await categoryService.GetCategoriesAsync()));

            categoryGroup.MapPost("", async (CategoryDto dto, CategoryService categoryService) =>
            Results.Ok(await categoryService.SaveCategoryAsync(dto)))
                .RequireAuthorization(x => x.RequireRole(nameof(UserRole.Admin)));

            return app;
        }
    }
}
