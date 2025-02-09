﻿using SimpleIRCLib.ErrorCodes;

namespace SimpleIRCLib
{
    /// <summary>
    /// Contians all the error codes specified by the RFC1459 protocol
    /// </summary>
    public static class Rfc1459Codes
    {
        /// <summary>
        /// List with error messages.
        /// </summary>
        public static Rfc1459ErrorCode[] ListWithErrors { get; } =
        [
            new(401, "ERR_NOSUCHNICK", "<nickname> :No such nick/channel\nUsed to indicate the nickname parameter supplied to a command is currently unused."),
            new(402, "ERR_NOSUCHSERVER", "<server name> :No such server\nUsed to indicate the server name given currently doesn't exist."),
            new(403, "ERR_NOSUCHCHANNEL", "<channel name> :No such channel\nUsed to indicate the given channel name is invalid."),
            new(404, "ERR_CANNOTSENDTOCHAN", "<channel name> :Cannot send to channel\nSent to a user who is either (a) not on a channel which is mode +n or (b) not a chanop (or mode +v) on a channel which has mode +m set and is trying to send a PRIVMSG message to that channel."),
            new(405, "ERR_TOOMANYCHANNELS", "<channel name> :You have joined too many channels\nSent to a user when they have joined the maximum number of allowed channels and they try to join another channel."),
            new(406, "ERR_WASNOSUCHNICK", "<nickname> :There was no such nickname\nReturned by WHOWAS to indicate there is no history information for that nickname."),
            new(407, "ERR_TOOMANYTARGETS", "<target> :Duplicate recipients. No message delivered\nReturned to a client which is attempting to send a PRIVMSG/NOTICE using the user@host destination format and for a user@host which has several occurrences."),
            new(409, "ERR_NOORIGIN", ":No origin specified\nPING or PONG message missing the originator parameter which is required since these commands must work without valid prefixes."),
            new(411, "ERR_NORECIPIENT", ":No recipient given (<command>)"),
            new(412, "ERR_NOTEXTTOSEND", ":No text to send"),
            new(413, "ERR_NOTOPLEVEL", "<mask> :No toplevel domain specified"),
            new(414, "ERR_WILDTOPLEVEL", @"<mask> :Wildcard in toplevel domain\n412 - 414 are returned by PRIVMSG to indicate that the message wasn't delivered for some reason. ERR_NOTOPLEVEL and ERR_WILDTOPLEVEL are errors that are returned when an invalid use of ""PRIVMSG $<server>"" or ""PRIVMSG #<host>"" is attempted."),
            new(421, "ERR_UNKNOWNCOMMAND", "<command> :Unknown command\nReturned to a registered client to indicate that the command sent is unknown by the server."),
            new(422, "ERR_NOMOTD", ":MOTD File is missing\nServer's MOTD file could not be opened by the server."),
            new(423, "ERR_NOADMININFO", "<server> :No administrative info available\nReturned by a server in response to an ADMIN message when there is an error in finding the appropriate information."),
            new(424, "ERR_FILEERROR", ":File error doing <file op> on <file>\nGeneric error message used to report a failed file operation during the processing of a message."),
            new(431, "ERR_NONICKNAMEGIVEN", ":No nickname given\nReturned when a nickname parameter expected for a command and isn't found."),
            new(432, "ERR_ERRONEUSNICKNAME", "<nick> :Erroneus nickname\nReturned after receiving a NICK message which contains characters which do not fall in the defined set.  See section x.x.x for details on valid nicknames."),
            new(433, "ERR_NICKNAMEINUSE", "<nick> :Nickname is already in use\nReturned when a NICK message is processed that results in an attempt to change to a currently existing nickname."),
            new(436, "ERR_NICKCOLLISION", "<nick> :Nickname collision KILL\nReturned by a server to a client when it detects a nickname collision (registered of a NICK that already exists by another server)."),
            new(441, "ERR_USERNOTINCHANNEL", "<nick> <channel> :They aren't on that channel\nReturned by the server to indicate that the target user of the command is not on the given channel."),
            new(442, "ERR_NOTONCHANNEL", "<channel> :You're not on that channel\nReturned by the server whenever a client tries to perform a channel effecting command for which the client isn't a member."),
            new(443, "ERR_USERONCHANNEL", "<user> <channel> :is already on channel\nReturned when a client tries to invite a user to a channel they are already on."),
            new(444, "ERR_NOLOGIN", "<user> :User not logged in\nReturned by the summon after a SUMMON command for a user was unable to be performed since they were not logged in."),
            new(445, "ERR_SUMMONDISABLED", ":SUMMON has been disabled\nReturned as a response to the SUMMON command.  Must be returned by any server which does not implement it."),
            new(446, "ERR_USERSDISABLED", ":USERS has been disabled\nReturned as a response to the USERS command.  Must be returned by any server which does not implement it."),
            new(451, "ERR_NOTREGISTERED", ":You have not registered\nReturned by the server to indicate that the client must be registered before the server will allow it to be parsed in detail."),
            new(461, "ERR_NEEDMOREPARAMS", "<command> :Not enough parameters\nReturned by the server by numerous commands to indicate to the client that it didn't supply enough parameters."),
            new(462, "ERR_ALREADYREGISTRED", ":You may not reregister\nReturned by the server to any link which tries to change part of the registered details (such as password or user details from second USER message)."),
            new(463, "ERR_NOPERMFORHOST", ":Your host isn't among the privileged\nReturned to a client which attempts to register with a server which does not been setup to allow connections from the host the attempted connection is tried."),
            new(464, "ERR_PASSWDMISMATCH", ":Password incorrect\nReturned to indicate a failed attempt at registering a connection for which a password was required and was either not given or incorrect."),
            new(465, "ERR_YOUREBANNEDCREEP", ":You are banned from this server\nReturned after an attempt to connect and register yourself with a server which has been setup to explicitly deny connections to you."),
            new(467, "ERR_KEYSET", "<channel> :Channel key already set"),
            new(471, "ERR_CHANNELISFULL", "<channel> :Cannot join channel (+l)"),
            new(472, "ERR_UNKNOWNMODE", "<char> :is unknown mode char to me"),
            new(473, "ERR_INVITEONLYCHAN", "<channel> :Cannot join channel (+i)"),
            new(474, "ERR_BANNEDFROMCHAN", "<channel> :Cannot join channel (+b)"),
            new(475, "ERR_BADCHANNELKEY", "<channel> :Cannot join channel (+k)"),
            new(481, "ERR_NOPRIVILEGES", ":Permission Denied- You're not an IRC operator\nAny command requiring operator privileges to operate must return this error to indicate the attempt was unsuccessful."),
            new(482, "ERR_CHANOPRIVSNEEDED", "<channel> :You're not channel operator\nAny command requiring 'chanop' privileges (such as MODE messages) must return this error if the client making the attempt is not a chanop on the specified channel."),
            new(483, "ERR_CANTKILLSERVER", ":You cant kill a server!\nAny attempts to use the KILL command on a server are to be refused and this error returned directly to the client."),
            new(491, "ERR_NOOPERHOST", ":No O-lines for your host\nIf a client sends an OPER message and the server has not been configured to allow connections from the client's host as an operator, this error must be returned."),
            new(501, "ERR_UMODEUNKNOWNFLAG", ":Unknown MODE flag\nReturned by the server to indicate that a MODE message was sent with a nickname parameter and that the a mode flag sent was not recognized."),
            new(502, "ERR_USERSDONTMATCH", ":Cant change mode for other users\nError sent to any user trying to view or change the user mode for a user other than themselves.")
            ];
    }
}
