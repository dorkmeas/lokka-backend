using SocialNetwork.Modules.Posts.Domain;

namespace SocialNetwork.Modules.Posts.Features;

public sealed record PostResponse(
    Guid Id,
    Guid AuthorId,
    string Content,
    string Visibility,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt)
{
    public static PostResponse From(Post post) => new(
        post.Id,
        post.AuthorId,
        post.Content,
        post.Visibility,
        post.CreatedAt,
        post.UpdatedAt);
}

public sealed record CreatePostRequest(Guid AuthorId, string Content, string Visibility = "Public");

public sealed record UpdatePostRequest(string Content, string Visibility = "Public");
