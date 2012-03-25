using System.Collections.Generic;
using System.Linq;
using Amazon.CloudFront;
using Amazon.CloudFront.Model;

namespace Sitereactor.CloudProviders.Amazon
{
    public class CloudFrontFactory
    {
        private readonly AmazonCloudFront _client;

        public CloudFrontFactory(string accessKeyID, string secretAccessKeyID)
        {
            _client = GetAmazonCloudFrontClient(accessKeyID, secretAccessKeyID);
        }

        private static AmazonCloudFront GetAmazonCloudFrontClient(string accessKeyID, string secretAccessKeyID)
        {
            return global::Amazon.AWSClientFactory.CreateAmazonCloudFrontClient(accessKeyID, secretAccessKeyID);
        }

        public string CreateDistribution(string bucketName, bool enabled)
        {
            CloudFrontDistributionConfig config = new CloudFrontDistributionConfig();
            config.WithOrigin(bucketName)
                .WithEnabled(enabled);
            CreateDistributionRequest request = new CreateDistributionRequest();
            request.WithDistributionConfig(config);
            CreateDistributionResponse response = _client.CreateDistribution(request);
            return response.Distribution.DomainName;
        }

        public List<string> GetCloudFrontDistributionDomains()
        {
            ListDistributionsRequest request = new ListDistributionsRequest();
            ListDistributionsResponse response = _client.ListDistributions(request);
            return response.Distribution.Select(dist => dist.DomainName).ToList();
        }
    }
}
