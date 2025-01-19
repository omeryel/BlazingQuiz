using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazingQuiz.Shared.DTOs
{
    public record StudentQuizQuestionResponseDto(int StudentQuizId, int QuestionId, int OptionId);
    
    
}
