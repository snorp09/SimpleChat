namespace SimpleChatLib.Types;

/* Barrowing from IRC, a command is a plain text message sent over a socket. It is deliminated by ||
A command is structred as the following:
    | [SOURCE] [COMMAND] [ARGUMENTS] |
A Client message to send a message would look like this:
 | Username message All parameters are concated into a message. |
*/
public class CommandMessage
{
    public string Source {get; set;} = String.Empty;
    public string Command {get; set;} = String.Empty;
    public string[] Args = [];

    public static CommandMessage ParseCommandMessage(string rawMsg)
    {
        CommandMessage command = new();
        var commandMsgSplit = rawMsg.Split(['|', ' '], StringSplitOptions.RemoveEmptyEntries);
        if (commandMsgSplit.Length != 3)
        {
            command.Source = "Err";
            command.Command = "Err";
        }
        command.Source = commandMsgSplit[0];
        command.Command = commandMsgSplit[1];
        command.Args = commandMsgSplit[2..];
        return command;
    }

    public override string ToString()
    {
        return $"Command: {Command}\nSource: {Source}\nArgs: {string.Join(" ", Args)}";
    }

}