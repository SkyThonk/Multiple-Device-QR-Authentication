// Define a standard ApiResponse class to wrap your response data.
namespace MultiDeviceQrLogin.Helpers;
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
}
