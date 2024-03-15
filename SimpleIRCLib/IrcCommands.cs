using SimpleIRCLib.ErrorCodes;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading;

namespace SimpleIRCLib
{
    /// <summary>
    /// Class used for sending specific commands to the IRC server, and waiting for responses from the irc server before continueing
    /// </summary>
    public class IrcCommands : IDisposable
    {
        /// <summary>
        /// reader to read from the irc server stream
        /// </summary>
        private readonly StreamReader _reader;

        /// <summary>
        /// reader to write to the irc server stream
        /// </summary>
        private readonly StreamWriter _writer;

        /// <summary>
        /// global error message string, used for defining the error that might occur with a readable string
        /// </summary>
        private string _errorMessage;

        /// <summary>
        /// global error integer, used for getting the specific error id specified within the IRC specs RFC 2812
        /// </summary>
        private int _responseNumber;

        /// <summary>
        /// user name that has been registered
        /// </summary>
        private string _username;

        /// <summary>
        /// channel that hs been joined
        /// </summary>
        private string _channels;
        private bool disposedValue;

        /// <summary>
        /// Constructor, that requires the stream reader and writer set before initializing
        /// </summary>
        /// <param name="stream">Stream to read/write from/to</param>
        public IrcCommands(NetworkStream stream)
        {
            this._reader = new StreamReader(stream);
            this._writer = new StreamWriter(stream);
        }

        /// <summary>
        /// Constructor, that requires the stream reader and writer set before initializing
        /// </summary>
        /// <param name="stream">Stream to read/write from/to</param>
        public IrcCommands(SslStream stream)
        {
            this._reader = new StreamReader(stream);
            this._writer = new StreamWriter(stream);
        }

        /// <summary>
        /// Get the error message that probably has occured when calling this method
        /// </summary>
        /// <returns>returns error message</returns>
        public string GetErrorMessage()
        {
            return this._errorMessage;
        }

        /// <summary>
        /// Get the error number that probably had occured when calling this method (RFC 2812)
        /// </summary>
        /// <returns></returns>
        public int GetErrorNumber()
        {
            return this._responseNumber;
        }

        /// <summary>
        /// Sets the password for a connection and waits if there is any response, if there is, it may continue, unless the reponse contains an error message
        /// </summary>
        /// <param name="password">password to set</param>
        /// <returns>true/false depending if error occured</returns>
        public bool SetPassWord(string password)
        {
            _writer.WriteLine("PASS " + password + Environment.NewLine);
            _writer.Flush();

            while (true)
            {
                string ircData = _reader.ReadLine();
                if (ircData.Contains("462"))
                {
                    this._responseNumber = 462;
                    this._errorMessage = "PASSWORD ALREADY REGISTERED";
                    return false;
                }
                else if (ircData.Contains("461"))
                {
                    this._responseNumber = 461;
                    this._errorMessage = "PASSWORD COMMAND NEEDS MORE PARAMETERS";
                    return false;
                }
                else if (ircData.Contains("004"))
                {
                    return true;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Sends the user and nick command to the irc server, checks for error messages (it waits for a reply to come through first, before deciding what to do). 
        /// </summary>
        /// <param name="user">Username</param>
        /// <param name="channel"></param>
        /// <returns>True/False, depending if error occured or not</returns>
        public bool JoinNetwork(string user, string channels)
        {
            this._username = user;
            this._channels = channels;
            this._writer.WriteLine("NICK " + user + Environment.NewLine);
            this._writer.Flush();
            this._writer.WriteLine("USER " + user + " 8 * :" + user + "_SimpleIRCLib" + Environment.NewLine);
            this._writer.Flush();

            while (true)
            {
                try
                {
                    string ircData = this._reader.ReadLine();
                    if (ircData != null)
                    {
                        if (ircData.Contains("PING"))
                        {
                            string pingID = ircData.Split(':')[1];
                            this._writer.WriteLine("PONG :" + pingID);
                            this._writer.Flush();
                        }

                        if (this.CheckMessageForError(ircData) && this._responseNumber == 266)
                        {
                            return this.JoinChannel(channels, user);
                        }
                    }
                    Thread.Sleep(1);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("RECEIVED: " + e.ToString());
                    return false;
                }
            }
        }

        /// <summary>
        /// Sends a join request to the irc server, then waits for a response before continueing
        /// </summary>
        /// <param name="channels">Channels to join</param>
        /// <param name="username">Username that joins</param>
        /// <returns>True on sucess, false on error</returns>
        public bool JoinChannel(string channels, string username)
        {
            this._channels = channels;

            this._writer.WriteLine("JOIN " + channels + Environment.NewLine);
            this._writer.Flush();
            while (true)
            {
                string ircData = this._reader.ReadLine();

                if (ircData != null)
                {
                    if (ircData.Contains("PING"))
                    {
                        string pingID = ircData.Split(':')[1];
                        this._writer.WriteLine("PONG :" + pingID);
                        this._writer.Flush();
                    }

                    if (ircData.Contains(username) && ircData.Contains("JOIN"))
                    {
                        return true;
                    }
                }
                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// Sends the set nickname request to the server, waits until a response is given from the server before deciding to continue
        /// </summary>
        /// <param name="nickname">Nickname</param>
        /// <returns>True on success, false on error.</returns>
        public bool SetNickName(string nickname)
        {
            _writer.WriteLine("NICK " + nickname + Environment.NewLine);
            _writer.Flush();

            return this.CheckMessageForError(this._reader.ReadLine());
        }

        public bool CheckMessageForError(string message)
        {
            string codeString = message.Split(' ')[1].Trim();
            if (int.TryParse(codeString, out _responseNumber))
            {
                Rfc1459ErrorCode errorcode = Rfc1459Codes.ListWithErrors.FirstOrDefault(x => $"{x.Code} {x.Name} {x.Description}".Equals(codeString, StringComparison.InvariantCultureIgnoreCase));

                if (errorcode != null)
                {
                    this._errorMessage = $"{errorcode.Code} {errorcode.Name} {errorcode.Description}";
                    return false;
                }

                this._errorMessage = "Message does not contain Error Code!";
                return true;
            }

            Debug.WriteLine("Could not parse number");
            this._responseNumber = 0;
            this._errorMessage = "Message does not contain Error Code, could not parse number!";
            return true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this._reader?.Dispose();
                    this._writer?.Dispose();
                }

                this.disposedValue = true;
            }
        }

        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
