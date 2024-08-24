using MassTransit;

namespace RabbitRateLimiter.Middlewares
{
    public class RateLimitMiddleware : IFilter<ConsumeContext>
    {
        private readonly int _limit;
        private readonly TimeSpan _timeSpan;
        private int _requestCount = 0;
        private DateTime _resetTime;

        public RateLimitMiddleware(int limit, TimeSpan timeSpan)
        {
            _limit = limit;
            _timeSpan = timeSpan;
            _resetTime = DateTime.UtcNow.Add(timeSpan);
        }

        public async Task Send(ConsumeContext ctx, IPipe<ConsumeContext> next)
        {
            if (DateTime.UtcNow > _resetTime)
            {
                _requestCount = 0;
                _resetTime = DateTime.UtcNow.Add(_timeSpan);
            }

            if (_requestCount < _limit)
            {
                _requestCount++;
                await next.Send(ctx);
            }
            else
            {
                Console.WriteLine($"Rate limit exceeded");
            }
        }

        public void Probe(ProbeContext ctx) { }
    }
}
