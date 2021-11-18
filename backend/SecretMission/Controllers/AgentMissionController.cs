using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using SecretMission.DB;
using SecretMission.Entities;
using SecretMission.Helpers;
using SecretMission.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SecretMission.Controllers
{
    [ApiController]
    public class AgentMissionController : ControllerBase
    {
        private readonly IDBCommunicator _dbCommunicator;

        // using dependency injection for db selection
        public AgentMissionController(IDBCommunicator communicator)
        {
            _dbCommunicator = communicator;
        }

        [Route("mission")]
        [HttpPost]
        public async Task<IActionResult> AddAgentMissionRequest([FromBody] AgentMission agentMission) 
        {
            try
            {
                await _dbCommunicator.AddAgentMissionAsync(agentMission);
                return Ok();
            }

            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.ToString());
            }
        }

        [Route("countries-by-isolation")]
        [HttpGet]
        public async Task<IActionResult> GetMostIsolatedCountryRequest()
        {
            try
            {
                var result = await AgentMissionService.GetMostIsolatedCountry(_dbCommunicator);
                return Ok(result);
            }

            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.ToString());
            }
        }

        [Route("find-closest")]
        [HttpPost]
        [ReadableBodyStream]
        public async Task<IActionResult> AddAgentMissionRequest()
        {
            try
            {
                using (StreamReader stream = new StreamReader(HttpContext.Request.Body))
                {
                    string body = await stream.ReadToEndAsync();
                    var data = BsonDocument.Parse(body);
                    var address = data.GetValue("target-location").AsString;

                    AgentMission closestMission = await AgentMissionService.GetClosestMission(_dbCommunicator, address);
                    return Ok(closestMission);
                }
            }

            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.ToString());
            }
        }
    }
}
