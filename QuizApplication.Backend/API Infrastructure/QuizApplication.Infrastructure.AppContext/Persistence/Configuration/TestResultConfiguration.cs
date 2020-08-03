using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizApplication.Contracts.Constants;
using QuizApplication.Contracts.Entities;

namespace QuizApplication.Infrastructure.AppContext.Persistence.Configuration
{
    public class TestResultConfiguration : IEntityTypeConfiguration<TestResult>
    {
        public void Configure(EntityTypeBuilder<TestResult> builder)
        {
            builder.HasKey(tr => tr.Id);
            builder.Property(tr => tr.UserName).HasMaxLength(PropertyConstrains.UserNameLength);
            builder.HasOne(tr => tr.Test)
                .WithMany(t => t.TestResults)
                .HasForeignKey(tr => tr.TestId);
        }
    }
}
