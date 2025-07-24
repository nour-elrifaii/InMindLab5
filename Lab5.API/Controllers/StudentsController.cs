
using System.Globalization;
using System.Runtime.InteropServices.JavaScript;
using AutoMapper;
using Lab5.Apllication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Lab5.Domain.Models;
using Lab5.Application.Mappers;
using Lab5.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Lab5.API.Controllers;

[ApiController]
[Route("[controller]")]
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

    [HttpPost("AddTeacher")]
    public async Task<IActionResult> AddTeacher([FromBody] TeacherDto dto)
    {
        var teacher = _mapper.Map<Teacher>(dto);
        await _context.Teachers.AddAsync(teacher);
        await _context.SaveChangesAsync();
        return Ok(_mapper.Map<TeacherViewModel>(teacher));
    }
    
    [HttpPost("AddStudent")]
    public async Task<IActionResult> AddStudent([FromBody] StudentDto dto)
    {
        var student = _mapper.Map<Student>(dto);
        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();
        return Ok(_mapper.Map<StudentViewModel>(student));
    }
    [HttpPost("AddCourse")]
    public async Task<IActionResult> AddCourse([FromBody] CourseClassDto dto)
    {
        var courseClass = _mapper.Map<CourseClass>(dto);
        await _context.CourseClasses.AddAsync(courseClass);
        await _context.SaveChangesAsync();

        var fullClass = await _context.CourseClasses
            .Include(cc => cc.Teacher)
            .Include(cc => cc.Course)
            .FirstAsync(cc => cc.CourseClassId == courseClass.CourseClassId);

        return Ok(_mapper.Map<CourseClassViewModel>(fullClass));
    }
    
    public class FileUploadDto
    {
        public IFormFile File { get; set; }
    }

    
    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage([FromForm] FileUploadDto dto)
    {
        var file = dto.File;
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var path = Path.Combine("wwwroot/uploads", file.FileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);
        }
        catch(IOException e)
        {
            return StatusCode(500, "There was an error uploading the file.");
        }
        return Ok(new { path });
    }

    
    [HttpDelete("delete/{id}")]
    public IActionResult DeleteStudent([FromRoute] long id)
    {
        var student = _context.Students.FirstOrDefault(s => s.Id == id);
        _context.Students.Remove(student);
        _context.SaveChanges();
        return Ok($"student {id} has been deleted");
    }

    [HttpPost("mapStudentToPerson")]
    public IActionResult MapStudentToPerson([FromBody] Student student)
    {
        if (student == null)
            return BadRequest("Student is null");

        Person person = _objectMapperService.Map<Student, Person>(student);
        return Ok(person);
    }
    

}

