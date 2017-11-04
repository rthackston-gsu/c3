using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.SimpleDB;
using Amazon.SimpleDB.Model;
using Amazon.S3;
using Amazon.S3.Model;
using System.IO;
using System.Diagnostics;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using Amazon.S3.Transfer;
using Amazon.EC2.Util;
using Amazon.S3.Util;
using Amazon.IdentityManagement;
using Amazon.Auth.AccessControlPolicy.ActionIdentifiers;
using Amazon.Auth.AccessControlPolicy;
using Amazon.IdentityManagement.Model;

namespace App3.Utility
{
    class MagicServiceUtility
    {
        public MagicServiceUtility()
        { }
        public static string accessKeyId = "XXXXXXXXXX";
        public static string secretAccessKey = "XXXXXXXXXXXXXXX";

        
        private static RegionEndpoint region;
        private static IAmazonS3 s3Client;
        public static string BUCKET_NAME = "";

        public static async Task<string> GetServiceOutputAsync()
        {
            region = RegionEndpoint.USWest2;
            StringBuilder sb = new StringBuilder(1024);
            using (StringWriter sr = new StringWriter(sb))
            {
                sr.WriteLine("===========================================");
                sr.WriteLine("Welcome to the MAGIC HEALTH CHECK!");
                sr.WriteLine("===========================================");

               
                // Print the number of Amazon EC2 instances.
                IAmazonEC2 ec2 = new AmazonEC2Client(accessKeyId,secretAccessKey,region);
                Debug.WriteLine(ec2.Config.RegionEndpoint.ToString());
                DescribeInstancesRequest ec2Request = new DescribeInstancesRequest();

                try
                {
                    DescribeInstancesResponse ec2Response =await ec2.DescribeInstancesAsync(ec2Request);
                   
                        int numInstances = 0;
                        numInstances = ec2Response.Reservations.Count;
                        sr.WriteLine(string.Format("You have {0} Amazon EC2 instance(s) running in the {1} region.",
                                                   numInstances, "AWSRegion"));

                    

                }
                catch (AmazonEC2Exception ex)
                {
                    if (ex.ErrorCode != null && ex.ErrorCode.Equals("AuthFailure"))
                    {
                        sr.WriteLine("The account you are using is not signed up for Amazon EC2.");
                        sr.WriteLine("You can sign up for Amazon EC2 at http://aws.amazon.com/ec2");
                    }
                    else
                    {
                        sr.WriteLine("Caught Exception: " + ex.Message);
                        sr.WriteLine("Response Status Code: " + ex.StatusCode);
                        sr.WriteLine("Error Code: " + ex.ErrorCode);
                        sr.WriteLine("Error Type: " + ex.ErrorType);
                        sr.WriteLine("Request ID: " + ex.RequestId);
                    }
                }
                sr.WriteLine();

                // Print the number of Amazon SimpleDB domains.
                IAmazonSimpleDB sdb = new AmazonSimpleDBClient(accessKeyId, secretAccessKey, region);
                ListDomainsRequest sdbRequest = new ListDomainsRequest();

                try
                {
                    ListDomainsResponse sdbResponse = await sdb.ListDomainsAsync(sdbRequest);

                    int numDomains = 0;
                    numDomains = sdbResponse.DomainNames.Count;
                    sr.WriteLine(string.Format("You have {0} Amazon SimpleDB domain(s) in the {1} region.",
                                               numDomains, "AWSRegion"));
                }
                catch (AmazonSimpleDBException ex)
                {
                    if (ex.ErrorCode != null && ex.ErrorCode.Equals("AuthFailure"))
                    {
                        sr.WriteLine("The account you are using is not signed up for Amazon SimpleDB.");
                        sr.WriteLine("You can sign up for Amazon SimpleDB at http://aws.amazon.com/simpledb");
                    }
                    else
                    {
                        sr.WriteLine("Caught Exception: " + ex.Message);
                        sr.WriteLine("Response Status Code: " + ex.StatusCode);
                        sr.WriteLine("Error Code: " + ex.ErrorCode);
                        sr.WriteLine("Error Type: " + ex.ErrorType);
                        sr.WriteLine("Request ID: " + ex.RequestId);
                    }
                }
                sr.WriteLine();

                // Print the number of Amazon S3 Buckets.
                s3Client = new AmazonS3Client(accessKeyId, secretAccessKey, region);

                try
                {
                    ListBucketsResponse response = await s3Client.ListBucketsAsync();
                    int numBuckets = 0;
                    if (response.Buckets != null &&
                        response.Buckets.Count > 0)
                    {
                        numBuckets = response.Buckets.Count;
                    }
                    sr.WriteLine("You have " + numBuckets + " Amazon S3 bucket(s).");
                }
                catch (AmazonS3Exception ex)
                {
                    if (ex.ErrorCode != null && (ex.ErrorCode.Equals("InvalidAccessKeyId") ||
                        ex.ErrorCode.Equals("InvalidSecurity")))
                    {
                        sr.WriteLine("Please check the provided AWS Credentials.");
                        sr.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
                    }
                    else
                    {
                        sr.WriteLine("Caught Exception: " + ex.Message);
                        sr.WriteLine("Response Status Code: " + ex.StatusCode);
                        sr.WriteLine("Error Code: " + ex.ErrorCode);
                        sr.WriteLine("Request ID: " + ex.RequestId);
                    }
                }
             
            }
            return sb.ToString();
        }

