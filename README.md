[![Build Status](https://barradas.visualstudio.com/Contributions/_apis/build/status/Queue%20Move%20Worker?branchName=master)](https://barradas.visualstudio.com/Contributions/_build/latest?definitionId=5&branchName=master)

# QueueMove
 
A simple worker to move Rabbit MQ messages between queues;

## Running with Docker

```
docker run --name queue-move -d \
    -e OriginQueueConnectionString="amqp://user:password@localhost:5672/current-vh" \
    -e OriginQueueName=my-demo-queue \
    -e DestinationQueueConnectionString="amqp://user:password@localhost:5672/other-vh" \
	-e DestinationExchangeName=my-other-exchange \
    -e DestinationQueueName=my-other-queue \
	-e StopWhenEmpty=false \
    thiagobarradas/queue-move:latest
```

## Configuration

Set environment variables to setup QueueMove:

| Env Var | Type | Required | Description | e.g. |
| ------- | ---- | -------- | ----------- | ---- |
| `OriginQueueConnectionString` | string | yes | origin rabbit connection | `amqp://user:pass@localhost:5672/current-vh` |
| `OriginQueueName`             | string | yes | origin queue name | `some-queue` |
| `DestinationQueueConnectionString` | string | yes | destination rabbit connection | `amqp://user:pass@localhost:5672/other-vh` |
| `DestinationExchangeName`             | string | no | destination exchange name | `other-exchange` |
| `DestinationQueueName` | string | no | destination queue name | `other-queue` |
| `StopWhenEmpty`             | bool | no | when origin queue is empty (0 messages) this worker is stopped | `false` |

## How can I contribute?

Please, refer to [CONTRIBUTING](.github/CONTRIBUTING.md)

## Found something strange or need a new feature?

Open a new Issue following our issue template [ISSUE TEMPLATE](.github/ISSUE_TEMPLATE.md)

## Did you like it? Please, make a donate :)

if you liked this project, please make a contribution and help to keep this and other initiatives, send me some Satochis.

BTC Wallet: `1G535x1rYdMo9CNdTGK3eG6XJddBHdaqfX`

![1G535x1rYdMo9CNdTGK3eG6XJddBHdaqfX](https://i.imgur.com/mN7ueoE.png)
