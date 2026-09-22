namespace Shared.Responce;
public enum ErrorType
{
    ItemNotFound,
    ItemUnavailable,
    OwnItem,
    AlreadyInCart,
    Unauthorized,
    DatabaseError
}
public class Result
{
    public bool IsSuccess { get; }
    public ErrorType? ErrorType { get; }
    public string? ErrorMessage { get; }

    private Result(
        bool isSuccess,
        ErrorType? errorType = null,
        string? errorMessage = null)
    {
        IsSuccess = isSuccess;
        ErrorType = errorType;
        ErrorMessage = errorMessage;
    }

    public static Result Success()
        => new(true);

    public static Result Failure(
        ErrorType errorType,
        string errorMessage)
        => new(false, errorType, errorMessage);
}