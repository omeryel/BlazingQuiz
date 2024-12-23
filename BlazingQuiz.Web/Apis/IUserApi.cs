using BlazingQuiz.Shared;
using BlazingQuiz.Shared.DTOs;
using Refit;

namespace BlazingQuiz.Web.Apis
{
    [Headers("Authorization: Bearer ")]
    public interface IUserApi
    {
        [Get("/api/users")]
        Task<PagedResult<UserDto>> GetUserAsync(UserApprovedFilter approveType, int startIndex, int pageSize);
        [Patch("/api/users/{userId}/toggle-status")]
        Task ToggleUserApprovedStatus(int userId);
    }
}
