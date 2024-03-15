using SimpleIRCLib.Clients;
using SimpleIRCLib.Exceptions;
using System;
using System.Threading;

namespace SimpleIRCLib
{
    /// <summary>
    /// A combiner class that combines all the logic from both the IrcClient & DCCClient with simple methods to control these clients.
    /// </summary>
    public class SimpleIrc : IDisposable
    {
        /// <summary>
        /// Ip address of irc server
        /// </summary>
        public string NewIP { get; set; }

        private int _NewPort;
        private bool disposedValue;

        /// <summary>
        /// Port of irc server to connect to
        /// </summary>
        public int NewPort
        {
            get
            {
                return _NewPort;
            }
            set
            {
                _NewPort = value;
                if (_NewPort > 65535 || _NewPort <= 9)
                {
                    throw new InvalidPortException($"Port {_NewPort} is not valid");
                }
            }
        }

        /// <summary>
        /// Username to register on irc server
        /// </summary>
        public string NewUsername { get; set; }

        /// <summary>
        /// List with channels seperated with ',' to join when connecting to IRC server
        /// </summary
        public string NewChannels { get; set; }

        /// <summary>
        /// Password to connect to a secured irc server
        /// </summary>
        public string NewPassword { get; set; }

        /// <summary>
        /// download directory used for DCCClient.cs
        /// </summary>
        public string DownloadDir { get; set; }

        /// <summary>
        /// Irc Client for sending and receiving messages to a irc server 
        /// </summary>
        public IrcClient IrcClient { get; set; }

        /// <summary>
        /// DCCClient used by the IRCClient for starting a download on a separate thread using the DCC Protocol
        /// </summary>
        public DCCClient DccClient { get; set; }


        /// <summary>
        /// Constructor, sets up bot ircclient and dccclient, so that users can register event handlers.
        /// </summary>
        public SimpleIrc()
        {
            this.IrcClient = new IrcClient();
            this.DccClient = new DCCClient();
        }


        /// <summary>
        /// Setup the client to be able to connect to the server
        /// </summary>     
        /// <param name="ip">Server address, possibly works with dns addresses (irc.xxx.x), but ip addresses works just fine (of type string)</param>
        /// <param name="username">Username the client wants to use, of type string</param>
        /// <param name="channels">Channel(s) the client wants to connect to, possible to connect to multiple channels at once by seperating each channel with a ',' (Example: #chan1,#chan2), of type string</param>
        /// <param name="port">Port, optional parameter, where default = 0 (Automatic port selection), is port of the server you want to connect to, of type int</param>
        /// <param name="password">Password, optional parameter, where default value is "", can be used to connect to a password protected server.</param>
        /// <param name="timeout">Timeout, optional parameter, where default value is 3000 milliseconds, the maximum time before a server needs to answer, otherwise errors are thrown.</param>
        /// <param name="enableSSL">Timeout, optional parameter, where default value is 3000 milliseconds, the maximum time before a server needs to answer, otherwise errors are thrown.</param>
        public void SetupIrc(string ip, string username, string channels, int port = 0, string password = null, int timeout = 3000, bool enableSSL = true, bool acceptAllCertificates = true)
        {
            this.NewIP = ip;
            this.NewPort = port;
            this.NewUsername = username;
            this.NewPassword = password;
            this.NewChannels = channels;

            this.IrcClient.SetConnectionInformation(ip, username, channels, this.DccClient, this.DownloadDir, port, password, timeout, enableSSL, acceptAllCertificates);
        }

        /// <summary>
        /// Sets the download directory for dcc downloads.
        /// </summary>
        /// <param name="downloaddir"> Requires a path to a directory of type string as parameter.</param>
        public void SetCustomDownloadDir(string downloaddir)
        {
            this.DownloadDir = downloaddir;
            this.IrcClient.SetDownloadDirectory(downloaddir);
        }

        /// <summary>
        /// Starts the irc client with the given parameters in the constructor
        /// </summary>
        /// <returns>true or false depending if it starts succesfully</returns>
        public bool StartClient()
        {
            if (this.IrcClient != null)
            {
                if (!this.IrcClient.IsConnectionEstablished())
                {
                    this.IrcClient.Connect();

                    int timeout = 0;
                    while (!this.IrcClient.IsClientRunning())
                    {
                        Thread.Sleep(1);
                        if (timeout >= 3000)
                        {
                            return false;
                        }
                        timeout++;
                    }
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the client is running.
        /// </summary>
        /// <returns>true or false</returns>
        public bool IsClientRunning()
        {
            return this.IrcClient.IsClientRunning();
        }

        /// <summary>
        /// Stops the client
        /// </summary>
        /// <returns>true or false depending on succes</returns>
        internal void StopClient()
        {
            this.IrcClient.Dispose();
            this.IrcClient.StopXDCCDownload();
        }

        /// <summary>
        ///returns true or false upon calling this method, for telling you if the downlaod has been stopped or not
        /// </summary>
        /// <returns></returns>
        public bool StopXDCCDownload()
        {
            return this.IrcClient.StopXDCCDownload();
        }

        /// <summary>
        ///returns true or false upon calling this method, for telling you if the downlaod has been stopped or not
        /// </summary>
        /// <returns></returns>
        public bool CheckIfDownload()
        {
            return this.IrcClient.CheckIfDownloading();
        }

        /// <summary>
        ///get users in current channel
        /// </summary>
        public void GetUsersInCurrentChannel()
        {
            this.IrcClient.GetUsersInChannel();
        }

        /// <summary>
        ///get users in different channel, parameter is the channel name of type string (example: "#yourchannel")
        /// </summary>
        /// <param name="channel"></param>
        public void GetUsersInDifferentChannel(string channel)
        {
            this.IrcClient.GetUsersInChannel(channel);
        }

        /// <summary>
        ///send message to all channels
        /// </summary>
        /// <param name="message">message to send</param>
        /// <returns>true if succesfully send</returns>
        public bool SendMessageToAll(string message)
        {
            return this.IrcClient.SendMessageToAll(message);
        }

        /// <summary>
        /// Sends a message to a specific channel.
        /// </summary>
        /// <param name="message">message to send</param>
        /// <param name="channel">channel for destination</param>
        /// <returns>true/false depending if sending was succesfull</returns>
        public bool SendMessageToChannel(string message, string channel)
        {
            return this.IrcClient.SendMessageToChannel(message, channel);
        }

        /// <summary>
        /// Sends a raw message to irc server
        /// </summary>
        /// <param name="message">message to send</param>
        /// <returns>true/false depending if sending was succesfull</returns>
        public bool SendRawMessage(string message)
        {
            return this.IrcClient.SendRawMsg(message);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.StopClient();
                    this.IrcClient.StopClient();
                    if (this.DccClient.CheckIfDownloading())
                    {
                        this.DccClient.AbortDownloader(1);
                    }
                }

                disposedValue = true;
            }
        }
        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
