using System;
using System.Collections.Generic;
using Amazon.Auth.AccessControlPolicy;
using Amazon.Auth.AccessControlPolicy.ActionIdentifiers;
using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.IdentityManagement;
using Amazon.IdentityManagement.Model;
using Statement = Amazon.Auth.AccessControlPolicy.Statement;

namespace magic.gsu.edu
{
    public static class CreateEc2
    {
        private static readonly string ResourcdePostfix = DateTime.Now.Ticks.ToString();
        static readonly AmazonEC2Client Ec2Client = new AmazonEC2Client();
        public static void CreateInstance()
        {
            const string amiId = "ami-e189c8d1";
            const string keyPairName = "my_sample_key";

            var instanceProfileArn = CreateInstanceProfile();
            Console.WriteLine("Created Instance Profile: {0}", instanceProfileArn);

            var keyPair = Ec2Client.CreateKeyPair(new CreateKeyPairRequest
            {
                KeyName = keyPairName + ResourcdePostfix
            }).KeyPair;

            /*
            List<string> groups = new List<string>()
            {
                CreateSecurityGroup().GroupId
            };
            */

            var launchRequest = new RunInstancesRequest()
            {
                ImageId = amiId,
                InstanceType = InstanceType.T1Micro,
                MinCount = 1,
                MaxCount = 1,
                KeyName = keyPair.KeyName,
                IamInstanceProfile = new IamInstanceProfileSpecification
                {
                    Arn = instanceProfileArn
                }
                // SecurityGroupIds = groups
            };

            var launchResponse = Ec2Client.RunInstances(launchRequest);
            var instances = launchResponse.Reservation.Instances;
            var instanceIds = new List<string>();
            foreach (var item in instances)
            {
                instanceIds.Add(item.InstanceId);
                Console.WriteLine();
                Console.WriteLine("New instance: " + item.InstanceId);
                Console.WriteLine("Instance state: " + item.State.Name);
            }
            // var ImageID = ImageUtilities.FindImage(ec2Client, ImageUtilities.)
        }

        public static SecurityGroup CreateSecurityGroup()
        {
            const string secGroupName = "my_sample_sg_vpc";
            SecurityGroup mySg = null;
            var vpcId = "vpc-f1663d98";

            var vpcFilter = new Filter
            {
                Name = "vpc-id",
                Values = new List<string>()
                {
                    vpcId
                }
            };
            var dsgRequest = new DescribeSecurityGroupsRequest();
            dsgRequest.Filters.Add(vpcFilter);
            var dsgResponse = Ec2Client.DescribeSecurityGroups(dsgRequest);
            Console.WriteLine(dsgResponse.HttpStatusCode);
            var mySGs = dsgResponse.SecurityGroups;
            foreach (var item in mySGs)
            {
                Console.WriteLine("Existing security group: " + item.GroupId);
                if (item.GroupName == secGroupName)
                {
                    mySg = item;
                }
            }
            return mySg;
        }

        private static string CreateInstanceProfile()
        {
            var roleName = "ec2-sample-" + ResourcdePostfix;
            var client = new AmazonIdentityManagementServiceClient();
            client.CreateRole(new CreateRoleRequest
            {
                RoleName = roleName,
                AssumeRolePolicyDocument = @"{""Statement"":[{""Principal"":{""Service"":[""ec2.amazonaws.com""]},""Effect"":""Allow"",""Action"":[""sts:AssumeRole""]}]}"
            });

            var statement = new Statement(Statement.StatementEffect.Allow);
            statement.Actions.Add(S3ActionIdentifiers.AllS3Actions);
            statement.Resources.Add(new Resource("*"));

            var policy = new Policy();
            policy.Statements.Add(statement);

            client.PutRolePolicy(new PutRolePolicyRequest
            {
                RoleName = roleName,
                PolicyName = "S3Access",
                PolicyDocument = policy.ToJson()
            });

            var response = client.CreateInstanceProfile(new CreateInstanceProfileRequest
            {
                InstanceProfileName = roleName
            });

            client.AddRoleToInstanceProfile(new AddRoleToInstanceProfileRequest
            {
                InstanceProfileName = roleName,
                RoleName = roleName
            });

            return response.InstanceProfile.Arn;
        }
    }
}
