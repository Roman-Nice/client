using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using LoraClient.Models;

namespace LoraClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ExchangeClient Client { get; set; }
        public MainWindowDataContext Context => MainWindowDataContext.Singleton;

        //public DispatcherTimer Timer { get; set; }
        public BackgroundWorker Mistery { get; set; }

        public List<BitmapImage> CardImages;

        private void LoadImages()
        {
            string[] files = Directory.GetFiles(@"Images");
            CardImages = new List<BitmapImage>();

            foreach (string file in files)
            {
                BitmapImage image = new BitmapImage();
                using (FileStream stream = File.OpenRead(file))
                {
                    image.BeginInit();
                    image.StreamSource = stream;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit();
                }
                CardImages.Add(image);
            }
        }

        public MainWindow(ResponseModel_GameRegisterPlayer intobj)
        {
            LoadImages();
            InitializeComponent();
            Initialize(intobj);

            Context.CardImages = CardImages;
            DesitkyViewModel.Singleton.Images = CardImages;

            this.DataContext = MainWindowDataContext.Singleton;

            Client = ExchangeClient.Singleton;

            Mistery = new BackgroundWorker();
            Mistery.DoWork += UpdateGameState;
            Mistery.RunWorkerAsync();


            /*
            Timer = new DispatcherTimer();
            Timer.Tick += UpdateGameState;
            Timer.Interval = new TimeSpan(0,0,0,2);
            Timer.Start();*/
        }

        public Random Rnd { get; set; } = new Random();
        private async void UpdateGameState(object sender, EventArgs e)
        {
            while (true)
            {
                await Task.Delay(1000);
                ResponseModel_GameState g = new ResponseModel_GameState();
                g = await Client.UpdateAsync(Context.GameID, Context.MainP_ID);

                Context.GameState = g;
                DesitkyViewModel.Singleton.GameState = g;

               

                if (g.PlayMode == 7)
                {
                    bool board = this.Dispatcher.Invoke(
                        () => IsRegularBoard);

                    if(board == true)
                        this.Dispatcher.Invoke(
                            () => SwitchBoard(DesitkyBoard)); 
                }
                else
                {
                    bool board = this.Dispatcher.Invoke(
                        () => IsRegularBoard);

                    if (board == false)
                        this.Dispatcher.Invoke(
                            () => SwitchBoard(RegularBoard));

                    this.Dispatcher.Invoke(
                        () => fe_EndTurn.Visibility = Visibility.Hidden);
                }

                if (g.PlayModeSub != 0 && g.PlayMode == 0 && 
                    (Dispatcher.Invoke(()=> fe_PlayingField.ContentTemplate != MaturiaChoice)))
                {
                    this.Dispatcher.Invoke(
                        () => SwitchBoard(MaturiaChoice));
                }
                //GameState = await Client.TestUpdateAsync();

            }
        }

        private bool IsRegularBoard => fe_PlayingField.ContentTemplate == RegularBoard;

        public DataTemplate RegularBoard => this.FindResource("regularBoard") as DataTemplate;
        public DataTemplate DesitkyBoard => this.FindResource("desitkyBoard") as DataTemplate;
        public DataTemplate MaturiaChoice => this.FindResource("maturitaChoice") as DataTemplate;

        private void SwitchBoard(DataTemplate board)
        {
            fe_EndTurn.Visibility = Visibility.Visible;
            DesitkyViewModel.Singleton.Images = CardImages;
            fe_PlayingField.ContentTemplate = board;
        }

        private void Initialize(ResponseModel_GameRegisterPlayer intobj)
        {
            //UpdatePlayer(intobj.PlayerName, 0, 0, 0);

            Context.GameID = intobj.GameId;
            Context.MainP_ID = intobj.PlayerId;
            Context.MainPlayerName = intobj.PlayerName;
            Context.MainPlayerAvatar = intobj.PlayerAvatar;
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            /*
            Image image = sender as Image;
            DefW = image.Width;
            DefH = image.Height;
            image.Width = DefW + DefW * 0.1;
            image.Height = DefH + DefH * 0.1;
            MessageBox.Show(""+DefW);*/
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            /*
            Image image = sender as Image;
            image.Width = DefW;
            image.Height = DefH;*/
        }

        private void Ellipse_Loaded(object sender, RoutedEventArgs e)
        {
            Ellipse el = sender as Ellipse;

            el.Width = el.Height;
        }

#warning Old Code
        private void UpdatePlayer(dynamic name, dynamic score, dynamic cisloHry, dynamic trestneB)
        {
            fe_Player_Name.Content = name;
            fe_Player_Score.Content = "Score: " + score;
            fe_Player_CisloHry.Content = "Talie: " + cisloHry;
            fe_Player_Trestne.Content = "Trestne: " + trestneB;
        }

