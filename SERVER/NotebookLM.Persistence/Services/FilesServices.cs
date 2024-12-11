using Microsoft.AspNetCore.Http;
using NotebookLM.Application.Contracts.Persistence;
using NotebookLM.Application.DTOs.Files;
using NotebookLM.Application.Exceptions;
using NotebookLM.Persistence.Data;
using System.IO;
namespace NotebookLM.Persistence.Services;

public class FileService : IFileService
{
    private readonly NotebookLMDbContext _dbContext;
    private readonly string _filesDirectory;
    private readonly KernelMemoryService _kernelMemoryService;

    public FileService(NotebookLMDbContext dbContext, KernelMemoryService kernelMemoryService)
    {
        _dbContext = dbContext;
        _filesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files");

        if (!Directory.Exists(_filesDirectory))
        {
            Directory.CreateDirectory(_filesDirectory);
        }

        _kernelMemoryService = kernelMemoryService;

    }

    public async Task<AddFileResponseDto> AddFileAsync(int userId, IFormFile file, int chatHistoryId)
    {

        try
        {
            if (file == null || file.Length == 0)
            {
                throw new BadRequestException("File is invalid");
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(_filesDirectory, uniqueFileName);


            var stream = new FileStream(filePath, FileMode.Create);


            await file.CopyToAsync(stream);



            var fileEntity = new Domain.Entities.File
            {
                FileName = file.FileName,
                FilePath = filePath,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                chatHistoryId = chatHistoryId
            };

            _dbContext.Files.Add(fileEntity);
            await _dbContext.SaveChangesAsync();

            await _kernelMemoryService.ImportEmbeddingsToDB(filePath, Guid.Parse(fileEntity.Id.ToString()), chatHistoryId,
                stream, Guid.Parse(userId.ToString()));

            return new AddFileResponseDto("File added successfully");

        } catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
        
    }

    public async Task<FileStream> GetFileFromDb(Guid fileId)
    {
        var file = await _dbContext.Files.FindAsync(fileId);

        if (file == null)
        {
            throw new Exception("File not found");
        }

        return new FileStream(file.FilePath, FileMode.Open, FileAccess.Read);
    }
}
