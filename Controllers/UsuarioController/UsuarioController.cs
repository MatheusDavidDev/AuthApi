using AuthApi.Api.Controllers.UsuarioController.Models;
using AuthApi.Application.Commands.UsuarioCommands.CadastrarUsuarioCommand;
using AuthApi.Domain.Aggregates.Usuario;
using AuthApi.Domain.Aggregates.Usuario.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Api.Controllers.UsuarioController;

[ApiController]
[Produces("application/json")]
[ApiVersion("1.0")]
[Route("api/v{version:ApiVersion}/[controller]")]
public class UsuarioController : Controller
{
    private readonly IMediator _mediator;

    public UsuarioController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("cadastrar-admin")]
    [ProducesResponseType(typeof(Guid), 201)]
    [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CadastrarAdm([FromBody] CadastrarUsuarioModel model)
    {
        var command = new CadastrarUsuarioCommand(model.Nome, model.Email, model.Senha, TipoUsuario.Admin);
        var usuarioId = await _mediator.Send(command);
        return CreatedAtAction(nameof(BuscarById), new { id = usuarioId }, usuarioId);
    }

    [HttpPost("cadastrar-comum")]
    [ProducesResponseType(typeof(Guid), 201)]
    [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CadastrarComum([FromBody] CadastrarUsuarioModel model)
    {
        var command = new CadastrarUsuarioCommand(model.Nome, model.Email, model.Senha, TipoUsuario.Comum);
        var usuarioId = await _mediator.Send(command);
        return CreatedAtAction(nameof(BuscarById), new { id = usuarioId }, usuarioId);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UsuarioDto), 200)]
    public async Task<IActionResult> BuscarById([FromServices] IUsuarioDao usuarioDao, [FromRoute] Guid id)
    {
        var response = await usuarioDao.UsuarioById(id);
        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UsuarioDto>), 200)]
    public async Task<IActionResult> ListarUsuarios([FromServices] IUsuarioDao usuarioDao)
    {
        var response = await usuarioDao.ListarUsuarios();
        return Ok(response);
    }

}
