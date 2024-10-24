using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
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

namespace ClientChat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IPEndPoint serverEndPoint;
        UdpClient client ;
        private string nickname;
        ObservableCollection<MessageInfo> messages = new ObservableCollection<MessageInfo>();
        NicknameWindow nicknameWindow = new NicknameWindow();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = messages; 
            client = new UdpClient();

            string address = ConfigurationManager.AppSettings["ServerAddress"]!;
            short port = short.Parse(ConfigurationManager.AppSettings["ServerPort"]!);
            serverEndPoint = new IPEndPoint(IPAddress.Parse(address), port);
            LeaveCheck();
            nicknameWindow.ShowDialog();
            nickname = nicknameWindow.Nickname;
            NicknameTextBox.Text = nickname;


        }
      
        private void Leave_Button_Click(object sender, RoutedEventArgs e)
        {

            string message = "$<Leave>";
            SendMessage(message);
            LeaveCheck();

        }

        private async void Join_Button_Click(object sender, RoutedEventArgs e)
        {

            string message = "$<join>";  
            SendMessage(message);
            Listen();
            JoinCheck();

        }


        private void Send_Button_Click(object sender, RoutedEventArgs e)
        {
            if (msgTextBox.Text == "")
                return;  
            else
            {

                string message = $"|{nickname}|: {msgTextBox.Text}";
                
                SendMessage(FormatText(message));
                msgTextBox.Text = "";
            }
        }
        private string FormatText(string text)
        {
            string trimmedText = text.Trim();
            StringBuilder formattedText = new StringBuilder(trimmedText);

            int index = 50;
            while (index < formattedText.Length)
            {
                formattedText.Insert(index, "\n\n");
                index += 51;
            }

            return formattedText.ToString();
        }


     
        private async void SendMessage(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(data, data.Length, serverEndPoint);
        }
        private async void Listen()
        {
            while (true)
            {
                var data = await client.ReceiveAsync();
                string message = Encoding.UTF8.GetString(data.Buffer);
                messages.Add(new MessageInfo(message, DateTime.Now));
            }
        }


        private void LeaveCheck()
        {
            Join_Button.IsEnabled = true;
            Leave_Button.IsEnabled = false;
            msgTextBox.IsEnabled = false;
            Send_Button.IsEnabled = false;
        }
        private void JoinCheck()
        {
            Join_Button.IsEnabled = false;
            Leave_Button.IsEnabled = true;
            msgTextBox.IsEnabled = true;
            Send_Button.IsEnabled = true;
        }
    }
}