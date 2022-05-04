using Messages;
using UnityEngine;
using UnityEngine.UI;

// TODO: put this script in the game scene and connect UI
public class Game : MonoBehaviour
{
    public Text redCookies;
    public Text blueCookies;
    
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
        // TODO: update cookie UIs
        Debug.Log("Update UI here.");
    }
    
    public void OnClick()
    {
        
        // Connection.Instance.SendMessage(new CookieClickMessage());
    }
}
