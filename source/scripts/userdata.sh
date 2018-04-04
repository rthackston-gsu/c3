#!/bin/bash
cd ~

BUCKET=magicbucket123456789
GUID=6ccbd3e0-d9b0-413a-a059-bcbcb4969644
aws s3 cp s3://$BUCKET/$GUID/magiccontroller.sh .
chmod +x magiccontroller.sh
./magiccontroller.sh
