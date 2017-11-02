#!/bin/bash
GUID=<GUID>
echo '$GUID'

YUM_CMD=$(which yum)
APT_CMD=$(which apt)
APT_GET_CMD=$(which apt-get)

get_package() {
  PACKAGE = $1
  if [[ ! -z $YUM_CMD ]]; then
    yum install $PACKAGE -y
  elif [[ ! -z $APT_CMD ]]; then
    apt install $PACKAGE -y
  elif [[ ! -z $APT_GET_CMD ]]; then
    apt-get install $PACKAGE -y
  else
    echo "Could not install $PACKAGE due to incompatible package manager"
    exit 1;
  fi
}

get_package awscli
(aws s3 cp s3://$GUID/controller.sh controller.sh)
(bash controller.sh)
