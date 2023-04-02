using System.Net;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;

namespace Nexttag.Aws.S3.Services;

public class Aws3Services : IAws3Services
{
    private readonly string _bucketName;
    private readonly IAmazonS3 _awsS3Client;

    public Aws3Services(string awsAccessKeyId, string awsSecretAccessKey, string awsSessionToken, string region,
        string bucketName)
    {
        _bucketName = bucketName;
        _awsS3Client = new AmazonS3Client(awsAccessKeyId, awsSecretAccessKey,
            RegionEndpoint.GetBySystemName(region));
    }

    public async Task<byte[]> DownloadFileAsync(string file)
    {
        MemoryStream ms = null;

        try
        {
            var getObjectRequest = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = file
            };

            using (var response = await _awsS3Client.GetObjectAsync(getObjectRequest))
            {
                if (response.HttpStatusCode == HttpStatusCode.OK)
                {
                    using (ms = new MemoryStream())
                    {
                        await response.ResponseStream.CopyToAsync(ms);
                    }
                }
            }

            if (ms is null || ms.ToArray().Length < 1)
                throw new FileNotFoundException($"O documento '{file}' nao foi encontrado");

            return ms.ToArray();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> UploadFileAsync(IFormFile file, string nomeDoArquivo = "")
    {
        try
        {
            using (var newMemoryStream = new MemoryStream())
            {
                await file.CopyToAsync(newMemoryStream);

                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = newMemoryStream,
                    Key =  (string.IsNullOrEmpty(nomeDoArquivo) ?  file.FileName : nomeDoArquivo ),
                    BucketName = _bucketName,
                    ContentType = file.ContentType
                };

                var fileTransferUtility = new TransferUtility(_awsS3Client);

                await fileTransferUtility.UploadAsync(uploadRequest);

                return true;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    
    
        
    
    

    public Task<bool> DeleteFileAsync(string fileName, string versionId = "")
    {
        var exist = IsFileExists(fileName, versionId);

        if (exist)
        {
            DeleteFile(fileName, versionId);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
       
    }

    private async Task DeleteFile(string fileName, string versionId)
    {
        var request = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = fileName
        };

        if (!string.IsNullOrEmpty(versionId))
            request.VersionId = versionId;

        await _awsS3Client.DeleteObjectAsync(request);
    }

    private bool IsFileExists(string fileName, string versionId)
    {
        try
        {
            var request = new GetObjectMetadataRequest()
            {
                BucketName = _bucketName,
                Key = fileName,
                VersionId = !string.IsNullOrEmpty(versionId) ? versionId : null
            };

            var response = _awsS3Client.GetObjectMetadataAsync(request).Result;

            return true;
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null && ex.InnerException is AmazonS3Exception awsEx)
            {
                if (string.Equals(awsEx.ErrorCode, "NoSuchBucket"))
                    return false;

                else if (string.Equals(awsEx.ErrorCode, "NotFound"))
                    return false;
            }

            throw;
        }
    }
}