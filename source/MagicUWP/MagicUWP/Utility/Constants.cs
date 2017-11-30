using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*
    Constants:
    Code contains contants for the worker, config, task, controller and user-data scripts
 * By Beloved Egbedion
 * For Dr. Russell Thackston
 * MAGIC Project
    10/28/2017
*/
namespace MagicUWP
{
    class Constants
    {
        public static string weDoNotUse = "TestString";
        public static string S3_BUCKET_NAME = "magicbucket12345678";
        public static string worker_text_content = "#!/bin/bash \n \n" + "BUCKET=<bucket_name> \n" + "GUID=<bucket_guid> \n" +
            " echo '$GUID' \n" +
            "\apt install awscli -y \n"
            +
            "aws configure set aws_access_key_id <access_key_id>\n" +
            "aws configure set aws_secret_access_key <secret_access_key>\n" +
            "aws configure set region <region_name>\n" +
            "aws configure set output json \n" +
            "(aws s3 cp s3://$BUCKET/$GUID/task.sh task.sh) \n" +
            "(bash task.sh)\n"
            +
            "(aws s3 cp result.txt s3://$BUCKET/$GUID/result.txt)\n"
            +
            "(shutdown -h now)\n";

        
        public static string ud_file_text_content = "#!/bin/bash \n echo 'bar' > /tmp/bar \n echo 'foo' > /tmp/foo";
 
        
        
        public static string task_file_text_content = "#!/bin/bash \necho 'hello world' > result.txt";

        public static string controller_text_content = "#!/bin/bash \n \n" + "BUCKET=<bucket_name> \n" + "GUID=<bucket_guid> \n" +
            " echo '$GUID' \n" +
            "aws configure set aws_access_key_id <access_key_id>\n" +
            "aws configure set aws_secret_access_key <secret_access_key>\n" +
            "aws configure set region <region_name>\n" +
            "aws configure set output json \n" +
            "(aws s3 cp s3://$BUCKET/$GUID/worker.sh worker.sh) \n"+
            "(aws ec2 run-instances --image-id ami-6e1a0117 --count 1 --instance-type t2.micro --user-data file://worker.sh) ";

        public static string config_text_content = "#!bin/bash \n <GUID>";

        public static string user_data_string = "#!/bin/bash \n \n" +"BUCKET=<bucket_name> \n" + "GUID=<bucket_guid> \n" +
            " echo '$GUID' \n" +
            " YUM_CMD=$(which yum) \n" +
            " APT_CMD =$(which apt) \n" +
            " APT_GET_CMD =$(which apt-get) \n " +
            " get_package() \n " +
            "{ \n PACKAGE = $1 \n" +
            " if [[!-z $YUM_CMD]]; then \n" +
            " yum install $PACKAGE - y \n" +
            " elif[[!-z $APT_CMD]]; then \n" +
            " apt install $PACKAGE - y \n" +
            " elif[[!-z $APT_GET_CMD]]; then \n" +
            " apt - get install $PACKAGE - y \n" +
            " else \n" +
            " echo \"Could not install $PACKAGE due to incompatible package manager\" \n" +
            " exit 1; \n" +
            " fi \n" +
            " } \n" +
            " get_package awscli \n" +
            " (aws s3 cp s3://$BUCKET/$GUID/controller.sh controller.sh) \n \n" +
            "(bash controller.sh)";

        public static string USER_DATA_SCRIPT_OLD =
    "<powershell>\n" +
    "Import-Module \"C:\\Program Files (x86)\\AWS Tools\\PowerShell\\AWSPowerShell\\AWSPowerShell.psd1\"\n" +
    "Set-DefaultAWSRegion {0}\n" +
    "New-Item c:\\Data -type directory\n" +
    "Add-Content -path c:\\Data\\results.txt -value \"Results from lots of data processing\"\n" +
    "New-S3Bucket -BucketName {1}\n" +
    "Write-S3Object -BucketName {1} -File c:\\Data\\results.txt -Key results.txt\n" +
    "shutdown.exe /s\n" +
    "</powershell>";

    }
}
