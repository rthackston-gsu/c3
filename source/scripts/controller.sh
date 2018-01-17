#!/bin/bash

aws configure set aws_secret_access_key <secret_access_key>
aws configure set region <region_name>
aws configure set output json
aws configure set aws_access_key_id <access_key_id>

(aws s3 cp s3://$BUCKET/$GUID/magic.conf magic.conf)
(aws s3 cp s3://$BUCKET/$GUID/worker.sh worker.sh)

# TODO: Read config values from magic.conf
(aws ec2 run-instances --image-id ami-6e1a0117 --count 1 --instance-type t2.micro --security-group-ids launch-wizard-2 --user-data file://worker.sh)
