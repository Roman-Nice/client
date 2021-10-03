using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LoraClient.Models
{
    public class ResponseModel_GamePlayerMove
    {
        [JsonProperty("gameId")]
        public Int32 GameId { get; set; }

        [JsonProperty("moveNo")]
        public Int32 MoveNo { get; set; }

        [JsonProperty("playerId")]
        public Int32 PlayerId { get; set; }

        [JsonProperty("validMove")]
        public Boolean ValidMove { get; set; }

    }
}