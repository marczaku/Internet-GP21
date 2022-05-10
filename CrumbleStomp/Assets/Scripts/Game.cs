using CrumbleStompShared.Messages;
using CrumbleStompShared.Networking;
using UnityEngine;
using UnityEngine.UI;

// TODO: put this script in the game scene and connect UI
public class Game : MonoBehaviour
{
    public Text redCookies;
    public Text blueCookies;
    
    void Awake()
    {
    }

    private void OnDestroy()
    {
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
