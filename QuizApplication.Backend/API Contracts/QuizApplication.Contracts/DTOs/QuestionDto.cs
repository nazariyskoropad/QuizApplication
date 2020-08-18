using QuizApplication.Contracts.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuizApplication.Contracts.DTOs
{
    public class QuestionDto : BaseDto
    {
        public int TestId { get; set; }

        [Required]
        public int TimeLimit { get; set; }

        [Required]
        public double Points { get; set; }

        [Required]
        [StringLength(PropertyConstrains.QuestionDescriptionLength)]
        public string Description { get; set; }
        public ICollection<QuestionAnswerDto> QuestionAnswers { get; set; }
    }
}