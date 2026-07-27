using Identity.DTO;
using MediatR;

namespace Identity.Commands;

// Подтверждение кода на ВТОРОМ шаге логина (юзер ещё НЕ авторизован —
// auth-куки на этом этапе нет, поэтому [Authorize] тут не применим,
// в отличие от TwoFactorEnableConfirmCommand).
public record TwoFactorLoginVerifyCommand(Guid CodeId, string Code) : IRequest<UserResponse>;