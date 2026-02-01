namespace Pickpong.Models
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? ErrorCode { get; set; }
        public T? Value { get; set; }

        public static Result<T> SuccessResult(T value, string? message = null)
            => new Result<T> { Success = true, Value = value, Message = message ?? "Success" };

        public static Result<T> Failure(string message, string? errorCode = null)
            => new Result<T> { Success = false, Message = message, ErrorCode = errorCode };

        //public static Result<T> Failure(string message, string? errorCode, Exception ex)
        //    => new Result<T>
        //    {
        //        Success = false,
        //        Message = $"{message} שגיאה: {ex.Message}",
        //        ErrorCode = errorCode
        //    };

        public static Result<T> Failure(string msg, string? errorCode, Exception ex)
          => new()
          {
              Success = false,
              Message = $"{msg} שגיאה: {ex.InnerException?.Message ?? ex.Message}",
              ErrorCode = errorCode
          };
    }
    public class Result
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? ErrorCode { get; set; }

        public static Result SuccessResult(string msg = "Success") => new() { Success = true, Message = msg };

        public static Result Failure(string msg, string? errorCode = null) =>
            new() { Success = false, Message = msg, ErrorCode = errorCode };

        //public static Result Failure(string msg, string? errorCode, Exception ex)
        //    => new() { Success = false, Message = $"{msg} שגיאה: {ex.Message}", ErrorCode = errorCode };

        public static Result Failure(string msg, string? errorCode, Exception ex)
            => new()
            {
                Success = false,
                Message = $"{msg} שגיאה: {ex.InnerException?.Message ?? ex.Message}",
                ErrorCode = errorCode
            };
    }

}