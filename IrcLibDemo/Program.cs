using System.Collections.Generic;
using System;
using SimpleIRCLib.EventArgs;
using SimpleIRCLib;

namespace IrcLibDemo
{
    class Program
    {
        private static SimpleIrc irc;
        static void Main(string[] args)
        {
            //setup vars
            string ip;
            int port;
            string username;
            string channel;

            Console.WriteLine("Server IP(default is : irc.rizon.net) = ");
            if ((ip = Console.ReadLine()) == "")
            {
                ip = "irc.abjects.net";
            }

            Console.WriteLine("Server Port(default is : 6697 with ssl enabled) = ");
            if (Console.ReadLine() != "")
            {
                port = Convert.ToInt32(Console.ReadLine());
            }
            else
            {
                port = 6697;
            }

            Console.WriteLine("Username(default is : RareIRC_Client) = ");
            if ((username = Console.ReadLine()) == "")
            {
                username = "AbstractShitFace";
            }

            Console.WriteLine("Channel(default is : #RareIRC) = ");
            if ((channel = Console.ReadLine()) == "")
            {
                channel = "#beast-xdcc";
            }

            irc = new SimpleIrc();

            irc.SetupIrc(ip, username, channel, port, acceptAllCertificates: true);

            irc.IrcClient.OnDebugMessage += DebugOutputCallback;
            irc.IrcClient.OnMessageReceived += ChatOutputCallback;
            irc.IrcClient.OnRawMessageReceived += RawOutputCallback;
            irc.IrcClient.OnUserListReceived += UserListCallback;

            irc.DccClient.OnDccDebugMessage += DccDebugCallback;
            irc.DccClient.OnDccEvent += DownloadStatusChanged;

            irc.StartClient();

            while (true)
            {
                string Input = Console.ReadLine();
                if (string.IsNullOrEmpty(Input) && irc.IsClientRunning())
                {
                    irc.SendMessageToAll(Input);
                }

            }
        }

        public static void DownloadStatusChanged(object source, DCCEventArgs args)
        {
            Console.WriteLine("===============DCC EVENT===============");
            Console.WriteLine("DOWNLOAD Bot: " + args.Bot);
            Console.WriteLine("DOWNLOAD BytesPerSecond: " + args.BytesPerSecond);
            Console.WriteLine("DOWNLOAD DccString: " + args.DccString);
            Console.WriteLine("DOWNLOAD STATUS: " + args.Status);
            Console.WriteLine("DOWNLOAD FILENAME: " + args.FileName);
            Console.WriteLine("DOWNLOAD PROGRESS: " + args.Progress + "%");
            Console.WriteLine("===============END DCC EVENT===============");
            Console.WriteLine("");
        }

        public static void ChatOutputCallback(object source, IrcReceivedEventArgs args)
        {
            Console.WriteLine("===============IRC MESSAGE===============");
            Console.WriteLine(args.Channel + " | " + args.User + ": " + args.Message);
            Console.WriteLine("===============END IRC MESSAGE===============");
            Console.WriteLine("");
        }

        public static void RawOutputCallback(object source, IrcRawReceivedEventArgs args)
        {
            Console.WriteLine("===============RAW MESSAGE===============");
            Console.WriteLine("RAW: " + args.Message);
            Console.WriteLine("===============END RAW MESSAGE===============");
        }

        public static void DebugOutputCallback(object source, IrcDebugMessageEventArgs args)
        {
            Console.WriteLine("===============IRC DEBUG MESSAGE===============");
            Console.WriteLine(args.Type + "|" + args.Message);
            Console.WriteLine("===============END IRC DEBUG MESSAGE===============");
        }

        public static void UserListCallback(object source, IrcUserListReceivedEventArgs args)
        {
            foreach (KeyValuePair<string, List<string>> usersPerChannel in args.UsersPerChannel)
            {
                Console.WriteLine("===============USERS ON CHANNEL " + usersPerChannel.Key + " ===============");
                foreach (string user in usersPerChannel.Value)
                {
                    Console.WriteLine(user);
                }
                Console.WriteLine("===============END USERS ON CHANNEL " + usersPerChannel.Key + " ===============");
            }
        }

        public static void DccDebugCallback(object source, DCCDebugMessageArgs args)
        {
            Console.WriteLine("===============IRC DEBUG MESSAGE===============");
            Console.WriteLine(args.Type + "|" + args.Message);
            Console.WriteLine("===============END IRC DEBUG MESSAGE===============");
        }

    }
}