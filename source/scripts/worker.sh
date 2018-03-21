#!/bin/bash
GUID=<GUID>
BUCKET=<BUCKET>

apt install awscli -y

aws s3 cp s3://$BUCKET/$GUID/task.sh /home/ec2-user/
aws s3 cp s3://$BUCKET/$GUID/rec.py /home/ec2-user/

# Download and run Rabbitmq_server script to install and launch RabbitMQ
(aws s3 cp s3://$BUCKET/$GUID/rabbitmq.sh /home/ec2-user/)
bash /home/ec2-user/rabbitmq.sh

# Start while loop
count=1
counter=${#message[@]}
while [ $count -le $counter ]
do
MSG=$(python rec.py)
bash /home/ec2-user/task.sh $MSG
aws s3 mv result$MSG.txt s3://$BUCKET/$GUID/$Instance_ID/
count=`expr $count + 1`
done

(shutdown -h now)
