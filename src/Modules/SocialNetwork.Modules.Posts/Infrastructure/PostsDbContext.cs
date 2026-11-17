using Microsoft.EntityFrameworkCore;
using SocialNetwork.Modules.Posts.Domain;

namespace SocialNetwork.Modules.Posts.Infrastructure;

public sealed class PostsDbContext(DbContextOptions<PostsDbContext> options) : DbContext(options)
{
    public DbSet<Post> Posts => Set<Post>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var post = modelBuilder.Entity<Post>();

        post.ToTable("posts");
        post.HasKey(x => x.Id);
        post.Property(x => x.Id).HasColumnName("id");
        post.Property(x => x.AuthorId).HasColumnName("author_id").IsRequired();
        post.Property(x => x.Content).HasColumnName("content").HasMaxLength(5000).IsRequired();
        post.Property(x => x.Visibility).HasColumnName("visibility").HasMaxLength(20).IsRequired();
        post.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        post.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();
        post.HasIndex(x => x.AuthorId);
        post.HasIndex(x => x.CreatedAt);
    }
}
