using Microsoft.AspNetCore.Mvc;
using NotebookLM.Application.Contracts.Persistence;
namespace API.Controllers;

[ApiController]
[Route("api/users/{userId}/files")]
public class FileController : ControllerBase
    {
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("")]
    [RequestSizeLimit(10 * 1024 * 1024)] // 10MB
    public async Task<IActionResult> Upload([FromRoute] int userId, IFormFile file)
    {
        try
        {
            var result = await _fileService.AddFileAsync(userId, file);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
            catch (Exception)
        {
                return StatusCode(500, "An error occurred while uploading the file");
            }
        }
    }
