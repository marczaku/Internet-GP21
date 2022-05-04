namespace CrumbleStompServer;

/// <summary>
/// Contains all important information for one Match.
/// </summary>
public class MatchInfo
{
    public bool started;
    public PlayerInfo red = new();
    public PlayerInfo blue = new();
}