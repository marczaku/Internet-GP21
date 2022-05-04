using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public InputField playerNameInput;
    
    public void OnClick(){
        var client = new TcpClient();
        client.Connect(IPAddress.Loopback, 12244);
        var connection = Connection.Instance;
        connection.Init(client, playerNameInput.text);
        SceneManager.LoadScene("Wait");
    }
}
