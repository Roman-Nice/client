using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LoraClient.Models;

namespace LoraClient
{
    public class MainWindowDataContext : INotifyPropertyChanged
    {
        public static MainWindowDataContext Singleton { get => _singleton ??= new MainWindowDataContext(); }
        static MainWindowDataContext _singleton;


        public int GameID { get; set; }
        public int MainP_ID { get; set; }

        public List<BitmapImage> CardImages { get; set; }

        private string roundNo;
        public string RoundNo
        {
            get 
            {
                /*foreach(string value in NotifyValue(test, "Test"))
                {
                    return value;
                }*/
                return "Talie: "+roundNo;
            }
            set { roundNo = value; NotifyPropertyChanged(); }
        }

        private string gameType;
        public string GameType
        {
            get
            {
                /*foreach(string value in NotifyValue(test, "Test"))
                {
                    return value;
                }*/
                return "Hra: "+gameType;
            }
            set { gameType = value; NotifyPropertyChanged(); }
        }

        public ResponseModel_GameState GameState { get => gameState; set => SetGameState(value); }
        ResponseModel_GameState gameState;

        private string[] games = new string[] { "červený", "filky", "prpo", "všechny", "bedrník", "kvarty", "desítky", "harakiri" };
        private async void SetGameState(ResponseModel_GameState value)
        {
            try
            {
                gameState = value;

                string mat = "";
                if (GameState.PlayModeSub != 0)
                    mat = "Maturita " + GameState.PlayModeSub + " ";

                string m = GameState.PlayMode == 0 ? "..." : mat + games[GameState.PlayMode -1];
                GameType = m;

                RoundNo = GameState.Players.Length != 4 ? "Waiting for players" : GameState.Players[GameState.RoundNo - 1].PlayerName;

                if (GameState.Players != null)
                    await ValueProcessor();
            }
            catch { }

        }

#pragma warning disable CS1998
        private async Task ValueProcessor()
        {
            //players
            MainPlayer = new ResponseModel_GameState.ResponseModel_GameState_Player();
            OtherPlayers = 
                new List<ResponseModel_GameState.ResponseModel_GameState_Player>();

            //MainP
            foreach (var player in GameState.Players)
            {
                if (player.PlayerId == MainP_ID)
                    MainPlayer = player;
                else
                    OtherPlayers.Add(player);
            }
            SetPlayers(OtherPlayers);

            MainPlayerScore = MainPlayer.TotalScore.ToString();
            MainPlayerTalie = MainPlayer.RoundScore.ToString();
            MainPlayerTrestne = MainPlayer.ActualScore.ToString();
            MainPlayerColor = GameState.PlayerIdOnMove == MainP_ID ? Brushes.Orange : null;

            if (GameState.Players.Length == 4 && GameState.RoundNo != 0)
                await GameUpdate();
        }

        public async Task UpdateMove(BitmapImage Card)
        {
            int i = 0;
            foreach (var card in PlayerCards)
            {
                if (card == Card)
                {
                    var temp = PlayerCards.FindLast(match => match != null);
                    int index = PlayerCards.IndexOf(temp);

                    PlayerCards[index] = null;
                    PlayerCards[i] = temp;
                }
            }
        }

        private async Task GameUpdate()
        {
            SetPlayerCards();
            if(GameState.PlayMode != 7)
                SetTalonCards();
        }

        private async void SetTalonCards()
        {
            if(GameState.Talon.Length == 0)
                for (int i1 = 0; i1 < TalonCards.Count; i1++)
                    TalonCards[i1] = null;
                

            for (int i = 0; i < GameState.Talon.Length; i++)
            {
                int card = GameState.Talon[i] - 1;
                TalonCards[i] = CardImages[card];
            }

            await CardsChanged("TalonCard", 4);
        }

        private async void SetPlayerCards()
        {

            for (int i1 = 0; i1 < MainPlayer.hand.Length; i1++)
            {
                int card = MainPlayer.hand[i1] -1;
                PlayerCards[i1] = CardImages[card];
            }

            /*for (int i = MainPlayer.hand.Length; i < 8; i++)
            {
                


            }*/

            await CardsChanged("Card", 8);
        }

        public async void RemoveCard(int index)
        {
            PlayerCards[index] = null;
            NotifyPropertyChanged("Card" + (index + 1));
        }

        private async Task CardsChanged(string PropertyName, int length, int start = 1)
        {
            for (int i = start; i <= length; i++)
            {
                NotifyPropertyChanged(PropertyName + i);
            }
        }

        private List<BitmapImage> Default(int count, BitmapImage defaultImage = null)
        {
            List<BitmapImage> r = new List<BitmapImage>();
            for (int i = 0; i < count; i++)
            {
                r.Add(defaultImage);
            }
            return r;
        }
#pragma warning restore CS1998

        #region MainPlayer
        ResponseModel_GameState.ResponseModel_GameState_Player MainPlayer { get; set; }
        private string MainPlayer_name;
        public string MainPlayerName
        {
            get { return MainPlayer_name; }
            set 
            {
                if (GameState != null)
                {
                   MainPlayer_name = value;
                }
                else
                    MainPlayer_name = value;
                NotifyPropertyChanged(); 
            }
        }

        #region Players on move

        private Brush mainPlayerColor;
        public Brush MainPlayerColor
        {
            get 
            {
                return mainPlayerColor;
            }
            set 
            {
                mainPlayerColor = value;
                NotifyPropertyChanged();
            }
        }

