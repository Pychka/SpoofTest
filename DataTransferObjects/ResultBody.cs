namespace DataTransferObjects;

public class ResultBody
{
    public bool Success { get; set; }

    public string? Message { get; set; }

    public string? Error { get; set; }


    public int StatusCode = 200;

    public string? Body { get; set; }

    public static ResultBody SuccessResult(string message, string body) => new() { Success = true, Message = message, Body = body, StatusCode = 200 };
    public static ResultBody DeletedResult(string message) => new() { Success = true, Message = message, StatusCode = 200 };
    public static ResultBody NotFoundResult(string message) => new() { Success = false, Error = message, StatusCode = 404 };
    public static ResultBody ErrorResult(string error, int statusCode = 500) => new() { Success = false, Error = error, StatusCode = statusCode };
}
