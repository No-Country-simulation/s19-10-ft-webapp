using Microsoft.AspNetCore.Http;
using NotebookLM.Application.Contracts.Persistence;
using NotebookLM.Application.DTOs.Files;
using NotebookLM.Application.Exceptions;
using NotebookLM.Persistence.Data;
namespace NotebookLM.Persistence.Services;

public class FileService : IFileService
{
    private readonly NotebookLMDbContext _dbContext;
    private readonly string _filesDirectory;

    public FileService(NotebookLMDbContext dbContext)
    {
        _dbContext = dbContext;
        _filesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files");

        if (!Directory.Exists(_filesDirectory))
        {
            Directory.CreateDirectory(_filesDirectory);
        }
    }

    async public Task<AddFileResponseDto> AddFileAsync(int userId, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new BadRequestException("File is invalid");
        }

        var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
        var filePath = Path.Combine(_filesDirectory, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var fileEntity = new Domain.Entities.File
        {
            FileName = file.FileName,
            FilePath = filePath,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        _dbContext.Files.Add(fileEntity);
        await _dbContext.SaveChangesAsync();

        return new AddFileResponseDto("File added successfully");
    }
}
