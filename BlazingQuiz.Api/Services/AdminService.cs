using BlazingQuiz.Api.Data;
using BlazingQuiz.Shared.DTOs;
using BlazingQuiz.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazingQuiz.Api.Services
{
    public class AdminService
    {
        private readonly IDbContextFactory<QuizContext> _contextFactory;

        public AdminService(IDbContextFactory<QuizContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }        

        public async Task<AdminHomeDataDto> GetHomeDataAsync()
        {
            var totalCategoriesTask = _contextFactory.CreateDbContext().Categories.CountAsync();
            var totalStudentsTask = _contextFactory.CreateDbContext().Users.Where(x => x.Role == nameof(UserRole.Student)).CountAsync();
            var approvedStudentsTask = _contextFactory.CreateDbContext().Users.Where(x => x.Role == nameof(UserRole.Student) && x.IsApproved).CountAsync();
            var totalQuizesTask = _contextFactory.CreateDbContext().Quizzes.CountAsync();
            var activeQuizesTask = _contextFactory.CreateDbContext().Quizzes.Where(x => x.IsActive).CountAsync();

            var totalCategories = await totalCategoriesTask;
            var totalStudents = await totalStudentsTask;
            var approvedStudents = await approvedStudentsTask;
            var totalQuizes = await totalQuizesTask;
            var activeQuizes = await activeQuizesTask;

            return new AdminHomeDataDto(totalCategories, totalStudents, approvedStudents, totalQuizes, activeQuizes);
        }


        public async Task<PagedResult<UserDto>> GetUserAsync(UserApprovedFilter approveType, int startIndex, int pageSize)
        {
            var query = _contextFactory.CreateDbContext().Users.Where(u => u.Role != nameof(UserRole.Admin)).AsQueryable();

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

            using var context = _contextFactory.CreateDbContext();
            var dbUser = await context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (dbUser != null)
            {
                dbUser.IsApproved = !dbUser.IsApproved;
                await context.SaveChangesAsync();
            }
        }

    }
}
