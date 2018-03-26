#!/bin/bash

sudo pip-2.7 install pika
# yum -y update

# Add and enable relevant application repositories:
# Note: We are also enabling third party remi package repositories.
wget http://dl.fedoraproject.org/pub/epel/6/x86_64/epel-release-6-8.noarch.rpm
wget http://rpms.famillecollet.com/enterprise/remi-release-6.rpm
rpm -Uvh remi-release-6*.rpm epel-release-6*.rpm

# Finally, download and install Erlang:
yum install -y erlang

# Download the latest RabbitMQ package using wget:
wget http://www.rabbitmq.com/releases/rabbitmq-server/v3.2.2/rabbitmq-server-3.2.2-1.noarch.rpm

# Add the necessary keys for verification:
rpm --import http://www.rabbitmq.com/rabbitmq-signing-key-public.asc

# Install the .RPM package using YUM:
yum install -y rabbitmq-server-3.2.2-1.noarch.rpm

# To have RabbitMQ start as a daemon by default
chkconfig rabbitmq-server on

# To start the service:
/sbin/service rabbitmq-server start

#Enabling rabbitmq-management 
rabbitmq-plugins enable rabbitmq_management

#Restarting the rabbitmq-server
/sbin/service rabbitmq-server restart
