using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
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
using LoraClient.Models;

namespace LoraClient
{
    /// <summary>
    /// Interaction logic for DesitkyBoard.xaml
    /// </summary>
    public partial class DesitkyBoard : Page
    {
        private Grid MainGrid => d_fe_MainBoard;

        public DesitkyViewModel Context => DesitkyViewModel.Singleton;


        public List<int> Talon { get; set; } = new List<int>();

        public BackgroundWorker UpdateClock { get; set; }

        public DesitkyBoard()
        {
            InitializeComponent();
            this.DataContext = DesitkyViewModel.Singleton;

            Context.View = this;

            for (int i = 0; i < 4; i++)
            {
                RowDefinition r = new RowDefinition();
                r.Height = new GridLength(1, GridUnitType.Star);
                MainGrid.RowDefinitions.Add(r);
            }

            ColumnDefinition s = new ColumnDefinition();
            s.Width = new GridLength(1.5, GridUnitType.Star);
            MainGrid.ColumnDefinitions.Add(s);
            for (int i = 0; i < 8; i++)
            {
                ColumnDefinition c = new ColumnDefinition();
                c.Width = new GridLength(1, GridUnitType.Star);
                MainGrid.ColumnDefinitions.Add(c);
            }
            ColumnDefinition e = new ColumnDefinition();
            e.Width = new GridLength(1.5, GridUnitType.Star);
            MainGrid.ColumnDefinitions.Add(e);

            Create();

            UpdateClock = new BackgroundWorker();
            UpdateClock.DoWork += UpdateTalon;
            UpdateClock.RunWorkerAsync();
        }

        private void Create()
        {
            for (int r = 1; r <= 4; r++)
            {
                for (int c = 1; c <= 8; c++)
                {
                    Image i = new Image();
                    i.Source = null;
                    Grid.SetColumn(i, c);
                    Grid.SetRow(i, r);

                    MainGrid.Children.Add(i);
                }
            }
            MainGrid.ShowGridLines = true;
        }

        public async void UpdateTalon(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                await Task.Delay(1000);
                if (Context.GameState.Talon.Length != 0 && Context.GameState != null)
                    await Update(Context.GameState.Talon);
            }
        }

        public async Task Update(int[] game)
        {
            Talon = new List<int>();
            foreach (int item in game)
            {
                Talon.Add(item);
            }

            if (Context.Images != null)
                this.Dispatcher.Invoke(
                    () => update(Context.Images));
            else
                MessageBox.Show("what");
        }

        private void update(List<BitmapImage> images)
        {
            var temp = MainGrid;

            d_fe_MainBoard.Children.Clear();


            for (int i = 0; i < 32; i++)
            {
                if (Talon.Contains(i+1))
                {
                    Image im = new Image();
                    im.Source = images[i];

                    int c = (i - ((i / 8) * 8));
                    Grid.SetColumn(im, c);
                    Grid.SetRow(im, (i / 8));

                    temp.Children.Add(im);
                }
                else
                {
                    Image im = new Image();
                    im.Source = null;

                    int c = (i - ((i / 8) * 8));
                    Grid.SetColumn(im, c);
                    Grid.SetRow(im, (i / 8));

                    temp.Children.Add(im);
                }
            }
            d_fe_MainBoard = temp;
            MainGrid.UpdateLayout();
        }
    }
}
