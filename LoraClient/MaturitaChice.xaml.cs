using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LoraClient.Models;

namespace LoraClient
{
    /// <summary>
    /// Interaction logic for MaturitaChice.xaml
    /// </summary>
    public partial class MaturitaChice : Page
    {
        public MaturitaChice()
        {
            InitializeComponent();
            this.DataContext = this;

            if (MainWindowDataContext.Singleton.GameState.PlayModeSub == 3)
                fe_m_prpo.IsEnabled = true;
            else
                fe_m_prpo.IsEnabled = false;

            if (MainWindowDataContext.Singleton.GameState.PlayerIdOnMove == MainWindowDataContext.Singleton.MainP_ID)
            {
                fe_m_pChoices.Visibility = Visibility.Visible;
                fe_m_description.Content = "Zvolte hru";
            }

            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int g = int.Parse((sender as Button).DataContext.ToString());
            int id = MainWindowDataContext.Singleton.GameID;

            var n = new RequestModel_GamePlayerMove() 
            {
                Cards = new int[1] { g },
                GameId = id,
                MoveNo = MainWindowDataContext.Singleton.GameState.LastMoveNo,
                PlayerId = MainWindowDataContext.Singleton.MainP_ID
            };

            ExchangeClient.Singleton.PlayerMoveAsync(n, id);
        }
    }
}
