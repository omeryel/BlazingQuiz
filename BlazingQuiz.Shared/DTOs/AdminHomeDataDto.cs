using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazingQuiz.Shared.DTOs
{
    public record AdminHomeDataDto(int TotalCategories, int TotalStudents, int ApprovedStudents, int TotalQuizes, int ActiveQuizes);

}
