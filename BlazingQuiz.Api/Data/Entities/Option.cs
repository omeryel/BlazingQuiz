using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazingQuiz.Api.Data.Entities
{
    public class Option
    {
        [Key]
        public int Id { get; set; }        
        public int QuestionId { get; set; }
        [MaxLength(200)]
        public string Text { get; set; }
        public bool IsCorrect { get; set; }


        [ForeignKey(nameof(QuestionId))]
        public Question Question { get; set; }

    }
}
