#!/bin/bash

curl -o taskFile.sh https://s3-us-west-2.amazonaws.com/<bucket_guid>/task.sh
sh taskFile.sh
apt install awscli -y
aws configure set aws_access_key_id <access_key_id>
aws configure set aws_secret_access_key <secret_access_key>
aws configure set region <region_name>
aws configure set output json
echo "aws ec2 run-instances --image-id ami-6e1a0117 --count 1 --instance-type t2.micro \
--key-name <private_key.pem> --security-group-ids launch-wizard-2 \
--user-data file://taskFile.sh"