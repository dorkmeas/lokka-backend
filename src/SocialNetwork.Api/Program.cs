using SocialNetwork.Modules.Identity;
using SocialNetwork.Modules.Posts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddIdentityModule();
builder.Services.AddPostsModule(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy",
    service = "SocialNetwork.Api",
    timestamp = DateTimeOffset.UtcNow
})).WithTags("System");

app.MapIdentityModule();
app.MapPostsModule();

await app.InitializePostsDatabaseAsync();

app.Run();

public partial class Program { }
