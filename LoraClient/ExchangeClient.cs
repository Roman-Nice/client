using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LoraClient.Models;
using Newtonsoft.Json;

namespace LoraClient
{
    public class ExchangeClient
    {
        private static ExchangeClient singleton;

        public static ExchangeClient Singleton
        {
            get => singleton ??= new ExchangeClient();
            set { singleton = value; }
        }

        Random rnd = new Random();


        private HttpClient Client { get; set; }
        public string Route { get; set; } = "https://intranet.loap.cz/lora/api/v1/game";

        public ExchangeClient()
        {
            Client = new HttpClient();
        }

        public async Task<ResponseModel_GameRegisterPlayer> RegisterAsync(
            RequestModel_GameRegisterPlayer player)
        {
            var obj = JsonConvert.SerializeObject(player);
            var body = new StringContent(obj, Encoding.UTF8, "application/json");

            HttpResponseMessage response =  await Client.PostAsync(Route+"/register", body);

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ResponseModel_GameRegisterPlayer>(json);
        }

        //pplayermove/1
        public async Task<ResponseModel_GamePlayerMove> PlayerMoveAsync(
            RequestModel_GamePlayerMove move, int gameid)
        {
            var obj = JsonConvert.SerializeObject(move);
            var body = new StringContent(obj, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await Client.PostAsync(Route + "/playermove/"+ gameid, body);

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ResponseModel_GamePlayerMove>(json);
        }

        public async Task<ResponseModel_GameState> UpdateAsync(int gameid, int playerid)
        {
            HttpResponseMessage response = await Client.PostAsync(Route +"/state/"+gameid+"/"+playerid, null);
            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ResponseModel_GameState>(json);
        }

        ///TEST
#pragma warning disable CS1998
        public async Task<ResponseModel_GameState> TestUpdateAsync()
        {
            throw new InvalidOperationException();
            ResponseModel_GameState response = new ResponseModel_GameState()
            {
                GameId = 1,
                Talon = new int[] { 0, 0 },
                LastMoveNo = 0,
                Players = new ResponseModel_GameState.ResponseModel_GameState_Player[]
                {
                    new ResponseModel_GameState.ResponseModel_GameState_Player()
                    {
                        TotalScore = 0,
                        ActualScore = 0,
                        RoundScore = 0,
                        hand = new int[] {0,0,0,0,0,0,0 },
                        PlayerAvatar = "default",
                        PlayerId = 3,
                        PlayerName = "Player"+rnd.Next(1,20)
                    },
                    new ResponseModel_GameState.ResponseModel_GameState_Player()
                    {
                        TotalScore = 0,
                        ActualScore = 0,
                        RoundScore = 0,
                        hand = new int[] {0,0,0,0,0,0,0,0},
                        PlayerAvatar = "default",
                        PlayerId = 4,
                        PlayerName = "Player"+rnd.Next(1,20)
                    },
                    new ResponseModel_GameState.ResponseModel_GameState_Player()
                    {
                        TotalScore = 0,
                        ActualScore = 0,
                        RoundScore = 0,
                        hand = new int[] {0,0,0,0,0,0,0,0},
                        PlayerAvatar = "default",
                        PlayerId = 4,
                        PlayerName = "Player"+rnd.Next(1,20)
                    },
                    new ResponseModel_GameState.ResponseModel_GameState_Player()
                    {
                        TotalScore = 0,
                        ActualScore = 0,
                        RoundScore = 0,
                        hand = new int[] {0,0,0,0,0,0,0,0},
                        PlayerAvatar = "default",
                        PlayerId = 4,
                        PlayerName = "Player"+rnd.Next(1,20)
                    }
                },
                PlayModeSub = 0,
                PlayerIdOnMove = rnd.Next(1,4),
                PlayMode = 1,
                RoundNo = 1,
            };
            return response;
        }
#pragma warning restore CS1998
    }
}
