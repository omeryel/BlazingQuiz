using BlazingQuiz.Api.Data;
using BlazingQuiz.Api.Data.Entities;
using BlazingQuiz.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BlazingQuiz.Api.Services
{
    public class CategoryService
    {
        private readonly QuizContext _context;
        public CategoryService(QuizContext context)
        {
            _context = context;
        }


        public async Task<QuizApiResponse> SaveCategoryAsync(CategoryDto dto)
        {
            if (await _context.Categories.AsNoTracking().AnyAsync(x => x.Name == dto.Name && x.Id != dto.Id))
            {

                return QuizApiResponse.Fail("Category with same name exists already");
            }

            if (dto.Id == 0)
            {
                var category = new Category
                {
                    Name = dto.Name
                };
                _context.Categories.Add(category);
            }
            else
            {
                var dbCategory = await _context.Categories.FirstOrDefaultAsync(x => x.Id == dto.Id);
                if (dbCategory == null)
                {
                    return QuizApiResponse.Fail("Category does not exist");
                }
                dbCategory.Name = dto.Name;
                _context.Categories.Update(dbCategory);


            }
            await _context.SaveChangesAsync();
            return QuizApiResponse.Success();
        }


        public async Task<CategoryDto[]> GetCategoriesAsync()
        {
            return await _context.Categories.AsNoTracking().Select(x => new CategoryDto
            {
                Name = x.Name,
                Id = x.Id
            }).ToArrayAsync();
        }


    }
}
