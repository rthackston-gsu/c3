#!/bin/bash

cd ~
aws s3 cp s3://magic-beta/test/magic-beta.sh .
chmod +x magic-beta.sh
./magic-beta.sh
