namespace CrumbleStompServer.Messages;

/// <summary>
/// Sent from Client to Server when joining a Match.
/// </summary>
public class LoginMessage
{
    public string playerName;
}