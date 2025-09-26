
namespace OnlineStore.Application.Common;

public class Result
{
    public bool Success { get; protected set; }
    public string? Message { get; protected set; }
    public List<ErrorItem> Errors { get; protected set; } = new();
    public static Result Ok() => new() { Success = true };
    public static Result Fail(params List<ErrorItem> errors)
        => new() { Success = false, Message = "Validation Error", Errors = errors };
    public static Result Fail(string? message)
        => new() { Success = false, Message = message };

}

public class Result<T> : Result
{

    public T? Data { get; private set; }

    public static Result<T> Ok(T value) => new() { Success = true, Data = value };

    public new static Result<T> Fail(params List<ErrorItem> errors)
      => new() { Success = false, Message = "Validation Error", Errors = errors };
    public new static Result<T> Fail(string? message)
        => new() { Success = false, Message = message };


    public static implicit operator Result<T>(T data)
    {
        return Result<T>.Ok(data);
    }

}

public class ErrorItem
{
    private string _field = string.Empty;

    public string Field
    {
        get { return _field; }
        set
        {
            _field =
                char.ToLower(value[0]) + value.Substring(1);
        }
    }
    public string Message { get; set; } = string.Empty;
}