        //in the future allow user enter the name
        public static async Task<string> createS3()
        {
            region = RegionEndpoint.USWest2;
            BUCKET_NAME = "magicbucket1234567";
            
            StringBuilder sb = new StringBuilder(1024);
            using (StringWriter sr = new StringWriter(sb))
            {
                sr.WriteLine("===========================================");
                sr.WriteLine("Creating S3 BUCKET!");
                sr.WriteLine("===========================================");
                s3Client = new AmazonS3Client(accessKeyId, secretAccessKey, region);
                try
                {
                    ListBucketsResponse response = await s3Client.ListBucketsAsync();
                    bool found = false;
                    foreach (S3Bucket bucket in response.Buckets)
                    {
                        if (bucket.BucketName == BUCKET_NAME)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (found == false)
                    {
                        PutBucketResponse x = await s3Client.PutBucketAsync(new PutBucketRequest() { BucketName = BUCKET_NAME });
                        sr.WriteLine(x.HttpStatusCode +"###" +x.ContentLength);
                        if ((""+x.HttpStatusCode).Equals("OK"))
                        {
                            found = true;
                        }
                    }
                    if(found == true){
                        //upload the files
                        sr.WriteLine("===========================================");
                        sr.WriteLine("Created S3Bucket, Now adding files");
                        sr.WriteLine("===========================================" );
                        FileOpenPicker openPicker = new FileOpenPicker();
                        openPicker.ViewMode = PickerViewMode.Thumbnail;
                        openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                        openPicker.FileTypeFilter.Add(".jpg");
                        openPicker.FileTypeFilter.Add(".png");
                        StorageFile file = await openPicker.PickSingleFileAsync();
                        if (file != null)
                        {
                            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                            var image = new BitmapImage();
                            image.SetSource(stream);

                            sr.WriteLine(stream.Size);
                            String genGuid = generateGUID();
                            String S3_KEY = "" + genGuid + "/" + "task.sh";
                            string task_file_text_content = Constants.task_file_text_content.Replace("<GUID>", genGuid);
                            sr.WriteLine(S3_KEY + "\n");
                            sr.WriteLine(task_file_text_content + "\n");

                            PutObjectRequest request = new PutObjectRequest();
                            request.BucketName = BUCKET_NAME;
                            request.Key = S3_KEY;
                            request.ContentBody = task_file_text_content;
                            PutObjectResponse x = await s3Client.PutObjectAsync(request);

                            sr.WriteLine("Response Code " + x.HttpStatusCode + "### Content Length" + x.ContentLength + "### Etag" + x.ETag);
                            if (("" + x.HttpStatusCode).Equals("OK"))
                            {
                                String S3_CONTROLLER_KEY = "" + genGuid + "/" + "controller.sh";
                                string controller_text_content = Constants.controller_text_content.Replace("<region_name>", region.SystemName).Replace("<bucket_name>", BUCKET_NAME).Replace("<bucket_guid>", genGuid)
                                        .Replace("<secret_access_key>", secretAccessKey)
                                        .Replace("<access_key_id>", accessKeyId);
                                sr.WriteLine(S3_CONTROLLER_KEY + "\n");
                                sr.WriteLine(controller_text_content + "\n");

                                PutObjectRequest requestController = new PutObjectRequest();
                                requestController.BucketName = BUCKET_NAME;
                                requestController.Key = S3_CONTROLLER_KEY;
                                requestController.ContentBody = controller_text_content;
                                PutObjectResponse xx = await s3Client.PutObjectAsync(requestController);

                                sr.WriteLine("Response Code " + xx.HttpStatusCode + "### Content Length" + xx.ContentLength + "### Etag" + xx.ETag);
                                if (("" + xx.HttpStatusCode).Equals("OK"))
                                {
                                    String S3_WORKER_KEY = "" + genGuid + "/" + "worker.sh";
                                    string worker_text_content = Constants.worker_text_content.Replace("<GUID>", genGuid).Replace("<region_name>", region.SystemName).Replace("<bucket_name>", BUCKET_NAME).Replace("<bucket_guid>",genGuid)
                                        .Replace("<secret_access_key>",secretAccessKey)
                                        .Replace("<access_key_id>", accessKeyId);
                                    sr.WriteLine(worker_text_content + "\n");
                                    sr.WriteLine(S3_WORKER_KEY + "\n");

                                    PutObjectRequest requestWorker = new PutObjectRequest();
                                    requestWorker.BucketName = BUCKET_NAME;
                                    requestWorker.Key = S3_WORKER_KEY;
                                    requestWorker.ContentBody = worker_text_content;
                                    PutObjectResponse xxx = await s3Client.PutObjectAsync(requestWorker);

                                    sr.WriteLine("Response Code " + xxx.HttpStatusCode + "### Content Length" + xxx.ContentLength + "### Etag" + xxx.ETag);
                                    if (("" + xxx.HttpStatusCode).Equals("OK"))
                                    {
                                        sr.WriteLine("CREATE EC2, LAUNCH WITH USER-DATA");
                                        createEc2(genGuid, sr);
                                        //string res = await createEc2V;
                                        //sr.WriteLine("\n" + res);
                                        // TransferUtility fileTransferUtility = new TransferUtility(accessKeyId, secretAccessKey, region);
                                        // fileTransferUtility.Upload(file.Path, BUCKET_NAME + "/"+genGuid + "/");

                                        //  PutObjectResponse xx = await s3Client.PutObjectAsync(requestImage);

                                        //   sr.WriteLine("Response Code " + x.HttpStatusCode + "### Content Length" + x.ContentLength + "### Etag" + x.ETag);

                                    }
                                }
                                   
                             }
                        }
                        else
                        {
                            sr.WriteLine("File is null");
                        }
                    }
                }
                catch (AmazonS3Exception ex)
                {
                    if (ex.ErrorCode != null && (ex.ErrorCode.Equals("InvalidAccessKeyId") ||
                        ex.ErrorCode.Equals("InvalidSecurity")))
                    {
                        sr.WriteLine("Please check the provided AWS Credentials.");
                        sr.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
                    }
                    else
                    {
                        sr.WriteLine("Caught Exception: " + ex.Message);
                        sr.WriteLine("Response Status Code: " + ex.StatusCode);
                        sr.WriteLine("Error Code: " + ex.ErrorCode);
                        sr.WriteLine("Request ID: " + ex.RequestId);
                    }
                }
            }

            return sb.ToString();
        }

        public static String generateGUID()
        {
            return Guid.NewGuid().ToString();
        }


        // This script will show how to use the AWS Tools for PowerShell to create a bucket and write to it.
        // It will be set as the user data in the EC2 Instance created which will run once the EC2 instance is 
        // fully launched.
        const string USER_DATA_SCRIPT_OLD =
            "<powershell>\n" +
            "Import-Module \"C:\\Program Files (x86)\\AWS Tools\\PowerShell\\AWSPowerShell\\AWSPowerShell.psd1\"\n" +
            "Set-DefaultAWSRegion {0}\n" +
            "New-Item c:\\Data -type directory\n" +
            "Add-Content -path c:\\Data\\results.txt -value \"Results from lots of data processing\"\n" +
            "New-S3Bucket -BucketName {1}\n" +
            "Write-S3Object -BucketName {1} -File c:\\Data\\results.txt -Key results.txt\n" +
            "shutdown.exe /s\n" +
            "</powershell>";

       

        static readonly string RESOURCDE_POSTFIX = DateTime.Now.Ticks.ToString();

       

        public static async void createEc2(String generatedUID, StringWriter sr)
        {
            var bucketName = "magicec2" + RESOURCDE_POSTFIX;
            region = RegionEndpoint.USWest2;
            
            StringBuilder sb = new StringBuilder(1024);
            using (sr = new StringWriter(sb))
            {
                sr.WriteLine("===========================================");
                sr.WriteLine("Welcome to the MAGIC CREATE EC2!");
                sr.WriteLine("===========================================");
                string encodedString="";
                //try {
                    string userdata = Constants.user_data_string.Replace("<bucket_name>", BUCKET_NAME).Replace("<bucket_guid>", generatedUID)
                            ;
                    sr.WriteLine("\n" + userdata);
                    string formattedString = Constants.ud_file_text_content;
                    sr.WriteLine("\n" + formattedString);
                    encodedString = EncodeToBase64(formattedString);
                    sr.WriteLine("\n" + encodedString);
               // }
               // catch (Exception e)
              //  {
              //      Debug.WriteLine(e.Message);
              //      sr.WriteLine(e.Message);
              //  }
      

                var ec2Client = new AmazonEC2Client(accessKeyId, secretAccessKey, region);
                try
                {
                    // Get latest 2012 Base AMI
                    //  var imageId = ImageUtilities.FindImageAsync(ec2Client, ImageUtilities.U).Result.ImageId;
                  //  sr.WriteLine("Using Image ID: {0}", imageId);
                    var imageId = "ami-6e1a0117";

                    // Create an IAM role with a profile that the Instance will use to run commands against AWS
                    var instanceProfileArn = CreateInstanceProfile();
                    sr.WriteLine("Created Instance Profile: {0}", instanceProfileArn);

                    // Sleep for a little to make sure the profile is fully propagated.
                    // Thread.Sleep(15000);
                    await Task.Delay(TimeSpan.FromMilliseconds(15000));

                    // Create key pair which will be used to demonstrate how get the Windows Administrator password.
                  //  var keyPair = ec2Client.CreateKeyPairAsync(new CreateKeyPairRequest { KeyName = "magicec2" + RESOURCDE_POSTFIX }).Result.KeyPair;
                  

                    var runRequest = new RunInstancesRequest
                    {
                        ImageId = imageId,
                        MinCount = 1,
                        MaxCount = 1,
                        KeyName = "magicec2636452255392115319",
                       // KeyName = "shahan_test",
                        IamInstanceProfile = new IamInstanceProfileSpecification { Arn = instanceProfileArn },
                        InstanceType = InstanceType.T2Micro,
       
                        
                    // Add the region for the S3 bucket and the name of the bucket to create
                    UserData = encodedString
                    };
                    var instanceId = ec2Client.RunInstancesAsync(runRequest).Result.Reservation.Instances[0].InstanceId;
                    sr.WriteLine("Launch Instance {0}", instanceId);


                    // Create the name tag
                    await ec2Client.CreateTagsAsync(new CreateTagsRequest
                    {
                        Resources = new List<string> { instanceId },
                        Tags = new List<Amazon.EC2.Model.Tag> { new Amazon.EC2.Model.Tag { Key = "Name", Value = "Processor" } }
                    });
                    sr.WriteLine("Adding Name Tag to instance");


                    sr.WriteLine("Waiting for EC2 Instance to stop");
                    // The script put in the user data will shutdown the instance when it is complete.  Wait
                    // till the instance has stopped which signals the script is done so the instance can be terminated.
                    Instance instance = null;
                    var instanceDescribeRequest = new DescribeInstancesRequest { InstanceIds = new List<string> { instanceId } };
                    do
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(10000));
                        instance = ec2Client.DescribeInstancesAsync(instanceDescribeRequest).Result.Reservations[0].Instances[0];

                        if (instance.State.Name == "stopped")
                        {
                            // Demonstrate how to get the Administrator password using the keypair.
                            var passwordResponse = ec2Client.GetPasswordDataAsync(new GetPasswordDataRequest
                            {
                                InstanceId = instanceId
                            });

                            // Make sure we actually got a password
                            if (passwordResponse.Result.PasswordData != null)
                            {
                               // var password = passwordResponse.Result.GetDecryptedPassword(keyPair.KeyMaterial);
                             //   sr.WriteLine("The Windows Administrator password is: {0}", password);
                            }
                        }
                    } while (instance.State.Name == "pending" || instance.State.Name == "running");

                    /*
                    // Terminate instance
                    await ec2Client.TerminateInstancesAsync(new TerminateInstancesRequest
                    {
                        InstanceIds = new List<string>() { instanceId }
                    });
                    */
                    // Delete key pair created.
                   // await ec2Client.DeleteKeyPairAsync(new DeleteKeyPairRequest { KeyName = keyPair.KeyName });

                   // var s3Client = new AmazonS3Client();
                    var listResponse = s3Client.ListObjectsAsync(new ListObjectsRequest
                    {
                        BucketName = BUCKET_NAME
                    });
                    //AmazonS3Exception: The specified bucket does not exist
                    Debug.WriteLine(listResponse.Result.ToString());
                    if (listResponse.Result.S3Objects.Count > 0)
                    {
                        sr.WriteLine("Found results file {0} in S3 bucket {1}", listResponse.Result.S3Objects[0].Key, bucketName);
                    }

                    // Delete bucket created for sample.
                    await AmazonS3Util.DeleteS3BucketWithObjectsAsync(s3Client, BUCKET_NAME);
                    sr.WriteLine("Deleted S3 bucket created for sample.");

                    DeleteInstanceProfile();
                    sr.WriteLine("Delete Instance Profile created for sample.");

                    sr.WriteLine("Instance terminated, EXIT");
                }
                catch (AmazonEC2Exception ex)
                {
                    if (ex.ErrorCode != null && ex.ErrorCode.Equals("AuthFailure"))
                    {
                        sr.WriteLine("The account you are using is not signed up for Amazon EC2.");
                        sr.WriteLine("You can sign up for Amazon EC2 at http://aws.amazon.com/ec2");
                    }
                    else
                    {
                        sr.WriteLine("Caught Exception: " + ex.Message);
                        sr.WriteLine("Response Status Code: " + ex.StatusCode);
                        sr.WriteLine("Error Code: " + ex.ErrorCode);
                        sr.WriteLine("Error Type: " + ex.ErrorType);
                        sr.WriteLine("Request ID: " + ex.RequestId);
                    }
                }
            }
         //   return sb.ToString();
        }

