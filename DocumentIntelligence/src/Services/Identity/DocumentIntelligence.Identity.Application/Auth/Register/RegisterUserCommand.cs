using MediatR;

namespace DocumentIntelligence.Identity.Application.Auth.Register;

public sealed record RegisterUserCommand(
    string Email,
    string Password,
    string DisplayName) : IRequest<RegisterUserResult>;