using System.Collections.Generic;

namespace SimpleIRCLib.EventArgs
{
    /// <summary>
    /// Event class for receiving messages from specific channels and users, fired within IrcClient.cs
    /// </summary>
    public class IrcReceivedEventArgs : System.EventArgs
    {
        /// <summary>
        /// Containing message from user on a specific channel
        /// </summary>
        public string Message { get; }
        /// <summary>
        /// Containing user name whom send the message on a specific channel
        /// </summary>
        public string User { get; }
        /// <summary>
        /// Containing channel name where user send his/hers/its message
        /// </summary>
        public string Channel { get; }

        /// <summary>
        /// Event for receiving messages from server, contains information about who send a message, from where, with the message send.
        /// </summary>
        /// <param name="message">message the user send</param>
        /// <param name="user">name of the user</param>
        /// <param name="channel">channel where the user send it to</param>
        public IrcReceivedEventArgs(string message, string user, string channel)
        {
            this.Message = message;
            this.User = user;
            this.Channel = channel;
        }
    }

    /// <summary>
    /// Event class for receiving raw messages from the irc server, fired within IrcClient.cs
    /// </summary>
    public class IrcRawReceivedEventArgs
    {
        /// <summary>
        /// Containing raw message from the irc server
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Event for handeling raw messages from the server without any parsing applied.
        /// </summary>
        /// <param name="message">message from server</param>
        public IrcRawReceivedEventArgs(string message)
        {
            this.Message = message;
        }
    }

    /// <summary>
    /// Event class for receiving a list with users per channel, fired within IrcClient.cs
    /// </summary>
    public class IrcUserListReceivedEventArgs
    {
        /// <summary>
        /// Dicitonary containing a list with user names per channel
        /// </summary>
        public Dictionary<string, List<string>> UsersPerChannel { get; }

        /// <summary>
        /// Event for getting the usersperchannel list from the server.
        /// </summary>
        /// <param name="usersPerChannel">Dictionary containg a list with names per key (channel)</param>
        public IrcUserListReceivedEventArgs(Dictionary<string, List<string>> usersPerChannel)
        {
            this.UsersPerChannel = usersPerChannel;
        }
    }

    /// <summary>
    /// Event class for receiving debug messages from the IrcClient, fired within IrcClient.cs
    /// </summary>
    public class IrcDebugMessageEventArgs
    {
        /// <summary>
        /// Containing debug message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Containing type of message, handy for determining where the message originates from
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Event for receiving debug messages from the client
        /// </summary>
        /// <param name="message">debug message itself</param>
        /// <param name="type">type of message, handy for identifying where the message occured</param>
        public IrcDebugMessageEventArgs(string message, string type)
        {
            this.Message = message;
            this.Type = type;
        }
    }
}
