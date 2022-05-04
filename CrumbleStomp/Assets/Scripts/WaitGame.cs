using Messages;
using UnityEngine;
using UnityEngine.UI;

// TODO: put this script in the wait scene and connect UI
public class WaitGame : MonoBehaviour
{
    public Text redPlayerName;
    public Text bluePlayerName;

    void Awake()
    {
        Connection.Instance.matchInfoMessageReceived += OnMatchInfoMessageReceived;
    }

    private void OnDestroy()
    {
        Connection.Instance.matchInfoMessageReceived -= OnMatchInfoMessageReceived;
    }

    private void OnMatchInfoMessageReceived(MatchInfoMessage obj)
    {
        // TODO: update player names
        // TODO: if matchInfo.ready => switch to game scene
        Debug.Log("Update UI here.");
    }
}
