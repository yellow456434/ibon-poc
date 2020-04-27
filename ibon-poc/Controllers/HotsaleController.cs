using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using GrpcGreeter;
using QueueContract;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ibon_poc.Controllers
{
    [ApiController]
    [Route("hotsale")]
    public class HotSaleController : ControllerBase
    {
        private readonly ILogger<HotSaleController> _logger;
        private readonly IConfiguration _config;
        private readonly IPublishEndpoint _ipublish;

        public HotSaleController(ILogger<HotSaleController> logger,
            IConfiguration config, IPublishEndpoint ipublish)
        {
            _logger = logger;
            _config = config;
            _ipublish = ipublish;
        }

        [HttpGet]
        [Route("get")]
        public async Task<string> Get(int id)
        {
            var config = _config["test"];
            var secret = _config["key"];
            var env_nodeName = Environment.GetEnvironmentVariable("nodeName");
            var env_podName = Environment.GetEnvironmentVariable("podName");

            var msg = "config: " + (config ?? " read fail") + " secret: " + (secret ?? " read fail");
            var msg2 = "env_nodeName: " + (env_nodeName ?? " read fail") + " env_podName: " + (env_podName ?? " read fail");

            _logger.LogInformation(msg);
            _logger.LogInformation(msg2);

            return msg+" "+msg2;
        }

        [HttpGet]
        [Route("grpc")]
        public async Task<string> Grpc(string t)
        {
            var grpcHost = Environment.GetEnvironmentVariable("grpcHost");
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            var channel = GrpcChannel.ForAddress("http://" + grpcHost + ":" + _config["grpcPort"]);
            var client = new Greeter.GreeterClient(channel);            

            var creply = await client.SayCheersAsync(new CheersRequest
            {
                Bol = true,
                Name = t,
                Stringlt = { new List<string> { "A", "B", "C", "D", "E" } },
                Numberlt = { new List<int>() { 1, 2, 3, 4, 5 } },
                Birthday = Timestamp.FromDateTime(DateTime.Now.ToUniversalTime()),
            });

            return creply.Message;
        }

        [HttpGet]
        [Route("publish")]
        public async Task Publish(string t)
        {
            await _ipublish.Publish<OrderMsg>(new
            {
                id = new Random().Next(),
                name = t,
                DateTime = DateTime.Now
            });
        }

        [HttpGet]
        [Route("publish2")]
        public async Task Publish2(string t)
        {
            await _ipublish.Publish<PriceMsg>(new
            {
                uuid = new Random().Next(),
                price = t
            });
        }
    }
}
