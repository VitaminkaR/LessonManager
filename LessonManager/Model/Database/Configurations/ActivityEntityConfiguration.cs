using LessonManager.Model.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LessonManager.Model.Database.Configurations
{
    internal class ActivityEntityConfiguration : IEntityTypeConfiguration<ActivityEntity>
    {
        public void Configure(EntityTypeBuilder<ActivityEntity> builder)
        {
            builder.HasKey(a => a.ID);
            builder.Property(a => a.Name).IsRequired();
            builder.Property(a => a.ActivityTime).IsRequired();
            builder.Property(a => a.State).IsRequired();
            builder.Property(a => a.Type).IsRequired();

            builder.HasOne(a => a.Subject).WithMany(s => s.Activities).IsRequired();
        }
    }
}