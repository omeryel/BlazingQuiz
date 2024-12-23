using BlazingQuiz.Api.Data;
using BlazingQuiz.Shared;
using BlazingQuiz.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BlazingQuiz.Api.Services
{
    public class UserService
    {
        private readonly QuizContext _quizContext;

        public UserService(QuizContext quizContext)
        {
            _quizContext = quizContext;
        }

        public async Task<PagedResult<UserDto>> GetUserAsync(UserApprovedFilter approveType, int startIndex, int pageSize)
        {
            var query = _quizContext.Users.Where(u => u.Role != nameof(UserRole.Admin)).AsQueryable();

            if (approveType != UserApprovedFilter.All)
            {
                if (approveType == UserApprovedFilter.ApprovedOnly)
                {
                    query = query.Where(x => x.IsApproved);
                }
                else
                {
                    query = query.Where(x => !x.IsApproved);
                }
            }

            var total = await query.CountAsync();

            var users = await query.OrderByDescending(x => x.Id)
                .Skip(startIndex)
                .Take(pageSize)
                .Select(x => new UserDto(x.Id, x.Name, x.Email, x.Phone, x.IsApproved))
                .ToArrayAsync();

            return new PagedResult<UserDto>(users, total);
        }

        public async Task ToggleUserApprovedStatus(int userId)
        {
            var dbUser = await _quizContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (dbUser != null)
            {
                dbUser.IsApproved = !dbUser.IsApproved;
                await _quizContext.SaveChangesAsync();
            }
        }
    }
}
