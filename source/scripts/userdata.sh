#!/bin/bash

cd ~
aws s3 cp s3://magic-beta/test/controller.sh .
chmod +x controller.sh
./controller.sh
