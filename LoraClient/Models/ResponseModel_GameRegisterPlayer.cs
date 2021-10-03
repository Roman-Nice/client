using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LoraClient.Models
{
    public class ResponseModel_GameRegisterPlayer
    {
        [JsonProperty("gameId")]
        public Int32 GameId { get; set; }

        [JsonProperty("playerId")]
        public Int32 PlayerId { get; set; }

        [JsonProperty("playerName")]
        public string PlayerName { get; set; }

        [JsonProperty("playerAvatar")]
        public string PlayerAvatar { get; set; }

    }
}