#!/bin/bash
GUID=<GUID>
BUCKET=<BUCKET>
PATH_1="C:\users\task\"

INSTANCE_ID=(curl http://169.254.169.254/latest/meta-data/)
echo '$GUID'
echo "$INSTANCE_ID"


apt install awscli -y

aws configure set aws_secret_access_key <secret_access_key>
aws configure set region <region_name>
aws configure set output json
aws configure set aws_access_key_id <access_key_id>

results(){
aws s3 cp "C:\users\*.txt" s3://$BUCKET/$GUID/$INSTANCE_ID
}

For files in $PATH_1
do
aws s3 cp $files s3://$BUCKET/$GUID/

done

$TASK_PATH="s3://$BUCKET/$GUID/"

For files in $TASK_PATH
do
echo "Performing Tasks"
bash "$TASK_PATH"
echo "Tasks Completed"
done
results()
(shutdown -h now)
