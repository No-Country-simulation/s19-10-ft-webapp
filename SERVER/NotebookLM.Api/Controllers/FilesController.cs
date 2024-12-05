using Microsoft.AspNetCore.Mvc;
using NotebookLM.Application.Contracts.Persistence;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]

public class FileController : ControllerBase
    {
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }

    /// <summary>
    /// Upload a file for a specific user.
    /// </summary>
    /// <remarks>
    /// This endpoint allows users to upload a file, with a maximum size of 10MB.
    /// The uploaded file will be stored on the server, and a record will be created in the database.
    /// </remarks>
    /// <param name="file">The file to upload.</param>
    /// <response code="200">File uploaded successfully.</response>
    /// <response code="400">The file is invalid or the request is incorrect.</response>
    /// <response code="500">An error occurred while uploading the file.</response>
    /// <returns>A confirmation message indicating that the file was uploaded successfully.</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))] // Adjust if you return a specific DTO
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost("")]
    [RequestSizeLimit(10 * 1024 * 1024)] // 10MB
    [Authorize(Roles = "User")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        try
        {
            var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var result = await _fileService.AddFileAsync(int.Parse(userId), file);
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
