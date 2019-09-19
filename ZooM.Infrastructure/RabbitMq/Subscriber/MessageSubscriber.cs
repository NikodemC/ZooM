using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;
using ZooM.Infrastructure.Options;

namespace ZooM.Infrastructure.RabbitMq.Subscriber
{
    internal class MessageSubscriber : IMessageSubscriber
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConnection _connection;
        private readonly RabbitMqOptions _options;

        public MessageSubscriber(IConnection connection,
            RabbitMqOptions options, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _connection = connection;
            _options = options;
        }
        public void Subscribe<TMessage>(Func<IServiceProvider, TMessage, Task> onReceived) where TMessage : class
        {
            var channel = _connection.CreateModel();
            var queueName = typeof(TMessage).Name;

            channel.ExchangeDeclare(exchange: _options.Exchange,
                durable: true,
                autoDelete: false,
                arguments: null,
                type: "topic");

            channel.QueueDeclare(queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            channel.QueueBind(queueName, _options.Exchange, $"#.{queueName}.#");

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body;
                var json = Encoding.UTF8.GetString(body);
                var message = JsonConvert.DeserializeObject<TMessage>(json);
                await onReceived(_serviceProvider, message);
            };
            channel.BasicConsume(queue: queueName,
                autoAck: true,
                consumer: consumer);
        }
    }
}
