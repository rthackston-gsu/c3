#!/bin/bash
GUID=<GUID>
BUCKET=<BUCKET>
echo '$GUID'

apt install awscli -y

aws configure set aws_secret_access_key <secret_access_key>
aws configure set region <region_name>
aws configure set output json
aws configure set aws_access_key_id <access_key_id>

(aws s3 cp s3://$GUID/task.sh task.sh)
(bash task.sh)

aws s3 cp result.txt s3://$BUCKET/$GUID/
(shutdown -h now)
