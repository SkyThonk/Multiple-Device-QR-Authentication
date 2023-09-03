namespace MultiDeviceQrLogin.Exceptions;
public class SqlException : Exception
{
    public SqlException(string message) : base(message)
    {
    }
}