using System.IO;
using System.Windows;
using System.Windows.Input;

namespace ClientChat
{
    public partial class NicknameWindow : Window
    {
        public string Nickname { get; private set; }

        public NicknameWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Nickname = nicknameTextBox.Text;
            if (string.IsNullOrEmpty(Nickname))
            {
                MessageBox.Show("Please enter a valid nickname.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                DialogResult = true;
                Close();
            }
        }
        private void msgTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OKButton_Click(sender, e);
            }
        }

      
        private void nicknameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}
