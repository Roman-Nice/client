using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;
using LoraClient.Models;

namespace LoraClient
{
    public class DesitkyViewModel
    {
        public static DesitkyViewModel Singleton
        {
            get => singleton ??= new DesitkyViewModel();
        }

        static DesitkyViewModel singleton;

        public List<BitmapImage> Images { get; set; } = new List<BitmapImage>();

        public DesitkyBoard View { get; set; }

        public ResponseModel_GameState GameState { get; set; }

    }
}
