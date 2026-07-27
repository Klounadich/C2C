using Identity.DTO;
using MediatR;

namespace Identity.Commands;

public record TFARequestCommand(string userId):IRequest<TFAResponce>;