#!/bin/bash

# Download and run XYZ script to populate queue with jobs/messages
message=("A" "B" "C")
for i in ${message[@]}
do
python send.py $i
done

