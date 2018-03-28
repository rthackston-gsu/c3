#!/bin/bash

# copying the script to the ec2 home directory
aws s3 cp s3://$BUCKET/$GUID/magic.conf .
aws s3 cp s3://$BUCKET/$GUID/worker.sh .
aws s3 cp s3://$BUCKET/$GUID/send.py .

# Download and run Rabbitmq_server script to install and launch RabbitMQ
aws s3 cp s3://$BUCKET/$GUID/rabbitmq.sh .
chmod +x rabbitmq.sh
./rabbitmq.sh

# Download and run XYZ script to populate queue with jobs/messages
message=("A" "B" "C")
for i in ${message[@]}
do
python send.py $i
done

# TODO: Read config values from magic.conf
source magic.conf
aws ec2 run-instances --image-id $MAGIC_IMAGE_ID --iam-instance-profile Name="$MAGIC_IAM_USER" --count $MAGIC_COUNT --instance-type $MAGIC_INSTANCE_TYPE --key-name $MAGIC_KEY_NAME --security-group-ids $MAGIC_SGI --user-data file://worker.sh

