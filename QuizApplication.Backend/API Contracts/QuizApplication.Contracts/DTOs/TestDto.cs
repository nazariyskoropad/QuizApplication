using QuizApplication.Contracts.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuizApplication.Contracts.DTOs
{
    public class TestDto : BaseDto
    {
        [Required]
        public int RunsNumber { get; set; }

        [Required]
        public int TimeLimit { get; set; }

        [Required]
        public double Points { get; set; }

        [Required]
        [StringLength(PropertyConstrains.TestNameLength)]
        public string Name { get; set; }

        [StringLength(PropertyConstrains.TestDescriptionLength)]
        public string Description { get; set; }

        [StringLength(PropertyConstrains.UserNameLength)]
        public string UserName { get; set; }

        [Required]
        public DateTime StartsAt { get; set; }

        [Required]
        public DateTime EndsAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<QuestionDto> Questions { get; set; }
    }
}
