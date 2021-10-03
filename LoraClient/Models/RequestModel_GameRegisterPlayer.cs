using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LoraClient.Models
{
    public class RequestModel_GameRegisterPlayer
    {
        [JsonProperty("playerName")]
        public string PlayerName { get; set; }

        [JsonProperty("playerAvatar")]
        public string PlayerAvatar { get; set; }
    }
}