using Lab5.Application.ViewModels;
using MediatR;

namespace Lab5.Application.Queries;

public record GetAllStudentsQuery() : IRequest<List<StudentDto>>;