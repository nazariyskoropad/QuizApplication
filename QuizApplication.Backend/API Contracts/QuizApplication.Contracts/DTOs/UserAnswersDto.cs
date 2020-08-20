using QuizApplication.Contracts.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QuizApplication.Contracts.DTOs
{
    public class UserAnswersDto
    {
        [Required]
        [StringLength(PropertyConstrains.UserNameLength)]
        public string UserName { get; set; }

        [Required]
        public ICollection<QApair> QApairs { get; set; }

        [Required]
        public DateTime StartedAt { get; set; }

        [Required]
        public DateTime EndedAt { get; set; }
    }

    public class QApair
    {
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
    }
}
