using Configuration.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Configuration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private IConfiguration _config;
        private Auth0Configuration _iOptions;
        private Auth0Configuration _snap;
        private Auth0Configuration _monitor;

        public ValuesController(
            IConfiguration config, 
            IOptions<Auth0Configuration> options,
            IOptionsSnapshot<Auth0Configuration> snap,
            IOptionsMonitor<Auth0Configuration> monitor)
        {
            _config = config;

                //access the value property to read the actual configuration.
            _iOptions = options.Value;

            _snap = snap.Value;

            _monitor = monitor.CurrentValue;
        }

        [HttpGet]
        public IActionResult GetConfigration()
        {
            //weakly typed
           string value =  _config["auth0:retryCount"];

            //strongly typed
            int value2 = _config.GetValue<int>("auth0:retryCount");

            //getting value from root file. 
            string innterValue = _config.GetValue<string>("innerValue");

            var response = new {

                //from the IConfiguration
                weakly_typed_object = value,

                //from strongly typed IConfiguration
                stringly_typed_object = value2,

                //from IOptions<T>
                options_configuration = _iOptions.retryCount,

                //from IOptionsSnapShot<T>
                snapShot_configuration = _snap.retryCount,

                //From IOptionsMonitor<T>
                _monitor_configuration = _monitor.retryCount
            };
            return Ok(response);
        }
    }
}
