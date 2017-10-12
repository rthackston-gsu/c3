using System;

using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;

namespace magic.gsu.edu
{
    class CreateS3
    {
        static string bucketName = "magic.bucket.one";
        public static void Main(string[] args)
        {
            using (var client = new AmazonS3Client(Amazon.RegionEndpoint.USWest2))
            {
                if (!(AmazonS3Util.DoesS3BucketExist(client, bucketName)))
                {
                    Console.Write("Creating Bucket");
                    CreateABucket(client);
                }
                string bucketLocation = FindBucketLocation(client);
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        static string FindBucketLocation(IAmazonS3 client)
        {
            GetBucketLocationRequest request = new GetBucketLocationRequest()
            {
                BucketName = bucketName
            };
            GetBucketLocationResponse response = client.GetBucketLocation(request); 
            return response.Location.ToString();
        }

        static void CreateABucket(IAmazonS3 client)
        {
            try
            {
                PutBucketRequest putRequest = new PutBucketRequest
                {
                    BucketName = bucketName,
                    UseClientRegion = true
                };
                PutBucketResponse response = client.PutBucket(putRequest);
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId")
                    ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity"))
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