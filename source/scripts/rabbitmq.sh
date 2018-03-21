#!/bin/bash

sudo pip install pika
sudo yum -y update

# Add and enable relevant application repositories:
# Note: We are also enabling third party remi package repositories.
wget http://dl.fedoraproject.org/pub/epel/6/x86_64/epel-release-6-8.noarch.rpm
wget http://rpms.famillecollet.com/enterprise/remi-release-6.rpm
sudo rpm -Uvh remi-release-6*.rpm epel-release-6*.rpm

# Finally, download and install Erlang:
sudo yum install -y erlang

# Download the latest RabbitMQ package using wget:
wget http://www.rabbitmq.com/releases/rabbitmq-server/v3.6.15/rabbitmq-server-3.6.15-1.el6.noarch.rpm

# Add the necessary keys for verification:
sudo rpm --import http://www.rabbitmq.com/rabbitmq-signing-key-public.asc

# Install the .RPM package using YUM:
sudo yum install -y rabbitmq-server-3.6.15-1.el6.noarch.rpm

# To have RabbitMQ start as a daemon by default
sudo chkconfig rabbitmq-server on

# To start the service:
sudo service rabbitmq-server start

#Enabling rabbitmq-management 
sudo rabbitmq-plugins enable rabbitmq_management

#Restarting the rabbitmq-server
sudo service rabbitmq-server restart
