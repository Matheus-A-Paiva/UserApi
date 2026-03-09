using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserApi.Application.DTOs;
using UserApi.Application.Services;

namespace UserApi.API.Controllers;

[ApiController]
[Route("api/usuarios")]
[Authorize(Roles = "Admin")]
public class UsuarioController : ControllerBase
{
    private readonly UsuarioService _usuarioService;

    public UsuarioController(UsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodosUsuarios()
    {
        var usuarios = await _usuarioService.ObterTodosAsync();

        return Ok(usuarios);
    }

    [HttpGet("{id:guid}", Name = "ObterPorIdAsync")]
    public async Task<IActionResult> ObterPorIdAsync(Guid id)
    {
        var usuario = await _usuarioService.ObterPorIdAsync(id);

        return Ok(usuario);
    }

    [HttpPost]
    public async Task<IActionResult> AdicionarUsuario(RegistrarUsuarioDto usuarioDto)
    {
        var usuario = await _usuarioService.AdicionarAsync(usuarioDto);

        return CreatedAtRoute(
            "ObterPorIdAsync",
            new { id = usuario.Id },
            usuario);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> AtualizarUsuario(Guid id, AtualizarUsuarioDto dto)
    {
        var usuario = await _usuarioService.AtualizarAsync(id, dto);

        return Ok(usuario);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DesativarUsuario(Guid id)
    {
        await _usuarioService.DesativarAsync(id);

        return NoContent();
    }
}