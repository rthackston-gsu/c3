#!/bin/bash

aws configure set aws_access_key_id <access_key_id>
aws configure set aws_secret_access_key <secret_access_key>
aws configure set region <region_name>
aws configure set output json 

# copying the script to the ec2 home directory
aws s3 cp s3://$BUCKET/$GUID/magic.conf /home/ec2-user/
aws s3 cp s3://$BUCKET/$GUID/worker.sh /home/ec2-user/
aws s3 cp s3://$BUCKET/$GUID/send.py /home/ec2-user/

# Download and run Rabbitmq_server script to install and launch RabbitMQ
aws s3 cp s3://$BUCKET/$GUID/rabbitmq.sh /home/ec2-user/
bash /home/ec2-user/rabbitmq.sh

# Download and run XYZ script to populate queue with jobs/messages
message=("A" "B" "C")
for i in ${message[@]}
do
python send.py $i
done

# TODO: Read config values from magic.conf
source /home/ec2-user/magic.conf
(aws ec2 run-instances --image-id $MAGIC_IMAGE_ID --iam-instance-profile Name="magic_S3_access_for_EC2" --count $MAGIC_COUNT --instance-type $MAGIC_INSTANCE_TYPE --key-name $MAGIC_KEY_NAME --security-group-ids $MAGIC_SGI --user-data file://worker.sh)

