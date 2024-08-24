using MassTransit;

namespace RabbitRateLimiter.Consumer
{
    public class MessageProcessor : IConsumer<Message>
    {
        public async Task Consume(ConsumeContext<Message> ctx)
        {
            Console.WriteLine($"[{DateTime.Now}] New message: {ctx.Message.Content}");
        }
    }

    public class Message
    {
        public string Content { get; set; }
    }
}
