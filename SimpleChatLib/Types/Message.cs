namespace SimpleChatLib.Types;

/* Barrowing from IRC, a command is a plain text message sent over a socket. It is deliminated by ||
A command is structred as the following:
    | [SOURCE] [COMMAND] [ARGUMENTS] |
A Client message to send a message would look like this:
 | Username message All parameters are concated into a message. |
*/
public class Message
{
    public string Source { get; set; } = String.Empty;
    public string Command { get; set; } = String.Empty;
    public string[] Args = [];

    public static Message ParseMessage(string rawMsg)
    {
        Message msg = new();
        var msgSplit = rawMsg.Split(['|', ' '], StringSplitOptions.RemoveEmptyEntries);
        if (msgSplit.Length != 3)
        {
            msg.Source = "Err";
            msg.Command = "Err";
        }
        msg.Source = msgSplit[0];
        msg.Command = msgSplit[1];
        msg.Args = msgSplit[2..];
        return msg;
    }

    public override string ToString()
    {
        return $"Command: {Command}\nSource: {Source}\nArgs: {string.Join(" ", Args)}";
    }

}