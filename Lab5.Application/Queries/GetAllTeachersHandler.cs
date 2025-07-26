using System.Text.Json;
using AutoMapper;
using Lab5.Application.ViewModels;
using Lab5.Domain.Models;
using Lab5.Persistence.Data.Repositories;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace Lab5.Application.Queries;

public class GetAllTeachersHandler : IRequestHandler<GetAllTeachersQuery, List<TeacherDto>>
{
    private readonly IRepository<Teacher> _TeacherRepository;
    private readonly IDistributedCache _cache;
    private readonly IMapper _mapper;

    public GetAllTeachersHandler(IRepository<Teacher> TeacherRepository,
        IDistributedCache cache, IMapper mapper)
    {
        _TeacherRepository = TeacherRepository;
        _cache = cache;
        _mapper = mapper;
    }

    public async Task<List<TeacherDto>> Handle(GetAllTeachersQuery request, CancellationToken cancellationToken)
    {
        const string cacheKey = "teachers_all";
        var cached = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cached))
        {
            return JsonSerializer.Deserialize<List<TeacherDto>>(cached)!;
        }

        var teachers = await _TeacherRepository.GetAllAsync();
        var teacherDtos = _mapper.Map<List<TeacherDto>>(teachers);

        await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(teacherDtos), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        });

        return teacherDtos;
}
}