using System.IO;
using System.Net.Sockets;

namespace Diamond_Chat_Bot
{
    public class IrcClient
    {
        private string userName;
        private string channel;

        private TcpClient tcpClient;
        private StreamReader inputStream;
        private StreamWriter outputStream;

        public IrcClient(string ip, int port, string userName, string password, string channel)
        {
            this.userName = userName;
            this.channel = channel;

            tcpClient = new TcpClient(ip, port);
            inputStream = new StreamReader(tcpClient.GetStream());
            outputStream = new StreamWriter(tcpClient.GetStream());

            outputStream.WriteLine($"PASS {password}");
            outputStream.WriteLine($"NICK {userName}");
            outputStream.WriteLine($"USER {userName} 8 * :{userName}");
            outputStream.WriteLine($"JOIN #{channel}");
            outputStream.Flush();
        }

        public void SendIrcMessage(string message)
        {
            try
            {
                outputStream.WriteLine(message);
                outputStream.Flush();
            }
            catch { }
        }

        public string ReadMessage()
        {
            try
            {
                return inputStream.ReadLine();
            }
            catch { return "Error"; }
        }


        public void SendChatMessage(string message)
        {
            SendIrcMessage($":{userName}!{userName}@{userName}.tmi.twitch.tv PRIVMSG #{channel} :{message}");
        }
        

    }
}