#warning Old Code
        private Grid[] Players { get; set; }
        private void UpdateOtherPlayers(ResponseModel_GameState state)
        {
            //fe_MIAN.Children.Clear();
            for (int i = 0; i < state.Players.Length -1; i++)
            {
                PlayerPill p = new PlayerPill(i, state.Players[i]);
                p.Pill.Background = Brushes.Red;
                fe_Main_Player_Display.Children.Add(p.Pill);
                //fe_MIAN.Children.Add(p.Pill);
            }

        }

#warning Old Code
        #region Components
        class PlayerPill
        {
            public Grid Pill { get; set; }

            public PlayerPill(int index, ResponseModel_GameState.ResponseModel_GameState_Player Player)
            {
                Grid Main = new Grid();
                Grid.SetColumn(Main, index);

                double l = 1;
                for (int i = 0; i < 3; i++)
                {
                    RowDefinition rd = new RowDefinition();

                    if (i == 2)
                        l = 1.5;
                    rd.Height = new GridLength(l, GridUnitType.Star);
                    Main.RowDefinitions.Add(rd);
                }

                Viewbox vb = new Viewbox();
                vb.Child = new Label() { Content = Player.PlayerName };
                Main.Children.Add(vb);

                Viewbox vbS = new Viewbox();
                vbS.Child = new Label() { Content = $"Skore: {Player.TotalScore} / Talie: {Player.ActualScore} / TrestneB: {Player.RoundScore}" };
                Main.Children.Add(vbS);

                Viewbox CardsCont = new Viewbox();
                Grid.SetRow(CardsCont, 2);
                Grid Cards = new Grid();

                for (int i = 0; i < 8; i++)
                {
                    ColumnDefinition rd = new ColumnDefinition();

                    rd.Width = new GridLength(1, GridUnitType.Star);
                    Cards.ColumnDefinitions.Add(rd);
                }

                for (int i = 0; i < Player.hand.Length; i++)
                {
                    Image o = new Image();
                    Grid.SetColumn(o, i);
                    Cards.Children.Add(o);
                }

                CardsCont.Child = Cards;
                Main.Children.Add(CardsCont);

                Pill = Main;
            }
        }
        #endregion

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            DesitkyViewModel.Singleton.Images = CardImages;
            fe_PlayingField.ContentTemplate = this.FindResource("desitkyBoard") as DataTemplate;
        }

        private async void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image card = sender as Image;
            bool valid = await MakeMove(card);

            if (valid)
            {
                var n = card.Name.ToCharArray();
                char i = n[n.Length - 1];
                int index = int.Parse(i.ToString());
                Context.RemoveCard(index - 1);
            }
        }

        private async Task<bool> MakeMove(Image card)
        {
            int cardI = GetIndex(card.Source as BitmapImage) + 1;
            Context.UpdateMove(card.Source as BitmapImage);

            RequestModel_GamePlayerMove move = new RequestModel_GamePlayerMove()
            {
                Cards = new int[1] { cardI },
                GameId = Context.GameID,
                PlayerId = Context.MainP_ID,
                MoveNo = Context.GameState.LastMoveNo
            };

            ResponseModel_GamePlayerMove resp = await Client.PlayerMoveAsync(move, Context.GameID);
            if (resp.ValidMove == false)
                Task.Run(()=>MessageBox.Show("Invalid move!"));

            return resp.ValidMove;
        }
        private int GetIndex(BitmapImage bim)
        {
            for (int i = 0; i < Context.CardImages.Count; i++)
            {
                if (Context.CardImages[i] == bim)
                    return i;
            }
            return 404; 
        }

        private async void Concede_Click(object sender, RoutedEventArgs e)
        {
            RequestModel_GamePlayerMove move = new RequestModel_GamePlayerMove()
            {
                Cards = new int[1] { -1 },
                GameId = Context.GameID,
                PlayerId = Context.MainP_ID,
                MoveNo = Context.GameState.LastMoveNo
            };

            ResponseModel_GamePlayerMove resp = await Client.PlayerMoveAsync(move, Context.GameID);
            if (resp.ValidMove == true)
                Task.Run(() => MessageBox.Show("Conceded!"));
        }

        private async void EndTurn_Click(object sender, RoutedEventArgs e)
        {
            RequestModel_GamePlayerMove move = new RequestModel_GamePlayerMove()
            {
                Cards = new int[1] { 0 },
                GameId = Context.GameID,
                PlayerId = Context.MainP_ID,
                MoveNo = Context.GameState.LastMoveNo
            };

            ResponseModel_GamePlayerMove resp = await Client.PlayerMoveAsync(move, Context.GameID);
        }
    }
}
