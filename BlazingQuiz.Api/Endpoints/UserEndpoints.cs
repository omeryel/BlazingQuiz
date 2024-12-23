using BlazingQuiz.Api.Services;
using BlazingQuiz.Shared;
using System.Reflection.Metadata.Ecma335;

namespace BlazingQuiz.Api.Endpoints
{
    public static class UserEndpoints
    {
        public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/users").RequireAuthorization(x => x.RequireRole(nameof(UserRole.Admin)));


            group.MapGet("", async (UserApprovedFilter approveType, int startIndex, int pageSize, UserService service) =>
            {
                //var approvedFilter = Enum.Parse<UserApprovedFilter>(filter);
                return Results.Ok(await service.GetUserAsync(approveType, startIndex, pageSize));
            });

            group.MapPatch("{userId:int}/toggle-status", async (int userId, UserService service) =>
            {

                await service.ToggleUserApprovedStatus(userId);
                return Results.Ok();
            });

            return app;

        }
    }
}
