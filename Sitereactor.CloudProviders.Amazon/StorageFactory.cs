using System.Collections.Generic;
using System.IO;
using System.Linq;
using Amazon.S3;
using Amazon.S3.Model;

namespace Sitereactor.CloudProviders.Amazon
{
    public class StorageFactory
    {
        private readonly AmazonS3 _client;
        private readonly string _accessKeyID;
        private readonly string _secretAccessKeyID;

        public StorageFactory(string accessKeyID, string secretAccessKeyID)
        {
            _client = GetAmazonS3Client(accessKeyID, secretAccessKeyID);
            _accessKeyID = accessKeyID;
            _secretAccessKeyID = secretAccessKeyID;
        }

        private static AmazonS3 GetAmazonS3Client(string accessKeyID, string secretAccessKeyID)
        {
            AmazonS3Config config = new AmazonS3Config { CommunicationProtocol = Protocol.HTTP };
            AmazonS3 amazonS3Client = global::Amazon.AWSClientFactory.CreateAmazonS3Client(accessKeyID, secretAccessKeyID, config);
            return amazonS3Client;
        }

        public List<string> GetAllBuckets()
        {
            ListBucketsResponse listBucketsResponse = _client.ListBuckets();
            return listBucketsResponse.Buckets.Select(b => b.BucketName).ToList();
        }

        public Dictionary<string, long> GetAllObjectsByBucketName(string bucketName)
        {
            ListObjectsRequest request = new ListObjectsRequest {BucketName = bucketName};
            ListObjectsResponse response = _client.ListObjects(request);
            return response.S3Objects.ToDictionary(o => o.Key, o => o.Size);
        }

        public string GetObjectInformation(string bucketName, string objectKey)
        {
            GetObjectRequest request = new GetObjectRequest();
            request.WithBucketName(bucketName).WithKey(objectKey);
            GetObjectResponse response = _client.GetObject(request);
            return response.AmazonId2;
        }

        public void CreateBucket(string bucketName)
        {
            PutBucketRequest request = new PutBucketRequest {BucketName = bucketName};
            _client.PutBucket(request);
        }

        public void CreateObject(string bucketName, Stream fileStream)
        {
            PutObjectRequest request = new PutObjectRequest();
            request.WithBucketName(bucketName);
            request.WithInputStream(fileStream);
            request.CannedACL = S3CannedACL.PublicRead;

            S3Response response = _client.PutObject(request);
            response.Dispose();
        }

        public string CreateObject(string bucketName, Stream fileStream, string contentType, string objectKey)
        {
            PutObjectRequest request = new PutObjectRequest();
            request.WithBucketName(bucketName).WithContentType(contentType).WithKey(objectKey);
            request.WithInputStream(fileStream);
            request.CannedACL = S3CannedACL.PublicRead;

            S3Response response = _client.PutObject(request);
            response.Dispose();
            return response.RequestId;
        }

        public void CreateObject(string bucketName, string contentBody, string contentType, Stream fileStream)
        {
            PutObjectRequest request = new PutObjectRequest();
            request.WithContentBody(contentBody)
                .WithContentType(contentType)
                .WithBucketName(bucketName);
            request.WithInputStream(fileStream);
            request.CannedACL = S3CannedACL.PublicRead;

            S3Response response = _client.PutObject(request);
            response.Dispose();
        }

        public void CreateFolderAsObject(string bucketName, string objectKey)
        {
            bool create = true;
            var list = GetAllObjectsByBucketName(bucketName);
            foreach (var o in list)
            {
                if(o.Equals(objectKey))
                { create = false; }
            }

            if (!create) return;

            PutObjectRequest request = new PutObjectRequest();
            request.WithBucketName(bucketName)
                .WithKey(objectKey);
            request.InputStream = new MemoryStream();
            request.CannedACL = S3CannedACL.PublicRead;

            S3Response response = _client.PutObject(request);
            response.Dispose();
        }
    }
}
