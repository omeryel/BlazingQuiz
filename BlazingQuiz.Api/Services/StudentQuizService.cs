using BlazingQuiz.Api.Data;
using BlazingQuiz.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BlazingQuiz.Api.Services
{
    public class StudentQuizService
    {
        private readonly QuizContext _context;

        public StudentQuizService(QuizContext context)
        {
            _context = context;
        }

        public async Task<QuizListDto[]> GetActiveQuizesAsync(int categoryId)
        {
            var query = _context.Quizzes.Where(x => x.IsActive);

            if (categoryId > 0)
            {
                query = query.Where(x => x.CategoryId == categoryId);
            }

            var quizes = await query.Select(x => new QuizListDto
            {
                CategoryId = categoryId,
                CategoryName = x.Category.Name,
                Name = x.Category.Name,
                TimeInMinutes = x.TimeInMinutes,
                TotalQuestions = x.TotalQuestions,
                Id = x.Id
            }).ToArrayAsync();

            return quizes;
        }

    }
}
