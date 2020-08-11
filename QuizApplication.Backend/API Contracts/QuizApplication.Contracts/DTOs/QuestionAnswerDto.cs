using QuizApplication.Contracts.Constants;
using System.ComponentModel.DataAnnotations;

namespace QuizApplication.Contracts.DTOs
{
    public class QuestionAnswerDto
    {
        public int QuestionId { get; set; }

        [Required]
        public bool IsCorrect { get; set; }

        [Required]
        [StringLength(PropertyConstrains.QuestionDescriptionLength)]
        public string Description { get; set; }
    }
}