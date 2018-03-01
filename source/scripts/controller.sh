#!/bin/bash

aws configure set aws_secret_access_key <secret_access_key>
aws configure set region <region_name>
aws configure set output json
aws configure set aws_access_key_id <access_key_id>

(aws s3 cp s3://$BUCKET/$GUID/magic.conf /home/ec2-user/magic.conf)
(aws s3 cp s3://$BUCKET/$GUID/worker.sh /home/ec2-user/worker.sh)

# Download and run Rabbitmq_server script to install and launch RabbitMQ

# Download and run XYZ script to populate queue with jobs/messages
message=("A" "B" "C")
for i in ${message[@]}
do
python send.py $i
done
# TODO: Read config values from magic.conf
source /home/ec2-user/magic.conf
(aws ec2 run-instances --image-id $MAGIC_IMAGE_ID --count $MAGIC_COUNT --instance-type $MAGIC_INSTANCE_TYPE --key-name $MAGIC_KEY_NAME --security-group-ids $MAGIC_SGI --user-data file://worker.sh)
