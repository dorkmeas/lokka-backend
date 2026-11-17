using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Modules.Posts.Domain;
using SocialNetwork.Modules.Posts.Infrastructure;

namespace SocialNetwork.Modules.Posts.Features;

internal static class PostEndpoints
{
    public static async Task<IResult> GetAll(
        PostsDbContext db,
        int page = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var posts = await db.Posts
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new PostResponse(
                x.Id, x.AuthorId, x.Content, x.Visibility, x.CreatedAt, x.UpdatedAt))
            .ToListAsync(cancellationToken);

        return Results.Ok(new { items = posts, page, pageSize });
    }

    public static async Task<IResult> GetById(
        Guid id,
        PostsDbContext db,
        CancellationToken cancellationToken)
    {
        var post = await db.Posts.AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new PostResponse(
                x.Id, x.AuthorId, x.Content, x.Visibility, x.CreatedAt, x.UpdatedAt))
            .FirstOrDefaultAsync(cancellationToken);

        return post is null ? Results.NotFound() : Results.Ok(post);
    }

    public static async Task<IResult> Create(
        CreatePostRequest request,
        PostsDbContext db,
        CancellationToken cancellationToken)
    {
        if (request.AuthorId == Guid.Empty)
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                ["authorId"] = ["AuthorId is required."]
            });

        if (string.IsNullOrWhiteSpace(request.Content) || request.Content.Trim().Length > 5000)
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                ["content"] = ["Content is required and must not exceed 5000 characters."]
            });

        if (request.Visibility is not ("Public" or "Followers" or "Private"))
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                ["visibility"] = ["Visibility must be Public, Followers or Private."]
            });

        var post = Post.Create(request.AuthorId, request.Content, request.Visibility);
        db.Posts.Add(post);
        await db.SaveChangesAsync(cancellationToken);

        var response = PostResponse.From(post);
        return Results.Created($"/api/posts/{post.Id}", response);
    }

    public static async Task<IResult> Update(
        Guid id,
        UpdatePostRequest request,
        PostsDbContext db,
        CancellationToken cancellationToken)
    {
        var post = await db.Posts.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (post is null)
            return Results.NotFound();

        try
        {
            post.Update(request.Content, request.Visibility);
        }
        catch (ArgumentException exception)
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                ["request"] = [exception.Message]
            });
        }

        await db.SaveChangesAsync(cancellationToken);
        return Results.Ok(PostResponse.From(post));
    }

    public static async Task<IResult> Delete(
        Guid id,
        PostsDbContext db,
        CancellationToken cancellationToken)
    {
        var post = await db.Posts.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (post is null)
            return Results.NotFound();

        db.Posts.Remove(post);
        await db.SaveChangesAsync(cancellationToken);
        return Results.NoContent();
    }
}
