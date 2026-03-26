using AuthApi.Api.Controllers.UsuarioController.Models;
using AuthApi.Application.Commands.UsuarioCommands.CadastrarUsuarioCommand;
using AuthApi.Application.Commands.UsuarioCommands.EditarUsuarioCommand;
using AuthApi.Application.Commands.UsuarioCommands.ExcluirUsuarioCommand;
using AuthApi.Application.Commands.UsuarioCommands.LoginCommand;
using AuthApi.Application.Commands.UsuarioCommands.RefreshTokenCommand;
using AuthApi.Application.Interfaces;
using AuthApi.Domain.Aggregates.Usuario;
using AuthApi.Domain.Aggregates.Usuario.Dtos;
using AuthApi.Infra.Daos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthApi.Api.Controllers.UsuarioController;

[ApiController]
[Produces("application/json")]
[ApiVersion("1.0")]
[Route("api/v{version:ApiVersion}/[controller]")]
public class UsuarioController : Controller
{
    private readonly IMediator _mediator;
    private readonly ITokenService _tokenService;
    public UsuarioController(IMediator mediator, ITokenService tokenService)
    {
        _mediator = mediator;
        _tokenService = tokenService;
    }


    [Authorize]
    [HttpGet("teste")]
    public IActionResult Teste()
    {
        return Ok(User.Claims.Select(c => new { c.Type, c.Value }));
    }

    [HttpPost("cadastrar-admin")]
    [ProducesResponseType(typeof(Guid), 201)]
    [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CadastrarAdm([FromBody] CadastrarUsuarioModel model)
    {
        var usuarioId = await _mediator.Send(new CadastrarUsuarioCommand(model.Nome, model.Email, model.Senha, TipoUsuario.Admin));

        return CreatedAtAction(nameof(BuscarById), new { id = usuarioId }, usuarioId);
    }

    [HttpPost("cadastrar-comum")]
    [ProducesResponseType(typeof(Guid), 201)]
    [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CadastrarComum([FromBody] CadastrarUsuarioModel model)
    {
        var usuarioId = await _mediator.Send(new CadastrarUsuarioCommand(model.Nome, model.Email, model.Senha, TipoUsuario.Comum));

        return CreatedAtAction(nameof(BuscarById), new { id = usuarioId }, usuarioId);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel model)
    {
        var login = await _mediator.Send(new LoginCommand(model.Email, model.Senha));

        return Ok(login);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenModel model)
    {
        var result = await _mediator.Send(new RefreshTokenCommand(model.RefreshToken));
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> EditarTipoUsuario([FromRoute] Guid id, [FromBody] string tipoUsuario)
    {
        var tipo = Enum.Parse<TipoUsuario>(tipoUsuario);

        await _mediator.Send(new EditarTipoUsuarioCommand(id, tipo));

        return NoContent();

    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> ExcluirUsuario([FromRoute] Guid id)
    {
        await _mediator.Send(new ExcluirUsuarioCommand(id));

        return NoContent();
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetUsuarioLogado([FromServices] IUsuarioDao usuarioDao)
    {
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

        if (userEmail == null)
            return Unauthorized();

        var usuario = await usuarioDao.UsuarioDtoByEmail(userEmail);

        if (usuario == null)
            return NotFound();

        return Ok(usuario);
    }

    [Authorize]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UsuarioDto), 200)]
    public async Task<IActionResult> BuscarById([FromServices] IUsuarioDao usuarioDao, [FromRoute] Guid id)
    {
        return Ok(await usuarioDao.UsuarioDtoById(id));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UsuarioDto>), 200)]
    public async Task<IActionResult> ListarUsuarios([FromServices] IUsuarioDao usuarioDao)
    {
        return Ok(await usuarioDao.ListarUsuarios());
    }

}
