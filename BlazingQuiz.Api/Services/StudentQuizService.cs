using BlazingQuiz.Api.Data;
using BlazingQuiz.Api.Data.Entities;
using BlazingQuiz.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Xml.Schema;

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


        public async Task<QuizApiResponse<int>> StartQuizAsync(int studentId, Guid quizId)
        {
            try
            {
                var studentQuiz = new StudentQuiz
                {
                    QuizId = quizId,
                    StudentId = studentId,
                    StartedOn = DateTime.UtcNow,
                    Status = nameof(StudentQuizStatus.Started)
                };

                _context.StudentQuizzes.Add(studentQuiz);
                await _context.SaveChangesAsync();

                return QuizApiResponse<int>.Success(studentQuiz.Id);
            }
            catch (Exception ex)
            {

                return QuizApiResponse<int>.Fail(ex.Message);
            }
        }

        public async Task<QuizApiResponse<QuestionDto>> GetNextQuestionForQuizAsync(int studentQuizId, int studentId)
        {
            var studentQuiz = await _context.StudentQuizzes
                .Include(x => x.StudentQuizQuestions)
                .FirstOrDefaultAsync(x => x.Id == studentQuizId);

            if (studentQuiz == null)
            {
                return QuizApiResponse<QuestionDto>.Fail("Quiz does not exist");
            }

            if (studentQuiz.StudentId != studentId)
            {
                return QuizApiResponse<QuestionDto>.Fail("Invalid Reqest");
            }

            var questionsServed = studentQuiz.StudentQuizQuestions.Select(x => x.QuestionId).ToArray();

            var nextQuestion = await _context.Questions.Where(x => x.QuizId == studentQuiz.QuizId)
                .Where(x => !questionsServed.Contains(x.Id))
                .OrderBy(q => Guid.NewGuid())
                .Take(1)
                .Select(x => new QuestionDto
                {
                    Id = x.Id,
                    Text = x.Text,
                    Options = x.Options.Select(l => new OptionDto
                    {
                        Id = l.Id,
                        Text = l.Text,
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (nextQuestion == null)
                return QuizApiResponse<QuestionDto>.Fail("No more question for this quiz");

            try
            {
                var studentQuizQuestion = new StudentQuizQuestion
                {
                    StudentQuizId = studentQuizId,
                    QuestionId = nextQuestion.Id,
                };

                _context.StudentQuizQuestions.Add(studentQuizQuestion);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return QuizApiResponse<QuestionDto>.Fail(ex.Message);
            }

            return QuizApiResponse<QuestionDto>.Success(nextQuestion);
        }

        public async Task<QuizApiResponse> SaveQuestionResponseAsync(StudentQuizQuestionResponseDto dto, int studentId)
        {
            var studentQuiz = await _context.StudentQuizzes.AsTracking()
               .FirstOrDefaultAsync(x => x.Id == dto.StudentQuizId);

            if (studentQuiz == null)
            {
                return QuizApiResponse.Fail("Quiz does not exist");
            }

            if (studentQuiz.StudentId != studentId)
            {
                return QuizApiResponse.Fail("Invalid Reqest");
            }

            var isSelectedOptionCorrect = await _context.Options.Where(x => x.QuestionId == dto.QuestionId && x.Id == dto.OptionId)
                .Select(x => x.IsCorrect)
                .FirstOrDefaultAsync();

            if (isSelectedOptionCorrect)
            {
                studentQuiz.Score++;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return QuizApiResponse.Fail(ex.Message);
                }
            }

            return QuizApiResponse.Success();
        }


        public async Task<QuizApiResponse> SubmitQuizAsync(int studentQuizId, int studentId)
        {
            return await CompleteQuizAsync(studentQuizId, DateTime.UtcNow, nameof(StudentQuizStatus.Completed), studentId);
        }

        public async Task<QuizApiResponse> ExitQuizAsync(int studentQuizId, int studentId)
        {
            return await CompleteQuizAsync(studentQuizId, null, nameof(StudentQuizStatus.Exited), studentId);
        }

        public async Task<QuizApiResponse> AutoSubmitQuizAsync(int studentQuizId, int studentId)
        {
            return await CompleteQuizAsync(studentQuizId, DateTime.UtcNow, nameof(StudentQuizStatus.AutoSubmitted), studentId);
        }

        private async Task<QuizApiResponse> CompleteQuizAsync(int studentQuizId, DateTime? completedOn, string status, int studentId)
        {
            var studentQuiz = await _context.StudentQuizzes.AsTracking()
               .FirstOrDefaultAsync(x => x.Id == studentQuizId);

            if (studentQuiz == null)
            {
                return QuizApiResponse.Fail("Quiz does not exist");
            }

            if (studentQuiz.CompletedOn.HasValue || studentQuiz.Status == nameof(StudentQuizStatus.Exited))
            {
                return QuizApiResponse.Fail("Quiz Already Submitted");
            }

            if (studentQuiz.StudentId != studentId)
            {
                return QuizApiResponse.Fail("Invalid Reqest");
            }

            try
            {
                studentQuiz.CompletedOn = completedOn;
                studentQuiz.Status = status;
                var studentQuestions = await _context.StudentQuizQuestions.Where(x => x.StudentQuizId == studentQuizId).ToListAsync();

                _context.StudentQuizQuestions.RemoveRange(studentQuestions);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return QuizApiResponse.Fail(ex.Message);
            }


            return QuizApiResponse.Success();

        }

    }
}
