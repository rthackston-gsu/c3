using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App3.Utility
{
    class Constants
    {
        public static string weDoNotUse = "TestString";
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

        
        public static string ud_file_text_content = "#!/bin/bash \n touch /tmp/test.txt";

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


    }
}
