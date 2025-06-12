using ACTPhotoIDViewer.helpers;
using ACTPhotoIDViewer.Models;
using Avalonia.Media.Imaging;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using ReactiveUI;
using System;

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
            }
        }

        public string fileSettings { get; set; }
        public UserModel User { get; set; }

        //public string Greeting { get; } = "Alan Grant";
        private int cardNumber;

        public int CardNumber
        {
            get { return cardNumber; }
            set
            {
                cardNumber = value;
                this.RaisePropertyChanged(nameof(CardNumber));
                LoadUserInfo(cardNumber.ToString()); // Ensure card number is formatted as a 6-digit string
            }
        }

        private string cardTextEmpty;

        public string CardTextEmpty
        {
            get { return cardTextEmpty; }
            set { cardTextEmpty = value;
            this.RaisePropertyChanged(nameof(CardTextEmpty));
            }
        }


        public MainWindowViewModel()
        {
            fileSettings = "Settings/Settings.json";
            SettingsConfig = ConfigHelper.LoadConfig(fileSettings);

            

            //da.TestConnection();

            //User = da.GetUser("1234567");
            //User = da.GetUser("123456");
            //FullName = $"{User.FirstName} {User.LastName}";
            //ImageFromBinding = User.Photo != null ? new Bitmap(new System.IO.MemoryStream(User.Photo)) : new Bitmap("Assets/images/testpicture.png");
        }

        public void LoadUserInfo(string cardNumber)
        {

            try
            {
                SQLDataAccess da = new SQLDataAccess(fileSettings);
                User = da.GetUser(cardNumber);
                if (User.CardNumber == 0)
                {
                    FullName = "User not found";
                    ImageFromBinding = new Bitmap("Assets/images/testpicture.png");
                    CardTextEmpty = string.Empty; // Clear the empty card text if user is found
                    return;
                }
                else
                {
                    FullName = $"{User.FirstName} {User.LastName}";
                    ImageFromBinding = User.Photo != null ? new Bitmap(new System.IO.MemoryStream(User.Photo)) : new Bitmap("Assets/images/testpicture.png");
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
