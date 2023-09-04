using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MultiDeviceQrLogin.Interfaces;

namespace MultiDeviceQrLogin.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("fixed")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService){
        _authenticationService = authenticationService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthenticationResponseDTO>>> Login(LoginDataRequestDTO loginDataRequest){
        if(!ModelState.IsValid)
        {
            throw new ValidationException();
        } 

       return  Ok(await _authenticationService.Login(loginDataRequest)) ;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<AuthenticationResponseDTO>>> Register(RegisterDataRequestDTO registerDataRequest){
        if(!ModelState.IsValid){
            throw new ValidationException();
        }

        return Ok(await _authenticationService.Register(registerDataRequest));
    }
}
