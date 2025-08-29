using ACTPhotoIDViewer.helpers;
using ACTPhotoIDViewer.Models;
using Avalonia.Media.Imaging;
using Avalonia.Rendering;
using Microsoft.Data.SqlClient;
using MsBox.Avalonia.Base;
using Newtonsoft.Json.Linq;
using ReactiveUI;
using System;
using System.Timers;

namespace ACTPhotoIDViewer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        //public string fileName { get; } = "testpicture.png";

        public JObject SettingsConfig { get; set; }

        //private Bitmap? _imageFromBinding;
        //public Bitmap? ImageFromBinding
        //{
        //    get
        //    {
        //        if (_imageFromBinding == null)
        //        {
        //            //_imageFromBinding = ImageHelper.LoadFromResource(new Uri($"avares://ACTPhotoIDViewer/Assets/images/{fileName}"));
        //        }
        //        return _imageFromBinding;
        //    }
        //}

        public SQLDataAccess da { get; set; }

        private Bitmap imageFromBinding;

        public Bitmap ImageFromBinding
        {
            get { return imageFromBinding; }
            set
            {
                imageFromBinding = value;
                this.RaisePropertyChanged(nameof(ImageFromBinding));
            }
        }

        private string fullName;

        public string FullName
        {
            get { return fullName; }
            set
            {
                fullName = value;
                this.RaisePropertyChanged(nameof(FullName));
                cardNumberTimer.Start();
            }
        }

        public string fileSettings { get; set; }
        public UserModel User { get; set; }

        //public string Greeting { get; } = "Alan Grant";
        private uint cardNumber;

        public uint CardNumber
        {
            get { return cardNumber; }
            set
            {
                cardNumber = value;
                this.RaisePropertyChanged(nameof(CardNumber));
                
                LoadUserInfo(cardNumber.ToString()); // Ensure card number is formatted as a 6-digit string
                cardNumberTimer.Stop();
                cardNumberTimer.Start();
            }
        }

        private string cardTextEmpty;

        public string CardTextEmpty
        {
            get { return cardTextEmpty; }
            set { cardTextEmpty = value;
            this.RaisePropertyChanged(nameof(CardTextEmpty));
                cardNumberTimer.Stop();
                cardNumberTimer.Start();
            }
        }

        private Timer cardNumberTimer;
        private const int CardNumberTimeout = 5000; // 5 seconds
        public MainWindowViewModel()
        {
            fileSettings = "Settings/Settings.json";
            SettingsConfig = ConfigHelper.LoadConfig(fileSettings);

            cardNumberTimer = new Timer(CardNumberTimeout);
            cardNumberTimer.Start();
            cardNumberTimer.Elapsed += CardNumberTimer_Elapsed;
            cardNumberTimer.AutoReset = false; // Only trigger once

            //da.TestConnection();

            //User = da.GetUser("1234567");
            //User = da.GetUser("123456");
            //FullName = $"{User.FirstName} {User.LastName}";
            //ImageFromBinding = User.Photo != null ? new Bitmap(new System.IO.MemoryStream(User.Photo)) : new Bitmap("Assets/images/testpicture.png");
        }

        private void CardNumberTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            FullName = "Please Flash Card";
            ImageFromBinding = new Bitmap("Assets/images/testpicture.png");
        }

        public void LoadUserInfo(string cardNumber)
        {

            try
            {
                //uint unsigned = UInt32.Parse(cardNumber);
                //int signed = unchecked((int)unsigned);
                //cardNumber = signed.ToString();

                SQLDataAccess da = new SQLDataAccess(fileSettings);
                User = da.GetUser(cardNumber);
                if (User.CardNumber == 0)
                {
                    FullName = "User not found";
                    ImageFromBinding = new Bitmap("Assets/images/NoUserPhoto.png");
                    CardTextEmpty = string.Empty; // Clear the empty card text if user is found
                    return;
                }
                else
                {
                    FullName = $"{User.FirstName} {User.LastName}";
                    ImageFromBinding = User.Photo != null ? new Bitmap(new System.IO.MemoryStream(User.Photo)) : new Bitmap("Assets/images/NoUserPhoto.png");
                    //CardNumber = User.CardNumber;
                    CardTextEmpty = string.Empty; // Clear the empty card text if user is found
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"An error occurred while loading user data: {ex.Message}");
            }
        }
    }
}
