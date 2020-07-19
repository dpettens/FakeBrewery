namespace FakeBrewery.Application
{
    public static class Result
    {
        public static Result<T> Success<T>(T value)
        {
            return new Result<T>(true, value);
        }

        public static Result<T> Failure<T>(T value, ResultErrorCode errorCode, string errorMessage)
        {
            return new Result<T>(false, value, errorCode, errorMessage);
        }
    }

    public class Result<T>
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public ResultErrorCode? ErrorCode { get; }
        public string ErrorMessage { get; }
        public T Value { get; }

        public Result(bool isSuccess, T value, ResultErrorCode? errorCode = null, string errorMessage = null)
        {
            IsSuccess = isSuccess;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            Value = value;
        }
    }
}