        private Brush player1Color;
        public Brush Player1Color
        {
            get
            {
                return player1Color;
            }
            set
            {
                player1Color = value;
                NotifyPropertyChanged();

            }

        }
        private Brush player2Color;
        public Brush Player2Color
        {
            get
            {
                return player2Color;
            }
            set
            {
                player2Color = value;
                NotifyPropertyChanged();

            }
        }

        private Brush player3Color;
        public Brush Player3Color
        {
            get
            {
                return player3Color;
            }
            set
            {
                player3Color = value;
                NotifyPropertyChanged();
            }

        }
        #endregion

        private string MainPlayer_score;
        public string MainPlayerScore
        {
            get { return MainPlayer_score; }
            set { MainPlayer_score = "Score: "+ value; NotifyPropertyChanged(); }
        }

        private string MainPlayer_talie;
        public string MainPlayerTalie
        {
            get { return MainPlayer_talie; }
            set { MainPlayer_talie = "Talie: " + value; NotifyPropertyChanged(); }
        }

        private string MainPlayer_trestne;
        public string MainPlayerTrestne
        {
            get { return MainPlayer_trestne; }
            set { MainPlayer_trestne = "Trestne: " + value; NotifyPropertyChanged(); }
        }

        private string MainPlayer_avatar;
        public string MainPlayerAvatar
        {
            get { return MainPlayer_avatar; }
            set { MainPlayer_avatar = "Trestne: " + value; NotifyPropertyChanged(); }
        }
        #endregion

        #region Visible cards

        List<BitmapImage> PlayerCards { get; set; } = new List<BitmapImage>();
        public BitmapImage Card1 { get => PlayerCards[0]; }
        public BitmapImage Card2 { get => PlayerCards[1]; }
        public BitmapImage Card3 { get => PlayerCards[2]; }
        public BitmapImage Card4 { get => PlayerCards[3]; }
        public BitmapImage Card5 { get => PlayerCards[4]; }
        public BitmapImage Card6 { get => PlayerCards[5]; }
        public BitmapImage Card7 { get => PlayerCards[6]; }
        public BitmapImage Card8 { get => PlayerCards[7]; }

        List<BitmapImage> TalonCards { get; set; } = new List<BitmapImage>();
        public BitmapImage TalonCard1 { get => TalonCards[0]; }
        public BitmapImage TalonCard2 { get => TalonCards[1]; }
        public BitmapImage TalonCard3 { get => TalonCards[2]; }
        public BitmapImage TalonCard4 { get => TalonCards[3]; }

        #endregion

        #region OtherPlayers
        List<ResponseModel_GameState.ResponseModel_GameState_Player> OtherPlayers { get => _OtherPlayers; set => _OtherPlayers = value; }
        List<ResponseModel_GameState.ResponseModel_GameState_Player> _OtherPlayers;

        private void SetPlayers(List<ResponseModel_GameState.ResponseModel_GameState_Player> value)
        {
            _OtherPlayers = value;

            int i = 0;
            if (i < value.Count)
            {
                Player1Color = value[i].PlayerId == GameState.PlayerIdOnMove ? Brushes.Orange : null;
                string name = value[i].PlayerName;
                Player1Name = name;
                Player1Score = $"Skore: {value[i].TotalScore} / Talie: {value[i].RoundScore} / Trestne {value[i].ActualScore}";
                i++;
            }
            if (i < value.Count)
            {
                Player2Color = value[i].PlayerId == GameState.PlayerIdOnMove ? Brushes.Orange : null;
                string name = value[i].PlayerName;
                Player2Name = name;
                Player2Score = $"Skore: {value[i].TotalScore} / Talie: {value[i].RoundScore} / Trestne {value[i].ActualScore}";
                i++;
            }
            if (i < value.Count)
            {
                Player3Color = value[i].PlayerId == GameState.PlayerIdOnMove ? Brushes.Orange : null;
                string name = value[i].PlayerName;
                Player3Name = name;
                Player3Score = $"Skore: {value[i].TotalScore} / Talie: {value[i].RoundScore} / Trestne {value[i].ActualScore}";
            }
        }

        private string P1Name;
        public string Player1Name
        {
            get { return P1Name; }
            set { P1Name = value; NotifyPropertyChanged(); }
        }

        private string P1Score;
        public string Player1Score
        {
            get { return P1Score; }
            set { P1Score = value; NotifyPropertyChanged(); }
        }

        private string P2Name;
        public string Player2Name
        {
            get { return P2Name; }
            set { P2Name = value; NotifyPropertyChanged(); }
        }

        private string P2Score;
        public string Player2Score
        {
            get { return P2Score; }
            set { P2Score = value; NotifyPropertyChanged(); }
        }

        private string P3Name;
        public string Player3Name
        {
            get { return P3Name; }
            set { P3Name = value; NotifyPropertyChanged(); }
        }

        private string P3Score;
        public string Player3Score
        {
            get { return P3Score; }
            set { P3Score = value; NotifyPropertyChanged(); }
        }


        #endregion

        

        public MainWindowDataContext()
        {
            PlayerCards = Default(8);
            TalonCards = Default(4);

            PlayerCards = new List<BitmapImage>() { Card1, Card2, Card3, Card4, Card5, Card6, Card7, Card8 };
            TalonCards = new List<BitmapImage>() { TalonCard1, TalonCard2, TalonCard3, TalonCard4 };
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<dynamic> NotifyValue(dynamic value, string propertyName)
        {
            yield return value;
            NotifyPropertyChanged(propertyName);
        }

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
