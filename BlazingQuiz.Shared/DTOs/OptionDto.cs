using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazingQuiz.Shared.DTOs
{
    public class OptionDto
    {
        public int Id { get; set; }        
        [Required, MaxLength(200)]
        public string Text { get; set; }
        public bool IsCorrect { get; set; }

    }
}
