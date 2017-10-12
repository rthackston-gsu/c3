using System;
using System.Collections.Generic;

using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.EC2.Util;

using Amazon.IdentityManagement;
using Amazon.IdentityManagement.Model;

using Amazon.Auth.AccessControlPolicy;
using Amazon.Auth.AccessControlPolicy.ActionIdentifiers;
using Statement = Amazon.Auth.AccessControlPolicy.Statement;

using Amazon.Util;

namespace magic.gsu.edu
{
    class CreateEC2
    {
        static readonly string RESOURCDE_POSTFIX = DateTime.Now.Ticks.ToString();
        static AmazonEC2Client ec2Client = new AmazonEC2Client();
        public static void CreateInstance()
        {
            string amiID = "ami-e189c8d1";
            string keyPairName = "my_sample_key";

            List<string> groups = new List<string>()
            {
                CreateSecurityGroup().GroupId
            };

            var launchRequest = new RunInstancesRequest()
            {
                ImageId = amiID,
                InstanceType = InstanceType.T1Micro,
                MinCount = 1,
                MaxCount = 1,
                KeyName = keyPairName,
                SecurityGroupIds = groups
            };

            var launchResponse = ec2Client.RunInstances(launchRequest);
            var instances = launchResponse.Reservation.Instances;
            var instanceIds = new List<string>();
            foreach (Instance item in instances)
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
            string secGroupName = "my_sample_sg_vpc";
            SecurityGroup mySG = null;
            string vpcID = "vpc-f1663d98";

            Filter vpcFilter = new Filter
            {
                Name = "vpc-id",
                Values = new List<string>()
                {
                    vpcID
                }
            };
            var dsgRequest = new DescribeSecurityGroupsRequest();
            dsgRequest.Filters.Add(vpcFilter);
            var dsgResponse = ec2Client.DescribeSecurityGroups(dsgRequest);
            Console.WriteLine(dsgResponse.HttpStatusCode);
            List<SecurityGroup> mySGs = dsgResponse.SecurityGroups;
            foreach (SecurityGroup item in mySGs)
            {
                Console.WriteLine("Existing security group: " + item.GroupId);
                if (item.GroupName == secGroupName)
                {
                    mySG = item;
                }
            }
            return mySG;
        }

        static string CreateInstanceProfile()
        {
            var roleName = "ec2-sample-" + RESOURCDE_POSTFIX;
            var client = new AmazonIdentityManagementServiceClient();
            client.CreateRole(new CreateRoleRequest
            {
                RoleName = roleName,
                AssumeRolePolicyDocument = @"{""Statement"":[{""Principal"":{""Service"":[""ec2.amazonaws.com""]},""Effect"":""Allow"",""Action"":[""sts.AssumeRole""]}]}"
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
