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
        public async Task<string> Get(int id)
        {
            var env = _config["test"];
            var secret = _config["key"];

            var msg = "config: " + (env ?? " read fail") + " secret: " + (secret ?? " read fail");

            _logger.LogInformation(msg);

            return msg;
        }
    }
}
