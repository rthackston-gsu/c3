#!/bin/bash
cd ~
BUCKET=magicbucket123456789
GUID=6ccbd3e0-d9b0-413a-a059-bcbcb4969644
Instance_ID=$(curl http://169.254.169.254/latest/meta-data/instance-id)

aws s3 cp s3://$BUCKET/$GUID/controller_ip.txt .
aws s3 cp s3://$BUCKET/$GUID/task.sh .
chmod +x task.sh
aws s3 cp s3://$BUCKET/$GUID/rec.py .

# Download and run Rabbitmq_server script to install and launch RabbitMQ
aws s3 cp s3://$BUCKET/$GUID/rabbitmq.sh .
chmod +x rabbitmq.sh
./rabbitmq.sh

# Start while loop
a=$(cat controller_ip.txt)
MSG=$(python rec.py $a)
while [[ $MSG != None ]]
do
./task.sh $MSG
MSG=$(python rec.py $a)
done
aws s3 mv result*.txt s3://$BUCKET/$GUID/$Instance_ID/
shutdown -h now
