namespace Domain.Common.Results;

public class Result
{
    protected Result(bool isSuccess, Error error)
    {

        if (isSuccess && error != Error.None
          || !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid Error", nameof(Error));
        }
        IsSuccess = isSuccess;
        Error = error;
    }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }
    public static Result Success() => new(true, Error.None);
    public static Result Fail(Error error) => new(false, error);
    public static Result<TResult> Success<TResult>(TResult data) => new(data);
    public static Result<TResult> Fail<TResult>(Error error) => new(error);
}

public class Result<TResult> : Result
{
    public TResult Data { get; }
    public Result(TResult data)
        : base(true, Error.None)
    {
        Data = data;
    }
    public Result(Error error) : base(false, error) { }
}