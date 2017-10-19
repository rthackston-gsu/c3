using System;

using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;

namespace magic.gsu.edu
{
    public static class CreateS3
    {
        private const string BucketName = "magic.bucket.two";

        public static void GetAvailableBucket()
        {
            using (var client = new AmazonS3Client(Amazon.RegionEndpoint.USWest2))
            {
                if (!(AmazonS3Util.DoesS3BucketExist(client, BucketName)))
                {
                    Console.Write("Creating Bucket");
                    CreateABucket(client);
                }
                var bucketLocation = FindBucketLocation(client);
                Console.WriteLine(bucketLocation);
            }
        }

        private static string FindBucketLocation(IAmazonS3 client)
        {
            var request = new GetBucketLocationRequest()
            {
                BucketName = BucketName
            };
            var response = client.GetBucketLocation(request); 
            return response.Location.ToString();
        }

        private static void CreateABucket(IAmazonS3 client)
        {
            try
            {
                var putRequest = new PutBucketRequest
                {
                    BucketName = BucketName,
                    UseClientRegion = true
                };
                var response = client.PutBucket(putRequest);
                Console.WriteLine(response.ResponseMetadata);
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null && 
                    (amazonS3Exception.ErrorCode != null &&
                     amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                     amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    Console.WriteLine("Check the provided AWS Credentials.");
                    Console.WriteLine("For service sign up go to http://aws.amazon.com/s3");
                }
                else
                {
                    Console.WriteLine("Error occurred. Message: '{0}' when writing an object",
                        amazonS3Exception.Message);
                }
            }
        }
    }
}