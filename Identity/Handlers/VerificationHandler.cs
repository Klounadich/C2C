using Identity.Commands;
using Identity.DTO;
using Identity.Repositories;
using MediatR;

namespace Identity.Handlers;

public class VerificationHandler : IRequestHandler<EmailVerificationCommand , UserResponse>
{
    private readonly IUserRepository _userRepository;
    public VerificationHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public Task<UserResponse> Handle(EmailVerificationCommand request, CancellationToken cancellationToken)
    {
        bool verified = _userRepository.VerificateEmailAsync(request.Email , request.code);
    }
}