namespace MultiDeviceQrLogin.Exceptions;

class ValidationException: Exception {
    public ValidationException(string message) : base(message)
    {
    }
}