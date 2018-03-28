#!/bin/bash
GUID=<GUID>
BUCKET=<BUCKET>

aws s3 cp s3://$BUCKET/$GUID/task.sh .
aws s3 cp s3://$BUCKET/$GUID/rec.py .

# Download and run Rabbitmq_server script to install and launch RabbitMQ
aws s3 cp s3://$BUCKET/$GUID/rabbitmq.sh .
./rabbitmq.sh

# Start while loop
while true
do
MSG=$(python rec.py)
bash task.sh $MSG
aws s3 mv result$MSG.txt s3://$BUCKET/$GUID/$Instance_ID/
if [[ $MSG == None ]]
then
break
fi
done
shutdown -h now
#count=1
#counter=${#message[@]}
#while [ $count -le $counter ]
#do
#MSG=$(python rec.py)
#bash /home/ec2-user/task.sh $MSG
#aws s3 mv result$MSG.txt s3://$BUCKET/$GUID/$Instance_ID/
#count=`expr $count + 1`
#done

