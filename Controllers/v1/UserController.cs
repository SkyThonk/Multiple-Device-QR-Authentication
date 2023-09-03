using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiDeviceQrLogin.Interfaces;

namespace MultiDeviceQrLogin;

public class UserController: ControllerBase{
    private readonly IUserService _userService;
    private readonly JwtSecurityTokenHandlerWrapper _jwtSecurityTokenHandler;

    public UserController(IUserService userService, JwtSecurityTokenHandlerWrapper jwtSecurityTokenHandler){
        _userService = userService;
        _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
    }


    [HttpGet("userdetails")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<UserDetailsResponseDTO>>> getUserDetails()
    {
        var userId = HttpContext.Items["UserId"]?.ToString();
        return Ok( await _userService.getUserDetails(int.Parse(userId!)));
    }

}
