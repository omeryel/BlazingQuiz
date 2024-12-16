using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazingQuiz.Shared.DTOs
{
    public class QuizListDto
    {

        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        
        public int TotalQuestions { get; set; }
        
        public int TimeInMinutes { get; set; }
        public bool IsActive { get; set; }

    }
}
