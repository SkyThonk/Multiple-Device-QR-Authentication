using MultiDeviceQrLogin.Exceptions;
using MultiDeviceQrLogin.Interfaces;

namespace MultiDeviceQrLogin.Services;

public class UserService : IUserService
{
    private readonly DataContext _dataContext;

    public UserService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public async Task<ApiResponse<UserDetailsResponseDTO>> getUserDetails(int userId)
    {
        var response = new ApiResponse<UserDetailsResponseDTO>();
        try
        {
            
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            // Checking if user is found.
            if (user != null)
            {
                response.Success = true;
                response.Message = "Sucessfull";
                response.Data = new UserDetailsResponseDTO
                {
                    UserId = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                };
                return response;
            }
            else
            {
                throw new NotFoundException("User does not exist");
            }
        }
        catch (Exception)
        {

            throw;
        }
    }
}