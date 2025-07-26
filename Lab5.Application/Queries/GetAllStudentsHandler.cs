using System.Text.Json;
using AutoMapper;
using Lab5.Application.ViewModels;
using Lab5.Domain.Models;
using Lab5.Persistence.Data;
using Lab5.Persistence.Data.Repositories;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using StackExchange.Redis;

namespace Lab5.Application.Queries;

public class GetAllStudentsHandler : IRequestHandler<GetAllStudentsQuery, List<StudentDto>>
{
    private readonly IRepository<Student> _StudentRepository;
    private readonly IDistributedCache _cache;
    private readonly IMapper _mapper;

    public GetAllStudentsHandler(IRepository<Student> StudentRepository,
        IDistributedCache cache, IMapper mapper)
    {
        _StudentRepository = StudentRepository;
        _cache = cache;
        _mapper = mapper;
    }
    
    public async Task<List<StudentDto>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
    {
        const string cacheKey = "all_students";
        var cached = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cached))
        {
            return JsonSerializer.Deserialize<List<StudentDto>>(cached)!;
        }

        var students = await _StudentRepository.GetAllAsync();
        var studentDtos = _mapper.Map<List<StudentDto>>(students);

        await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(studentDtos), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        });

        return studentDtos;
    }
}