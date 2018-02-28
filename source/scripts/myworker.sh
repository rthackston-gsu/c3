#cloud-boothook
#!/bin/bash
curl "https://bootstrap.pypa.io/get-pip.py" -o "get-pip.py"
python get-pip.py --user
pip install awscli --user

aws configure set aws_access_key_id <access_key_id>
aws configure set aws_secret_access_key <secret_access_key>
aws configure set region <region_name>
aws configure set output json

GUID=<GUID>
BUCKET=<BUCKET>
Instance_ID=$(curl http://169.254.169.254/latest/meta-data/instance-id)
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
shutdown -h now
