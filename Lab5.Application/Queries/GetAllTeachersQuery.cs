using Lab5.Application.ViewModels;
using MediatR;

namespace Lab5.Application.Queries;

public record GetAllTeachersQuery : IRequest<List<TeacherDto>>;