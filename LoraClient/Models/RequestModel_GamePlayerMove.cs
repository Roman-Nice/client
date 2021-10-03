using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LoraClient.Models
{
    public class RequestModel_GamePlayerMove
    {
        [JsonProperty("gameId")]
        public Int32 GameId { get; set; }

        [JsonProperty("moveNo")]
        public Int32 MoveNo { get; set; }

        [JsonProperty("playerId")]
        public Int32 PlayerId { get; set; }

        [JsonProperty("cards")]  //ID vyložených karet
        public Int32[] Cards { get; set; }

    }
}