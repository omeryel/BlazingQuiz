using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazingQuiz.Shared.DTOs
{
    public class QuizSaveDto
    {
        public Guid Id { get; set; }


        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Category is required.")]
        public int CategoryId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please provide valid number of questions.")]
        public int TotalQuestions { get; set; }

        [Range(1, 120, ErrorMessage = "Please provide valid time in minutes.")]
        public int TimeInMinutes { get; set; }
        public bool IsActive { get; set; }

        public List<QuestionDto> Questions { get; set; } = [];

        public string? TryValidate()
        {
            if (TotalQuestions != Questions.Count)
            {
                return "Number of questions does not match with Total Questions";

            }

            if (Questions.Any(x => string.IsNullOrWhiteSpace(x.Text)))
            {
                return "Question text is required";
            }

            if (Questions.Any(x => x.Options.Count < 2))
            {
                return "At least 2 Options are required for each questions";
            }

            if (Questions.Any(x => !x.Options.Any(o => o.IsCorrect)))
            {
                return "All options should have correct answer marked";
            }
            if (Questions.Any(x => x.Options.Any(o => string.IsNullOrWhiteSpace(o.Text))))
            {
                return "All options should have text";
            }


            return null;
        }

    }
}
