namespace MultiDeviceQrLogin.Interfaces;

public interface IUserService {
    Task<ApiResponse<UserDetailsResponseDTO>> getUserDetails(int userId);
}