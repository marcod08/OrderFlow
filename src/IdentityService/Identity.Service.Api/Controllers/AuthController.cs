using Identity.Service.Api.Auth;
using Identity.Service.Api.DTOs;
using Identity.Service.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Service.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(UserManager<ApplicationUser> userManager, JwtTokenGenerator tokenGenerator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register (RegisterUserRequest request)
    {
        var user = new ApplicationUser {UserName = request.Email, Email = request.Email};

        var result = await userManager.CreateAsync(user, request.Password);

        if(!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        await userManager.AddToRoleAsync(user, "Customer");

        return Ok(new {message = "User registered successfully."});
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login (LoginUserRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
        {
            return Unauthorized (new {message = "Invalid email or password"});
        }

        var token = await tokenGenerator.GenerateTokenAsync(user, userManager);

        return Ok(new {token});
    }
}