        /// <summary>
        /// Create the instance profile that will give permission for the EC2 instance to make request to Amazon S3.
        /// </summary>
        /// <returns></returns>
        static string CreateInstanceProfile()
        {
            var roleName = "magicec2" + RESOURCDE_POSTFIX;
           // AmazonIdentityManagementServiceClient
            var client = new AmazonIdentityManagementServiceClient(accessKeyId,secretAccessKey,region);
            client.CreateRoleAsync(new CreateRoleRequest
            {
                RoleName = roleName,
                AssumeRolePolicyDocument = @"{""Statement"":[{""Principal"":{""Service"":[""ec2.amazonaws.com""]},""Effect"":""Allow"",""Action"":[""sts:AssumeRole""]}]}"
            });

            var statement = new Amazon.Auth.AccessControlPolicy.Statement(Amazon.Auth.AccessControlPolicy.Statement.StatementEffect.Allow);
            statement.Actions.Add(S3ActionIdentifiers.AllS3Actions);
            statement.Resources.Add(new Resource("*"));

            var policy = new Policy();
            policy.Statements.Add(statement);

            client.PutRolePolicyAsync(new PutRolePolicyRequest
            {
                RoleName = roleName,
                PolicyName = "S3Access",
                PolicyDocument = policy.ToJson()
            });

            var response = client.CreateInstanceProfileAsync(new CreateInstanceProfileRequest
            {
                InstanceProfileName = roleName
            });

            client.AddRoleToInstanceProfileAsync(new AddRoleToInstanceProfileRequest
            {
                InstanceProfileName = roleName,
                RoleName = roleName
            });

            return response.Result.InstanceProfile.Arn;
        }

        /// <summary>
        /// Delete the instance profile created for the sample.
        /// </summary>
        static void DeleteInstanceProfile()
        {
            var roleName = "magicec2" + RESOURCDE_POSTFIX;
            // AmazonIdentityManagementServiceClient
            var client = new AmazonIdentityManagementServiceClient(accessKeyId, secretAccessKey, region);

            client.DeleteRolePolicyAsync(new DeleteRolePolicyRequest
            {
                RoleName = roleName,
                PolicyName = "S3Access"
            });

            client.RemoveRoleFromInstanceProfileAsync(new RemoveRoleFromInstanceProfileRequest
            {
                InstanceProfileName = roleName,
                RoleName = roleName
            });

            client.DeleteRoleAsync(new DeleteRoleRequest
            {
                RoleName = roleName
            });

            client.DeleteInstanceProfileAsync(new DeleteInstanceProfileRequest
            {
                InstanceProfileName = roleName
            });
        }

        static string EncodeToBase64(string str)
        {
            byte[] toEncodeAsBytes = System.Text.Encoding.UTF8.GetBytes(str);
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }
    }
}
