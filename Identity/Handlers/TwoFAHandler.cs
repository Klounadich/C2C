using System.Security.Cryptography;
using System.Text;
using Identity.Commands;
using Identity.DTO;
using Identity.Repositories;
using Identity.Services.SMTP;
using Identity.Services.SMTP.Models;
using MailKit;
using MediatR;

namespace Identity.Handlers;

// Раньше этот хендлер ВКЛЮЧАЛ 2FA (Enable2FA) ДО подтверждения кода —
// то есть флаг TwoFactorEnabled взводился уже на этапе "запросить код",
// а не после того, как юзер его реально ввёл и подтвердил.
// Теперь хендлер отвечает только за одно: сгенерировать код и отправить письмо.
// Само включение 2FA переехало в TwoFactorEnableConfirmHandler (после проверки кода).
// Это же позволяет безопасно переиспользовать хендлер и для логина —
// там TwoFactorEnabled уже true, его трогать вообще не нужно.
public class TwoFAHandler : IRequestHandler<TFARequestCommand, TFAResponce>
{
    private readonly IUserRepository _userRepository;
    private readonly ISMTPSerivce _mailService;

    public TwoFAHandler(IUserRepository userRepository, ISMTPSerivce mailService)
    {
        _userRepository = userRepository;
        _mailService = mailService;
    }

    public async Task<TFAResponce> Handle(TFARequestCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdAsync(request.userId);
        if (user is null)
        {
            return new TFAResponce { CodeSent = false };
        }

        string rand_confirm_number = new Random().Next(1000, 9999).ToString();
        var bytess = SHA256.HashData(Encoding.UTF8.GetBytes(rand_confirm_number));
        var code_hash = Convert.ToHexString(bytess);

        var mailData = new MailData
        {
            To = new List<string> { user.Email },
            From = "PodberuConfirm@yandex.ru",
            DisplayName = "TradeHub",
            ReplyTo = "PodberuConfirm@yandex.ru",
            ReplyToName = "Поддержка TradeHub",
            Subject = "Код подтверждения 2FA",
            Body = rand_confirm_number
        };

        var sent = await _mailService.SendAsync(mailData, CancellationToken.None);

        if (sent)
        {
            var code = await _userRepository.SaveVerificationCodeAsync(code_hash, user.Id);
            return new TFAResponce
            {
                CodeId = code,
                CodeSent = true
            };
        }

        return new TFAResponce
        {
            CodeSent = false,
        };
    }
}
