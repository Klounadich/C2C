using MediatR;

namespace Identity.Commands;

// Подтверждение кода при включении 2FA из настроек профиля.
// Юзер уже авторизован (auth_token-кука есть) — токены тут не нужны,
// просто флиппаем TwoFactorEnabled после проверки кода.
public record TwoFactorEnableConfirmCommand(Guid CodeId, string Code) : IRequest<bool>;
