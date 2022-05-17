using CrumbleStompShared.Messages;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public Text redPlayerCookies;
    public Text bluePlayerCookies;
    
    void Start()
    {
        ServerConnection.Instance.Broker.Subscribe<MatchInfoMessage>(OnMatchInfoReceived);
    }

    private void OnDestroy()
    {
        ServerConnection.Instance.Broker.Unsubscribe<MatchInfoMessage>(OnMatchInfoReceived);
    }

    public void OnCookieClicked()
    {
        ServerConnection.Instance.Connection.SendMessage(new CollectCookieMessage());
    }
    
    void OnMatchInfoReceived(MatchInfoMessage message)
    {
        redPlayerCookies.text = message.matchInfo.red.data.cookies.ToString();
        bluePlayerCookies.text = message.matchInfo.blue.data.cookies.ToString();
    }
}
