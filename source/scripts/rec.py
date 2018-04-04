#!/usr/bin/env python
import pika
import sys

localhost = ' '.join(sys.argv[1:]) or 'localhost'
connection = pika.BlockingConnection(pika.ConnectionParameters(host=localhost))
channel = connection.channel()

channel.queue_declare(queue='hello')

method_frame, header_frame, body = channel.basic_get(queue = 'hello')
print(body)

channel.basic_ack(delivery_tag=method_frame.delivery_tag)