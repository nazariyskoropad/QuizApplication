using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizApplication.Contracts.Constants;
using QuizApplication.Contracts.Entities;

namespace QuizApplication.Infrastructure.AppContext.Persistence.Configuration
{
    public class TestAccessConfigConfiguration : IEntityTypeConfiguration<TestAccessConfig>
    {
        public void Configure(EntityTypeBuilder<TestAccessConfig> builder)
        {
            builder.HasKey(tac => tac.Id);
            builder.Property(tac => tac.UserName).HasMaxLength(PropertyConstrains.UserNameLength);
            builder.HasOne(tac => tac.Test)
                .WithMany(t => t.TestAccessConfigs)
                .HasForeignKey(tac => tac.TestId);
        }
    }
}
