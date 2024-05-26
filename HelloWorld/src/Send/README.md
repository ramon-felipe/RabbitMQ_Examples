Running via docker:

https://hub.docker.com/_/rabbitmq/

It is possible to just use the rabbit-management image

docker run -d --hostname my-rabbit --name some-rabbit-management -p5672:5672 -p15672:15672 rabbitmq:3-management

It will make available a UI on http://localhost:15672/