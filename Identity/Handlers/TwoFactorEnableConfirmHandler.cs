using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Identity.Commands;
using Identity.Repositories;
using MediatR;

namespace Identity.Handlers;

public class TwoFactorEnableConfirmHandler : IRequestHandler<TwoFactorEnableConfirmCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public TwoFactorEnableConfirmHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(TwoFactorEnableConfirmCommand request, CancellationToken cancellationToken)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(request.Code));
        var codeHashed = Convert.ToHexString(bytes);

        // Переиспользуем существующую проверку кода — там уже есть
        // лимит попыток (5) и проверка срока действия, дублировать не нужно.
        var userIdStr = await _userRepository.VerificateEmailAsync(request.CodeId, codeHashed);

        var user = await _userRepository.GetUserByIdAsync(userIdStr);
        if (user is null)
            throw new ValidationException("Пользователь не найден.");

        // Единственное место, где TwoFactorEnabled реально становится true —
        // строго после успешной проверки кода, не раньше.
        return await _userRepository.Enable2FA(user);
    }
}
