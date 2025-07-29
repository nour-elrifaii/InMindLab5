
using System.Globalization;
using System.Runtime.InteropServices.JavaScript;
using AutoMapper;
using Lab5.Apllication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Lab5.Domain.Models;
using Lab5.Application.Mappers;
using Lab5.Persistence.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Lab5.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "student")]
public class StudentsController : ControllerBase
{
    private readonly UniversityContext  _context;
    private readonly ObjectMapperService _objectMapperService;
    private readonly IMapper _mapper;
    public StudentsController(UniversityContext context,ObjectMapperService mapperService, IMapper mapper)
    {
        _context = context;
        _objectMapperService = mapperService;
        _mapper = mapper;
    }
    

    [HttpGet]
    public IActionResult GetAllStudents()
    {
        var students = _context.Students.ToList();
        return Ok(_mapper.Map<List<StudentViewModel>>(students));
    }

    [HttpGet("student/{id}")]
    public IActionResult GetStudentById([FromRoute] long id)
    {
        var student = _context.Students.FirstOrDefault(s => s.Id == id);
        return Ok(_mapper.Map<StudentViewModel>(student));
    }

    [HttpGet("search")]
    public IActionResult GetStudentByName([FromQuery] string name)
    {
        var student = _context.Students.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
        return Ok( _mapper.Map<StudentViewModel>(student));
    }
    
    
    [HttpPost("AddStudent")]
    public async Task<IActionResult> AddStudent([FromBody] StudentDto dto)
    {
        var student = _mapper.Map<Student>(dto);
        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();
        return Ok(_mapper.Map<StudentViewModel>(student));
    }
    

}

