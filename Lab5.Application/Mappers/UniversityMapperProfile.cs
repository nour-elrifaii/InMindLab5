using AutoMapper;
using Lab5.Domain.Models;
using Lab5.Apllication.ViewModels;

namespace Lab5.Application.Mappers;

public class UniversityMappingProfile : Profile
{
    public UniversityMappingProfile()
    {
        CreateMap<Student, StudentViewModel>()
            .ForMember(d => d.Id,   e => e.MapFrom(src => src.Id))
            .ForMember(d => d.Name, e => e.MapFrom(src => src.Name));
        
        CreateMap<CourseClass, CourseClassViewModel>()
            .ForMember(d => d.CourseClassId, e => e.MapFrom(src => src.CourseClassId))
            .ForMember(d => d.CourseClassName, e => e.MapFrom(src => src.Course.CourseName))
            .ForMember(d => d.TeacherName, e => e.MapFrom(src => src.Teacher.TeacherName));
        
        CreateMap<Course, CourseViewModel>()
            .ForMember(d => d.CourseID, e => e.MapFrom(src => src.CourseID))
            .ForMember(d => d.CourseName, e => e.MapFrom(src => src.CourseName));
        
        CreateMap<Teacher, TeacherViewModel>()
            .ForMember(d => d.TeacherId,   e => e.MapFrom(src => src.TeacherId))
            .ForMember(d => d.TeacherName, e => e.MapFrom(src => src.TeacherName));
        
        CreateMap<StudentDto, Student>()
            .ForMember(d => d.Name,  e => e.MapFrom(src => src.StudentName))
            .ForMember(d => d.Email, e => e.MapFrom(src => src.Email));

        CreateMap<TeacherDto, Teacher>()
            .ForMember(d => d.TeacherName, e => e.MapFrom(src => src.TeacherName));

        CreateMap<CourseDto, Course>()
            .ForMember(d => d.CourseName, e => e.MapFrom(src => src.CourseName))
            .ForMember(d => d.TeacherId,  e => e.MapFrom(src => src.TeacherId));

        CreateMap<CourseClassDto, CourseClass>()
            .ForMember(d => d.TeacherId, e => e.MapFrom(src => src.TeacherID))
            .ForMember(d => d.CourseId,  e => e.MapFrom(src => src.CourseID));
    }
}