using System.Text.Json.Serialization;

namespace Domain.Common.Results;

public class Result
{
    // Domain constructor — keeps your validation logic
    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None
          || !isSuccess && error == Error.None)
            throw new ArgumentException("Invalid Error", nameof(Error));

        IsSuccess = isSuccess;
        Error = error;
    }

    // Separate deserialization constructor — skips validation intentionally
    // Data coming from cache was already valid when it was stored
    [JsonConstructor]
    protected Result(bool isSuccess, Error error, bool isFailure)
    {
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
    // Domain success constructor
    public Result(TResult data) : base(true, Error.None)
    {
        Data = data;
    }

    // Domain failure constructor
    public Result(Error error) : base(false, error) { }

    // Single deserialization constructor — handles BOTH success and failure
    // Parameter names must match JSON property names (case-insensitive)
    [JsonConstructor]
    public Result(TResult data, bool isSuccess, Error error, bool isFailure)
        : base(isSuccess, error, isFailure)
    {
        Data = data;
    }

    public TResult? Data { get; }
}