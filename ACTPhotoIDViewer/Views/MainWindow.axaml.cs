using ACTPhotoIDViewer.ViewModels;
using Avalonia.Controls;
using Avalonia.Input;
using System;

namespace ACTPhotoIDViewer.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CardNumberTextBox.KeyDown += CardNumberTextBox_KeyDown;

            // Set focus after initialization
            this.Opened += (_, __) => CardNumberTextBox.Focus();
        }
        

        private void CardNumberTextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && sender is TextBox tb)
            {
                if (DataContext is MainWindowViewModel vm)
                {
                    if (uint.TryParse(tb.Text, out uint cardNumber))
                    {
                        vm.CardNumber = cardNumber;
                    }
                    else
                    {
                        CardNumberTextBox.Text = string.Empty; // Clear the text box if parsing fails
                        // Optionally show a message to the user
                        // Example: MessageBox or set a ViewModel property
                    }
                }
            }
        }
    }
}