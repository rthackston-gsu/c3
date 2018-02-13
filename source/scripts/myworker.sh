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
bash /home/ec2-user/task.sh > /home/ec2-user/result.txt
aws s3 mv /home/ec2-user/result.txt s3://$BUCKET/$GUID/$Instance_ID/
shutdown -h now
