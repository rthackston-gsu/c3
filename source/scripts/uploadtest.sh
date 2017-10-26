#!bin/bash

resul="Hello World"
echo `${result} > result.txt`
file="result.txt"
bucket="magic-bucket-one"
resource="/${bucket}/${file}"
contentType="text/plain"
date=`date +%Y%m%d`
dateValue=`date -R`
stringToSign="PUT\n\n${contentType}\n${dateValue}\n${resource}"
s3Secret="KXM9bVfCQOSVig6M0rCNOP8bOiUSSQ0DKt7b65dQ"
s3Key="AKIAJ5OIR6BNGMHGICHA"
signature=`echo -en ${stringToSign} | openssl sha1 -hmac ${s3Secret} -binary | base64`
curl -X PUT -T "${file}" \
  -H "Host: ${bucket}.s3.amazonaws.com" \
  -H "Date: ${dateValue}" \
  -H "Content-Type: ${contentType}" \
  -H "Authorization: AWS ${s3Key}:${signature}" \
  https://${bucket}.s3.amazonaws.com/${file}
