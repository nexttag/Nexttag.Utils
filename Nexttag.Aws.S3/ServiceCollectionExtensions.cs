using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Nexttag.Aws.S3.Services;

namespace Nexttag.Aws.S3;

public static class ServiceCollectionExtensions
{
    public static void AddAwsS3Service(this IServiceCollection services, IConfiguration configuration)
    {
        var aws3Services = new Aws3Services(configuration["aws:s3:accessKey"],
            configuration["aws:s3:secretAccessKey"],
            configuration["aws:s3:sessionToken"],
            configuration["aws:s3:region"],
            configuration["aws:s3:bucketName"]);

        services.AddTransient<IAws3Services, Aws3Services>(x => aws3Services);
    }
}