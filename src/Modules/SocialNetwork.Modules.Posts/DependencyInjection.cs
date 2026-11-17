using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.Modules.Posts.Infrastructure;

namespace SocialNetwork.Modules.Posts;

public static class DependencyInjection
{
    public static IServiceCollection AddPostsModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Postgres")
            ?? throw new InvalidOperationException("ConnectionStrings:Postgres is not configured.");

        services.AddDbContext<PostsDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }

    public static IEndpointRouteBuilder MapPostsModule(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/posts")
            .WithTags("Posts");

        group.MapGet("/", PostEndpoints.GetAll);
        group.MapGet("/{id:guid}", PostEndpoints.GetById);
        group.MapPost("/", PostEndpoints.Create);
        group.MapPut("/{id:guid}", PostEndpoints.Update);
        group.MapDelete("/{id:guid}", PostEndpoints.Delete);

        return endpoints;
    }

    public static async Task InitializePostsDatabaseAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<PostsDbContext>();
        await db.Database.EnsureCreatedAsync();
    }
}
