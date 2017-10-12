using System;
using System.Collections.Generic;

using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.EC2.Util;
using Amazon.Util;

namespace magic.gsu.edu
{
    class CreateEC2
    {
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
    }
}
