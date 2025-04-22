namespace CommertialPortal_WebAPI;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }

    private ApiResponse(bool success, T? data, string? errorMessage)
    {
        Success = success;
        Data = data;
        ErrorMessage = errorMessage;
    }

    public static ApiResponse<T> SuccessResponse(T data)
    {
        return new ApiResponse<T>(true, data, null);
    }

    public static ApiResponse<T> FailureResponse(string errorMessage)
    {
        return new ApiResponse<T>(false, default, errorMessage);
    }
}
