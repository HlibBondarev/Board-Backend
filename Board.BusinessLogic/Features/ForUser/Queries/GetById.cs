using Board.BusinessLogic.DTOs.Users;
using MediatR;

namespace Board.BusinessLogic.Features.ForUser.Queries;

public record GetById(int Id) : IRequest<UserResponseDto>;