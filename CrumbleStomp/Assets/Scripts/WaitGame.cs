using CrumbleStompShared.Messages;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaitGame : MonoBehaviour
{
    public Text redPlayerName;
    public Text bluePlayerName;
    
    void Start()
    {
        ServerConnection.Instance.Broker.Subscribe<MatchInfoMessage>(OnMatchInfoReceived);
    }

    private void OnDestroy()
    {
        ServerConnection.Instance.Broker.Unsubscribe<MatchInfoMessage>(OnMatchInfoReceived);
    }

    void OnMatchInfoReceived(MatchInfoMessage message)
    {
        if (!string.IsNullOrEmpty(message.matchInfo.red.data.name))
        {
            redPlayerName.text = message.matchInfo.red.data.name;
        }
        if (!string.IsNullOrEmpty(message.matchInfo.blue.data.name))
        {
            bluePlayerName.text = message.matchInfo.blue.data.name;
        }

        if (message.matchInfo.started)
        {
            SceneManager.LoadScene("Game");
        }
    }
}
