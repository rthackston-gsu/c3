#!/bin/bash

# Download and run XYZ script to populate queue with jobs/messages
message=("A" "B" "C")
for i in ${message[@]}
do
e="$(echo "$i" | base64)"
python send.py $e
done

