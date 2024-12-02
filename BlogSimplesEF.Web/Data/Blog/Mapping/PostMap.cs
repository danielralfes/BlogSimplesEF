using BlogSimplesEF.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogSimplesEF.Web.Data.Blog.Mapping;

public class PostMap : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Posts");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .HasMaxLength(70)
            .IsRequired();

        builder.Property(x => x.Summary)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.Content)
            .HasColumnType("NVARCHAR(MAX)");

        builder.Property(x => x.UserId)
            .HasColumnType("nvarchar(450)")
            .IsRequired();

        builder.HasOne(x => x.UserBlog)
    .WithMany()
    .HasForeignKey(x => x.UserId)
    .IsRequired();

    }
}
