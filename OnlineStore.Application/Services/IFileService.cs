using Microsoft.AspNetCore.Http;

namespace OnlineStore.Application.Services;
public interface IFileService
{
    Task<string> SaveFileAsync(IFormFile file, string[]? allowedFileExtensions = null);
    void DeleteFile(string fileNameWithExtension);
}
