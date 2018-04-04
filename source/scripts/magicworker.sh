#!/bin/bash
cd ~
BUCKET=magicbucket123456789
GUID=6ccbd3e0-d9b0-413a-a059-bcbcb4969644
Instance_ID=$(curl http://169.254.169.254/latest/meta-data/instance-id)

aws s3 cp s3://$BUCKET/$GUID/controller_ip.txt .
chmod +x controller_ip.sh
aws s3 cp s3://$BUCKET/$GUID/task.sh .
chmod +x task.sh
aws s3 cp s3://$BUCKET/$GUID/rec.py .

# Download and run Rabbitmq_server script to install and launch RabbitMQ
aws s3 cp s3://$BUCKET/$GUID/rabbitmq.sh .
chmod +x rabbitmq.sh
./rabbitmq.sh

# Start while loop
while true
do
a=$(cat controller_ip.txt)
MSG=$(python rec.py $a)
./task.sh $MSG
aws s3 mv result$MSG.txt s3://$BUCKET/$GUID/$Instance_ID/
if [[ $MSG == None ]]
then
break
fi
done
