using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace OnlineStore.Application.Services;
public class FileService : IFileService
{
    private readonly string _FOLDER_PATH;
    private readonly string[] _defaultFileExtensions = [".jpg", ".jpeg", ".png"];

    public FileService(IHostEnvironment environment)
    {
        _FOLDER_PATH = Path.Combine(environment.ContentRootPath, "Uploads");
    }
    public IHostEnvironment Environment { get; }

    public async Task<string> SaveFileAsync(IFormFile imageFile, string[]? allowedFileExtensions = null)
    {
        ArgumentNullException.ThrowIfNull(imageFile, nameof(imageFile));
        allowedFileExtensions = allowedFileExtensions ?? _defaultFileExtensions;

        if (!Directory.Exists(_FOLDER_PATH))
            Directory.CreateDirectory(_FOLDER_PATH);

        var ext = Path.GetExtension(imageFile.FileName);
        if (!allowedFileExtensions.Contains(ext))
            throw new ArgumentException($"Only {string.Join(",", allowedFileExtensions)} are allowed");

        var fileName = $"{Guid.NewGuid().ToString()}{ext}";
        var fileNameWithPath = Path.Combine(_FOLDER_PATH, fileName);

        using var stream = new FileStream(fileNameWithPath, FileMode.Create);
        await imageFile.CopyToAsync(stream);

        return fileName;

    }

    public void DeleteFile(string fileNameWithExtension)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(fileNameWithExtension);

        var path = Path.Combine(_FOLDER_PATH, fileNameWithExtension);

        if (!Path.Exists(path))
            throw new FileNotFoundException($"Invalid file path: {fileNameWithExtension}");

        File.Delete(path);
    }
}
