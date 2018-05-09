#!/bin/bash

d="$(echo "$1" | base64 -d)"
Date=$(date '+%Y:%m:%d_%H:%M:%S')
echo "This is task $d" > result_$Date.txt
echo "Job $d completed"> job$d.txt
