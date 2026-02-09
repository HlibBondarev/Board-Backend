using Board.BusinessLogic.DTOs.Users;
using MediatR;

namespace Board.BusinessLogic.Features.ForUser.Queries;

public record GetAllQuery : IRequest<IEnumerable<UserResponseDto>>;