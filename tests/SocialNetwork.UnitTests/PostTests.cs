using SocialNetwork.Modules.Posts.Domain;

namespace SocialNetwork.UnitTests;

public sealed class PostTests
{
    [Fact]
    public void Create_ShouldTrimContent_AndSetDefaults()
    {
        var post = Post.Create(Guid.NewGuid(), "  Hello world  ", "Public");

        Assert.Equal("Hello world", post.Content);
        Assert.Equal("Public", post.Visibility);
        Assert.NotEqual(Guid.Empty, post.Id);
    }

    [Fact]
    public void Create_ShouldRejectEmptyContent()
    {
        Assert.Throws<ArgumentException>(() =>
            Post.Create(Guid.NewGuid(), "   ", "Public"));
    }

    [Fact]
    public void Update_ShouldChangeContentAndTimestamp()
    {
        var post = Post.Create(Guid.NewGuid(), "Old content", "Public");
        post.Update("New content", "Followers");

        Assert.Equal("New content", post.Content);
        Assert.Equal("Followers", post.Visibility);
    }
}
