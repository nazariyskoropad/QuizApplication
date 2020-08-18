using System;
using System.Collections.Generic;
using System.Text;

namespace QuizApplication.Contracts.DTOs
{
    public class UserAnswersDto
    {
        public string UserName { get; set; }
        public ICollection<QApair> QApairs { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime EndedAt { get; set; }
    }

    public class QApair
    {
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
    }
}
