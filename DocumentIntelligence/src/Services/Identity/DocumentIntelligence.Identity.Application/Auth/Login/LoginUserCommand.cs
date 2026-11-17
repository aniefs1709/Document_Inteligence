using MediatR;

namespace DocumentIntelligence.Identity.Application.Auth.Login;

public sealed record LoginUserCommand(
    string Email,
    string Password) : IRequest<LoginUserResult>;