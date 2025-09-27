using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Application.Providers;
public interface IFileUrlBuilder
{
    string BuildFileUrl(string fileName);
}

public class FileUrlBuilder: IFileUrlBuilder
{
    private readonly string _baseUrl;

    public FileUrlBuilder(IConfiguration config)
    {
        _baseUrl = config["FileSettings:baseUrl"]!;
    }

    public string BuildFileUrl(string fileName) => $"{_baseUrl}/{fileName}";
}
