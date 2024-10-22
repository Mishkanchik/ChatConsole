using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerApp
{
    public class ChatServer
    {
        const short port = 4040;
        const string JOIN_CMD = "$<join>";
        const string LEAVE_CMD = "$<Leave>";
        UdpClient server;
        IPEndPoint clientEndPoint = null;
        List<IPEndPoint> members ;
        public ChatServer()
        {
            server = new UdpClient(port);
            members = new List<IPEndPoint>();
        }
        public void Start()
        {
            while (true)
            {

                byte[] data = server.Receive(ref clientEndPoint);
                string message = Encoding.UTF8.GetString(data);
                Console.WriteLine($"Message : {message} from : {clientEndPoint}. " +
                    $"Date : {DateTime.Now.ToShortTimeString()}");

                switch (message)
                {
                    case JOIN_CMD:
                        AddMember(clientEndPoint);                        
                        break; 
                    case LEAVE_CMD:
                        LeaveMember(clientEndPoint);                        
                        break;                       
                    default:
                        SendToAll(data);
                        break;
                }
            }
        }

        private void LeaveMember(IPEndPoint member)
        {
            members.Remove(member);
            Console.WriteLine("Member was added!");
        }
        private void AddMember(IPEndPoint member)
        {
            members.Add(member);
            Console.WriteLine("Member was added!");
        }
        private void SendToAll(byte[] data)
        {
            foreach (var member in members)
            {
                server.SendAsync(data, data.Length, member);
            }
        }
    }
    internal class Program
    {
       
        static void Main(string[] args)
        {
            
            ChatServer server = new ChatServer();
            server.Start();
        
        }
    }
}
