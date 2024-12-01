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
        public int TotalQuesions { get; set; }

        [Range(1, 120, ErrorMessage = "Please provide valid time in minutes.")]
        public int TimeInMinutes { get; set; }
        public bool IsActive { get; set; }

        public List<QuestionDto> Questions { get; set; } = [];

    }
}
