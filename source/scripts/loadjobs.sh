#!/bin/bash

# Download and run XYZ script to populate queue with jobs/messages
message=("A" "B" "C")
Number_Jobs=${#message[@]}>Total_Job.txt
for i in ${message[@]}
do
e="$(echo "$i" | base64)"
python send.py $e
done

