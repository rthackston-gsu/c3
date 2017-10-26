#!/bin/bash

curl -o taskFile.sh https://s3-us-west-2.amazonaws.com/magic-bucket-one/task.sh
sh taskFile.sh
apt install awscli -y
echo "aws ec2 run-instances --image-id ami-6e1a0117 --count 1 --instance-type t2.micro \
--key-name shahan_test --subnet-id subnet-f41f8e92 --security-group-ids launch-wizard-2 \
--user-data file://taskFile.sh"
