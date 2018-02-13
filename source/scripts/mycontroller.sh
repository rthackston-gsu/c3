#cloud-boothook
#!/bin/bash
curl "https://bootstrap.pypa.io/get-pip.py" -o "get-pip.py"
python get-pip.py --user
pip install awscli --user

#Configuring AWS
aws configure set aws_access_key_id <access_key_id>
aws configure set aws_secret_access_key <secret_access_key>
aws configure set region <region_name>
aws configure set output json

aws s3 cp s3://$BUCKET/$GUID/worker.sh /home/ec2-user/

# TODO: Launching the worker script
aws ec2 run-instances --image-id ami-f63b1193 --count 1 --instance-type t2.micro --key-name magicuwp --security-group-ids sg-a58bbdcd --user-data file://worker.sh
