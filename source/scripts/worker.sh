#!/bin/bash
GUID=<GUID>
BUCKET=<BUCKET>
echo '$GUID'

apt install awscli -y

aws configure set aws_secret_access_key <secret_access_key>
aws configure set region <region_name>
aws configure set output json
aws configure set aws_access_key_id <access_key_id>

aws s3 cp s3://$BUCKET/$GUID/task.sh /home/ec2-user/task.sh

# Start while loop
count=1
counter=${#message[@]}
while [ $count -le $counter ]
do
MSG=$(python rec.py)
bash /home/ec2-user/task.sh $MSG
aws s3 cp result.txt s3://$BUCKET/$GUID/
aws s3 mv result$MSG.txt s3://$BUCKET/$GUID/$Instance_ID/
count=`expr $count + 1`
done

(shutdown -h now)
