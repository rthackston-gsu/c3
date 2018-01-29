#cloud-boothook
#!/bin/bash
GUID=<GUID>
BUCKET=<BUCKET>
apt install awscli -y
aws configure set aws_secret_access_key <secret_access_key>
aws configure set region <region_name>
aws configure set output json
aws configure set aws_access_key_id <access_key_id>
Instance_ID=$(curl http://169.254.169.254/latest/meta-data/instance-id)
bash $(aws s3 ls s3://$BUCKET/$GUID/task.sh)> /home/ec2-user/result2.txt
aws s3 mv /home/ec2-user/result2.txt s3://$BUCKET/$GUID/$Instance_ID/
shutdown -h now
