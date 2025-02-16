using BlazingQuiz.Api.Services;
using BlazingQuiz.Shared;
using System.Reflection.Metadata.Ecma335;

namespace BlazingQuiz.Api.Endpoints
{
    public static class AdminEndpoints
    {
        public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/admin/home-data", async (AdminService service) =>
            Results.Ok(await service.GetHomeDataAsync()))
                .RequireAuthorization(x => x.RequireRole(nameof(UserRole.Admin)));

            var group = app.MapGroup("/api/users").RequireAuthorization(x => x.RequireRole(nameof(UserRole.Admin)));


            group.MapGet("", async (UserApprovedFilter approveType, int startIndex, int pageSize, AdminService service) =>
            {
                //var approvedFilter = Enum.Parse<UserApprovedFilter>(filter);
                return Results.Ok(await service.GetUserAsync(approveType, startIndex, pageSize));
            });

            group.MapPatch("{userId:int}/toggle-status", async (int userId, AdminService service) =>
            {

                await service.ToggleUserApprovedStatus(userId);
                return Results.Ok();
            });

            return app;

        }
    }
}
