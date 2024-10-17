using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace np_sync_sockets
{
    class Program
    {
        static string address = "127.0.0.1"; 
        static int port = 8080;              
        static Dictionary<string, string> keywordResponses = new Dictionary<string, string>();

        static void Main(string[] args)
        {
         
            LoadKeywordResponses(@"C:\Users\Ron\Desktop\Code\ChatConsole\np_sync_sockets\responses.json");

            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
            UdpClient listener = new UdpClient(ipPoint);

            try
            {
                Console.WriteLine("Server started! Waiting for connection...");

                while (true)
                {
                    IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] data = listener.Receive(ref remoteEndPoint);
                    string message = Encoding.Unicode.GetString(data).ToLower();

                    Console.WriteLine($"{DateTime.Now.ToShortTimeString()}: {message} from {remoteEndPoint}");

               
                    string response = "I don't understand your request.";
                    foreach (var keyword in keywordResponses.Keys)
                    {
                        if (message.Contains(keyword))
                        {
                            response = keywordResponses[keyword];
                            if (keyword == "time") response += DateTime.Now.ToShortTimeString();
                            if (keyword == "date") response += DateTime.Now.ToShortDateString();
                            break;
                        }
                    }

                  
                    data = Encoding.Unicode.GetBytes(response);
                    listener.Send(data, data.Length, remoteEndPoint);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                listener.Close();
            }
        }

  
        static void LoadKeywordResponses(string filePath)
        {
            try
            {
               
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("JSON file not found: " + filePath);
                    return;
                }

                string jsonString = File.ReadAllText(filePath);

              
                if (string.IsNullOrWhiteSpace(jsonString))
                {
                    Console.WriteLine("JSON file is empty.");
                    return;
                }

        
                keywordResponses = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);
                Console.WriteLine("Keyword responses loaded from JSON.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading JSON: " + ex.Message);
            }
        }
    }
}
