using UnityEngine;
using ILogger = CrumbleStompShared.CrumbleStompShared.Interfaces.ILogger;

public class UnityLogger : ILogger
{
    public void Log(string message)
    {
        Debug.Log(message);
    }
}