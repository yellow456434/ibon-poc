using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using QueueContract;

namespace Order.QueueConsumer
{
    public class OrderConsumer : IConsumer<OrderMsg>
    {
        private readonly ILogger<OrderConsumer> _logger;

        public OrderConsumer(ILogger<OrderConsumer> logger)
        {
            _logger = logger;
        }


        public async Task Consume(ConsumeContext<OrderMsg> context)
        {
            _logger.LogInformation("##################Order################");
            var msg = context.Message;
            _logger.LogInformation("id:" + msg.id + " name:" + msg.name + " date:" + msg.dateTime);
        }
    }

    public class PriceConsumer : IConsumer<PriceMsg>
    {
        private readonly ILogger<PriceConsumer> _logger;

        public PriceConsumer(ILogger<PriceConsumer> logger)
        {
            _logger = logger;
        }


        public async Task Consume(ConsumeContext<PriceMsg> context)
        {
            throw new Exception("error 12312");
            _logger.LogInformation("################Price##################");
            var msg = context.Message;
            _logger.LogInformation("uuid:" + msg.uuid + " price:" + msg.price);
        }
    }
}
