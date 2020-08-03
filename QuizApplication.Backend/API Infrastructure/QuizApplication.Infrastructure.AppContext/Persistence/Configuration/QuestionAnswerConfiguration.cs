using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizApplication.Contracts.Constants;
using QuizApplication.Contracts.Entities;

namespace QuizApplication.Infrastructure.AppContext.Persistence.Configuration
{
    public class QuestionAnswerConfiguration : IEntityTypeConfiguration<QuestionAnswer>
    {
        public void Configure(EntityTypeBuilder<QuestionAnswer> builder)
        {
            builder.HasKey(qa => qa.Id);
            builder.Property(qa => qa.Description).HasMaxLength(PropertyConstrains.AnswerDescriptionLength);
            builder.HasOne(qa => qa.Question)
                .WithMany(q => q.QuestionAnswers)
                .HasForeignKey(qa => qa.QuestionId);
        }
    }
}
