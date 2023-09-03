using MultiDeviceQrLogin.Exceptions;
using MultiDeviceQrLogin.Interfaces;
using Npgsql;

namespace MultiDeviceQrLogin.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly DataContext _dbContext;
    private readonly JwtSecurityTokenHandlerWrapper _jwtSecurityTokenHandlerWrapper;

    public AuthenticationService(DataContext dataContext, JwtSecurityTokenHandlerWrapper jwtSecurityTokenHandlerWrapper)
    {
        _dbContext = dataContext;
        _jwtSecurityTokenHandlerWrapper = jwtSecurityTokenHandlerWrapper;
    }

    public async Task<ApiResponse<AuthenticationResponseDTO>> Login(LoginDataRequestDTO loginDataRequestDTO)
    {
        var response = new ApiResponse<AuthenticationResponseDTO>();

        try
        {
            // Finding the User from the database using their email address.
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == loginDataRequestDTO.Email);

            // Checking if the user is found.
            if (user != null)
            {
                // Verifying the entered password against the stored password hash.
                if (VerifyPassword(loginDataRequestDTO.Password, user))
                {
                    // Generating a JWT token for successful login.
                    var token = _jwtSecurityTokenHandlerWrapper.GenerateJwtToken(user.UserId.ToString(), "user");
                    response.Success = true;
                    response.Message = "Login Sucessfull";
                    response.Data = new AuthenticationResponseDTO()
                    {
                        UserId = user.UserId,
                        UserName = user.FirstName + ' '+ user.LastName,
                        Token = token

                    };

                }
                else
                {
                    throw new ApplicationException("Invalid Password");
                }
            }
            else
            {
                throw new NotFoundException("User not found");
            }
            return response;
        }
        catch (Exception)
        {
            throw;
        }
    }



    public async Task<ApiResponse<AuthenticationResponseDTO>> Register(RegisterDataRequestDTO registerDataRequestDTO)
    {
        var response = new ApiResponse<AuthenticationResponseDTO>();

        try
        {
            // Generate Salt and Hash password.
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string password = registerDataRequestDTO.Password;
            string saltedPassword = password + salt;
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(saltedPassword);

            // Initializing the new user.
            var user = new UserModel { Email = registerDataRequestDTO.Email, PasswordHash = hashedPassword, Salt = salt, FirstName = registerDataRequestDTO.FirstName, LastName = registerDataRequestDTO.LastName };

            // Saving the user to database.
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            Console.WriteLine("Data got created");
          
            // Generate authorization token.
            var token = _jwtSecurityTokenHandlerWrapper.GenerateJwtToken(user.UserId.ToString(), "user");
          
            response.Success = true;
            response.Message = "User sucessfully registered";
            response.Data = new AuthenticationResponseDTO
            {
                UserId = user.UserId,
                UserName = user.FirstName + ' ' + user.LastName,
                Token = token
            };
            return response;
        }
        catch (Exception ex)
        {   
            // Finding exception for database unique constraint which indicates the user already exists.
            if (ex.InnerException != null && ex.InnerException is NpgsqlException npgsqlEx && npgsqlEx.SqlState == "23505")
            {
                throw new ConflictDataException("Email is already registered!");
            }
            throw;
        }

    }

    private bool VerifyPassword(string enteredPassword, UserModel user)
    {
        // Combining the entered password with the user's salt and verifying it against the stored password hash.
        string saltedEnteredPassword = enteredPassword + user.Salt;
        bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(saltedEnteredPassword, user.PasswordHash);
        return isPasswordCorrect;
    }
}