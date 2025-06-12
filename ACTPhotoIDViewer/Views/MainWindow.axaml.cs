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
                    // Example: update a property called UserNumber
                    vm.CardNumber = Int32.Parse(tb.Text);

                    // Optionally, trigger any logic you want here
                }
            }
        }
    }
}