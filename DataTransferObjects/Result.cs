namespace DataTransferObjects;

public class Result
{
    public bool Success { get; set; }

    public string? Message { get; set; }

    public string? Error { get; set; }

    public object? Data { get; set; }


    public int StatusCode = 200;

    public static Result SuccessResult(string message, object? data = null) =>
        new() { Success = true, Message = message, Data = data, StatusCode = 200 };

    public static Result DeletedResult(string message) =>
        new() { Success = true, Message = message, StatusCode = 200 };

    public static Result NotFoundResult(string message) =>
        new() { Success = false, Error = message, StatusCode = 404 };

    public static Result ErrorResult(string error, int statusCode = 500) =>
        new() { Success = false, Error = error, StatusCode = statusCode };
}
