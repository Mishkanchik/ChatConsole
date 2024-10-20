using System.Windows;

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
    }
}
