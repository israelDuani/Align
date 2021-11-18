using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SecretMission.Entities
{
    [BsonIgnoreExtraElements]
    public class AgentMission
    {
        public string agent { get; set; }
        
        public string country { get; set; }
        
        public string address { get; set; }

        public string date { get; set; }
    }
}
