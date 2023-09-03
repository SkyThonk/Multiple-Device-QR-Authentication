namespace MultiDeviceQrLogin.Interfaces;

public interface IAuthenticationService
{
    Task<ApiResponse<AuthenticationResponseDTO>> Login(LoginDataRequestDTO loginDataRequestDTO);
    Task<ApiResponse<AuthenticationResponseDTO>> Register(RegisterDataRequestDTO registerDataRequestDTO);
}
