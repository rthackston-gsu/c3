#!/bin/bash

d="$(echo "$1" | base64 -d)"
echo "This is task $d" > result$d.txt
