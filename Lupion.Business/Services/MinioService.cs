using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;

namespace Lupion.Business.Services
{
    public class MinioService
    {
        private readonly IMinioClient _minio;

        public MinioService(IConfiguration config)
        {
            _minio = new MinioClient()
                .WithEndpoint("cheetaherp.net", 9000)
                .WithCredentials(config["Minio:AccessKey"], config["Minio:SecretKey"])
                .WithSSL(bool.Parse(config["Minio:UseSSL"] ?? "false"))
                .Build();
        }


        public async Task UploadStreamAsync(string bucketName, string objectName, Stream data, long size, string contentType)
        {
            bool found = await _minio.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName));
            if (!found)
                await _minio.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName));

            await _minio.PutObjectAsync(new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithStreamData(data)
                .WithObjectSize(size)
                .WithContentType(contentType));
        }

        public async Task<string> GetFileUrlAsync(string bucketName, string objectName, int expirySeconds)
        {
            return await _minio.PresignedGetObjectAsync(
                new PresignedGetObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName)
                    .WithExpiry(expirySeconds)
            );
        }
    }
}
