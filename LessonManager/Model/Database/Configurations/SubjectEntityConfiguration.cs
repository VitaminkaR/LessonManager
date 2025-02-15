using LessonManager.Model.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace LessonManager.Model.Database.Configurations
{
    internal class SubjectEntityConfiguration : IEntityTypeConfiguration<SubjectEntity>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<SubjectEntity> builder)
        {
            builder.HasKey(s => s.ID);
            builder.Property(s => s.Name).IsRequired();
            builder.Property(s => s.Exam).IsRequired();
            builder.Property(s => s.ExamDate).IsRequired();
            builder.Property(s => s.ExamMark).IsRequired();

            builder.HasMany(a => a.Activities).WithOne(s => s.Subject).IsRequired();
        }
    }
}