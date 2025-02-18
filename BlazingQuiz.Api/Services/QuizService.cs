﻿using BlazingQuiz.Api.Data;
using BlazingQuiz.Api.Data.Entities;
using BlazingQuiz.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BlazingQuiz.Api.Services
{
    public class QuizService
    {
        private readonly QuizContext _quizContext;
        public QuizService(QuizContext quizContext)
        {
            _quizContext = quizContext;
        }

        public async Task<QuizApiResponse> SaveQuizAsync(QuizSaveDto dto)
        {
            var questions = dto.Questions.Select(x => new Question()
            {
                Id = x.Id,
                Text = x.Text,
                Options = x.Options.Select(l => new Option
                {
                    Id = l.Id,
                    Text = l.Text,
                    IsCorrect = l.IsCorrect
                }).ToArray()
            }).ToArray();

            if (dto.Id == Guid.Empty)
            {


                var quiz = new Quiz()
                {
                    CategoryId = dto.CategoryId,
                    IsActive = dto.IsActive,
                    Name = dto.Name,
                    TimeInMinutes = dto.TimeInMinutes,
                    Questions = questions
                };

                _quizContext.Quizzes.Add(quiz);
            }
            else
            {
                var dbQuiz = await _quizContext.Quizzes.FirstOrDefaultAsync(x => x.Id == dto.Id);

                if (dbQuiz == null)
                {
                    return QuizApiResponse.Fail("Quiz does not exist");
                }
                else
                {
                    dbQuiz.CategoryId = dto.CategoryId;
                    dbQuiz.IsActive = dto.IsActive;
                    dbQuiz.Name = dto.Name;
                    dbQuiz.TotalQuestions = dto.TotalQuestions;
                    dbQuiz.TimeInMinutes = dto.TimeInMinutes;
                    dbQuiz.Questions = questions;

                    _quizContext.Quizzes.Update(dbQuiz);


                }
            }

            try
            {
                await _quizContext.SaveChangesAsync();
                return QuizApiResponse.Success();
            }
            catch (Exception ex)
            {
                return QuizApiResponse.Fail(ex.Message);
            }

        }

        public async Task<QuizListDto[]> GetQuizesAsync()
        {
            return await _quizContext.Quizzes.Select(x => new QuizListDto
            {
                Id = x.Id,
                Name = x.Name,
                TimeInMinutes = x.TimeInMinutes,
                TotalQuestions = x.TotalQuestions,
                IsActive = x.IsActive,
                CategoryName = x.Category.Name,
                CategoryId = x.CategoryId,
            }).ToArrayAsync();
        }

        public async Task<QuestionDto[]> GetQuizQuestions(Guid quizId)
        {
            return await _quizContext.Questions.Where(x => x.QuizId == quizId)
                 .Select(x => new QuestionDto
                 {
                     Id = x.Id,
                     Text = x.Text
                 }).ToArrayAsync();
        }

        public async Task<QuizSaveDto?> GetQuizToEditAsync(Guid quizId)
        {
            var quiz = await _quizContext.Quizzes
                .Where(q => q.Id == quizId)
                .Select(qz => new QuizSaveDto
                {
                    Id = qz.Id,
                    CategoryId = qz.CategoryId,
                    IsActive = qz.IsActive,
                    Name = qz.Name,
                    TimeInMinutes = qz.TimeInMinutes,
                    TotalQuestions = qz.TotalQuestions,
                    Questions = qz.Questions.Select(x => new QuestionDto
                    {
                        Id = x.Id,
                        Text = x.Text,
                        Options = x.Options.Select(o => new OptionDto
                        {
                            Id = o.Id,
                            Text = o.Text,
                            IsCorrect = o.IsCorrect,
                        }).ToList()
                    }).ToList()
                }).FirstOrDefaultAsync();

            return quiz;
        }
    }
}
