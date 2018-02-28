#!/bin/bash

aws configure set aws_secret_access_key <secret_access_key>
aws configure set region <region_name>
aws configure set output json
aws configure set aws_access_key_id <access_key_id>

(aws s3 cp s3://$BUCKET/$GUID/magic.conf magic.conf)
(aws s3 cp s3://$BUCKET/$GUID/worker.sh worker.sh)

# Download and run Rabbitmq_server script to install and launch RabbitMQ

# Download and run XYZ script to populate queue with jobs/messages
message=("A" "B" "C")
for i in ${message[@]}
do
python send.py $i
done
# TODO: Read config values from magic.conf
(aws ec2 run-instances --image-id ami-f63b1193 --count 1 --instance-type t2.micro --key-name magicuwp --security-group-ids sg-a58bbdcd --user-data file://worker.sh)
