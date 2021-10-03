using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LoraClient.Models
{
    public class ResponseModel_GameState
    {
        [JsonProperty("gameId")]
        public Int32 GameId { get; set; }

        [JsonProperty("roundNo")] //Thalie, 0 = matchmaking; 1..4 = index hráče na fořtu
        public Int32 RoundNo { get; set; }

        /// <summary>
        /// co se hraje za hru (červený, filky, prpo, všechny, King, kvarty, desítky, harakiri)
        /// </summary>
        [JsonProperty("playMode")] //co se hraje za hru (červený, filky, prpo, všechny, kvarty, desítky, harakiri)
        public Int32 PlayMode { get; set; }

        /// <summary>
        /// 0 = bežná hra, 1 = 1.kolo maturity, 2 = 2.kolo maturity, 3 = 3.kolo maturity
        /// </summary>
        [JsonProperty("playModeSub")] //0 = bežná hra, 1 = 1.kolo maturity, 2 = 2.kolo maturity, 3 = 3.kolo maturity
        public Int32 PlayModeSub { get; set; }

        /// <summary>
        /// Id hráče, který je na řadě, na kterého se čeká; 0 .. může hrát kdokoliv (kvarty)
        /// </summary>
        [JsonProperty("playerIdOnMove")] //Id hráče, který je na řadě, na kterého se čeká; 0 .. může hrát kdokoliv (kvarty)
        public Int32 PlayerIdOnMove { get; set; }

        [JsonProperty("players")] //hráči u stolu
        public ResponseModel_GameState_Player[] Players { get; set; }

        [JsonProperty("talon")] //karty na stole, na pořadí záleží (kromě desítek)
        public Int32[] Talon { get; set; }

        /// <summary>
        /// číslo posledního tahu, použije se pro RequestModel_GamePlayerMove.MoveNo
        /// </summary>
        [JsonProperty("lastMoveNo")] //číslo posledního tahu, použije se pro RequestModel_GamePlayerMove.MoveNo
        public Int32 LastMoveNo { get; set; }

        public class ResponseModel_GameState_Player
        {
            [JsonProperty("playerId")]
            public Int32 PlayerId { get; set; }

            [JsonProperty("playerName")]
            public string PlayerName { get; set; }

            [JsonProperty("playerAvatar")]
            public string PlayerAvatar { get; set; }

            /// <summary>
            /// Score
            /// </summary>
            [JsonProperty("totalScore")]
            public Int32 TotalScore { get; set; } 

            /// <summary>
            /// Score v talii
            /// </summary>
            [JsonProperty("roundScore")]
            public Int32 RoundScore { get; set; }

            /// <summary>
            /// ?
            /// </summary>
            [JsonProperty("actualScore")]
            public Int32 ActualScore { get; set; }

            [JsonProperty("hand")] //karty na ruce
            public Int32[] hand { get; set; }
        }
    }
}