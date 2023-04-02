using Microsoft.AspNetCore.Http;

namespace Nexttag.Aws.S3.Services;

public interface IAws3Services
{
    Task<byte[]> DownloadFileAsync(string documento);

    Task<bool> UploadFileAsync(IFormFile documento , string nomeDoArquivo = "");

    Task<bool> DeleteFileAsync(string nomeDoArquivo, string versionId = "");
}