using DocumentIntelligence.Identity.Application.Auth.Login;
using DocumentIntelligence.Identity.Application.Auth.Register;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace DocumentIntelligence.Identity.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public sealed class AuthController(ISender sender) : ControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType(
        typeof(RegisterUserResponse),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(
        [FromBody] RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(
            request.Email,
            request.Password,
            request.DisplayName);

        var result = await sender.Send(command, cancellationToken);

        if (!result.Succeeded)
        {
            if (result.ErrorCode == "email_already_registered")
            {
                return Conflict(new
                {
                    code = result.ErrorCode,
                    message = result.ErrorMessage
                });
            }

            return BadRequest(new
            {
                code = result.ErrorCode,
                message = result.ErrorMessage
            });
        }

        return StatusCode(
            StatusCodes.Status201Created,
            result.User);
    }

    [HttpPost("login")]
    [ProducesResponseType(
        typeof(LoginUserResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromBody] LoginUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginUserCommand(
            request.Email,
            request.Password);

        var result = await sender.Send(command, cancellationToken);

        if (!result.Succeeded)
        {
            return Unauthorized(new
            {
                code = result.ErrorCode,
                message = result.ErrorMessage
            });
        }

        return Ok(result.User);
    }
}
