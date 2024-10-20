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
     
        ObservableCollection<MessageInfo> messages = new ObservableCollection<MessageInfo>();   
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = messages; 
            client = new UdpClient();

            string address = ConfigurationManager.AppSettings["ServerAddress"]!;
            short port = short.Parse(ConfigurationManager.AppSettings["ServerPort"]!);
            serverEndPoint = new IPEndPoint(IPAddress.Parse(address), port);
            LeaveCheck();
        }
      
        private void Leave_Button_Click(object sender, RoutedEventArgs e)
        {
           
           
            string message = "$<Leave>";
            SendMessage(message);
            LeaveCheck();



        }
        
        private void Join_Button_Click(object sender, RoutedEventArgs e)
        {
        
            string message = "$<join>";
            SendMessage(message);
            Listen();
            JoinCheck();

        }

        private void Send_Button_Click(object sender, RoutedEventArgs e)
        {

          
               
                if (msgTextBox.Text == "")
                {
                    MessageBox.Show("Рядок пустий", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    
                }
                else
                {
                    string message = msgTextBox.Text;
                    SendMessage(message);
                    msgTextBox.Text = "";
                    
                }
           
        }
       
        private void msgTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) 
            {
                Send_Button_Click(sender, e); 
            }
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