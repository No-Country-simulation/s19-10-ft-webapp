using Microsoft.AspNetCore.Http;
using NotebookLM.Application.DTOs.Files;
namespace NotebookLM.Application.Contracts.Persistence;

public interface IFileService
{
    Task<AddFileResponseDto> AddFileAsync(int userId, IFormFile file, int chatHistoryId);

    Task<FileStream> GetFileFromDb(Guid fileId);

}
