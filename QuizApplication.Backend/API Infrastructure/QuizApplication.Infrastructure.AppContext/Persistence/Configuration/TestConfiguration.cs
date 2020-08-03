using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizApplication.Contracts.Constants;
using QuizApplication.Contracts.Entities;

namespace QuizApplication.Infrastructure.AppContext.Persistence.Configuration
{
    public class TestConfiguration : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.UserName).HasMaxLength(PropertyConstrains.UserNameLength);
            builder.Property(t => t.Name).HasMaxLength(PropertyConstrains.TestNameLength);
            builder.Property(t => t.Description).HasMaxLength(PropertyConstrains.TestDescriptionLength);
        }
    }
}
