namespace SocialNetwork.BuildingBlocks;

public sealed record Error(string Code, string Message);

public sealed record Result<T>(T? Value, Error? Error)
{
    public bool IsSuccess => Error is null;

    public static Result<T> Success(T value) => new(value, null);

    public static Result<T> Failure(string code, string message) =>
        new(default, new Error(code, message));
}
