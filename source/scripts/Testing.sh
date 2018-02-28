#cloud-boothook
#!/bin/bash

Instance_ID=$(curl http://169.254.169.254/latest/meta-data/instance-id)
BUCKET=magicbucket123456789
GUID=6ccbd3e0-d9b0-413a-a059-bcbcb4969644

aws s3 cp s3://$BUCKET/$GUID/task.sh /home/ec2-user/

message=("{1:45}" "{2:55}" "{3:90}")
for i in ${message[@]}
do
python send.py $i
done

count=1
counter=${#message[@]}
while [ $count -le $counter ]
do
MSG=$(python rec.py)
bash /home/ec2-user/task.sh $MSG
aws s3 mv result$MSG.txt s3://$BUCKET/$GUID/$Instance_ID/
count=`expr $count + 1`
done
