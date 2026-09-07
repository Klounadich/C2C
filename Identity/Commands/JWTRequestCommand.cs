namespace Identity.Commands;

public record JWTRequestCommand(string UserId, string Username, string Email , DateTime RegistrationDate , bool twoFA);
