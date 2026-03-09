
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UserApi.Application.DTOs;

namespace UserApi.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var token = await _authService.Login(dto.Email, dto.Senha);
        return Ok(new { access_token = token });
    }

    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar(RegistrarUsuarioDto dto)
    {
        var token = await _authService.Registrar(dto);
        return Ok(new { access_token = token });
    }
}