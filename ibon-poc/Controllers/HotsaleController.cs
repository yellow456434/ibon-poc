using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public HotSaleController(ILogger<HotSaleController> logger,
            IConfiguration config)
        {
            _logger = logger;
            _config = config;
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
    }
}
