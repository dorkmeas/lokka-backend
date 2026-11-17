namespace SocialNetwork.Modules.Posts.Domain;

public sealed class Post
{
    private Post() { }

    public Guid Id { get; private set; }
    public Guid AuthorId { get; private set; }
    public string Content { get; private set; } = string.Empty;
    public string Visibility { get; private set; } = "Public";
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public static Post Create(Guid authorId, string content, string visibility)
    {
        if (authorId == Guid.Empty)
            throw new ArgumentException("AuthorId is required.", nameof(authorId));

        var normalizedContent = content?.Trim() ?? string.Empty;
        if (normalizedContent.Length is < 1 or > 5000)
            throw new ArgumentException("Post content must contain 1 to 5000 characters.", nameof(content));

        if (visibility is not ("Public" or "Followers" or "Private"))
            throw new ArgumentException("Visibility must be Public, Followers or Private.", nameof(visibility));

        var now = DateTimeOffset.UtcNow;
        return new Post
        {
            Id = Guid.NewGuid(),
            AuthorId = authorId,
            Content = normalizedContent,
            Visibility = visibility,
            CreatedAt = now,
            UpdatedAt = now
        };
    }

    public void Update(string content, string visibility)
    {
        var normalizedContent = content?.Trim() ?? string.Empty;
        if (normalizedContent.Length is < 1 or > 5000)
            throw new ArgumentException("Post content must contain 1 to 5000 characters.", nameof(content));

        if (visibility is not ("Public" or "Followers" or "Private"))
            throw new ArgumentException("Visibility must be Public, Followers or Private.", nameof(visibility));

        Content = normalizedContent;
        Visibility = visibility;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
