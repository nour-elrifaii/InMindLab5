using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LAb5.Application.ViewModels;

public class UploadProfilePictureDto
{
    [FromForm(Name = "file")]
    public IFormFile File { get; set; }
}