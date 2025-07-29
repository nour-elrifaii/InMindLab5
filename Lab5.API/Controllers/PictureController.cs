using Azure.Storage.Blobs;
using LAb5.Application.ViewModels;
using Lab5.Persistence.Data;
using Microsoft.AspNetCore.Mvc;

namespace Lab5.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PictureController : ControllerBase
{
    private readonly UniversityContext _context;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly IConfiguration _configuration;

    public PictureController(UniversityContext context, BlobServiceClient blobServiceClient, IConfiguration configuration)
    {
        _context = context;
        _blobServiceClient = blobServiceClient;
        _configuration=configuration;
    }

    [HttpPost("{id}/upload")]
    public async Task<IActionResult> Upload([FromRoute] long id, [FromForm] UploadProfilePictureDto dto)

    {
        var student=await _context.Students.FindAsync(id);
        if (student == null)
        {
            return NotFound("Student with ID not found");
        }

        if (dto.File.Length == 0 ||  dto.File== null)
        {
            return BadRequest("File is empty");
        }
        
        var containerName = _configuration["AzureStorage:ContainerName"];
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync();
        
        var uniqueName = $"{Guid.NewGuid()}_{dto.File.FileName}";
        student.ProfileBlobName = uniqueName;
        var blobClient = containerClient.GetBlobClient(uniqueName);

        using  (var fileStream = dto.File.OpenReadStream())
        {
            await blobClient.UploadAsync(fileStream, new Azure.Storage.Blobs.Models.BlobHttpHeaders
            {
                ContentType = dto.File.ContentType
            });
        }
        
        await _context.SaveChangesAsync();
        return Ok(new {Message = $"Uploaded {dto.File.FileName}"});
    }

    [HttpGet("{Id}/download")]
    public async Task<IActionResult> Download(long Id)
    {
        var student=await _context.Students.FindAsync(Id);
        var containerName = _configuration["AzureStorage:ContainerName"];
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(student.ProfileBlobName);
        if (student.ProfileBlobName == null || student == null || student.ProfileBlobName.Length == 0)
        {
            return NotFound("Student or profile not found");
        }

        if (!await blobClient.ExistsAsync())
        {
            return NotFound("Profile not found");
        }
        
        var download = await blobClient.DownloadAsync();

        return File(download.Value.Content, download.Value.Details.ContentType ?? "application/octet-stream", fileDownloadName:student.ProfileBlobName);
        
    }
    
}