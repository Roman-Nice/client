using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LoraClient.Models;

namespace LoraClient
{
    /// <summary>
    /// Interaction logic for NewGame.xaml
    /// </summary>
    public partial class NewGame : Window
    {
        public ExchangeClient Client { get; set; }

        public NewGame()
        {
            InitializeComponent();

            Client = ExchangeClient.Singleton;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            button.IsEnabled = false;

            RequestModel_GameRegisterPlayer rq = 
                new RequestModel_GameRegisterPlayer();

            rq.PlayerName = fe_Name.Text;
            rq.PlayerAvatar = fe_Avatar.Text;

            var response = await Client.RegisterAsync(rq);

            MainWindow mw = new MainWindow(response);
            mw.Show();
            this.Close();
        }

    }
}
