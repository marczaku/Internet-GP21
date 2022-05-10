using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public InputField playerNameInput;
    
    public void OnClick(){
        ServerConnection.Instance.Connect(playerNameInput.text);
        SceneManager.LoadScene("Wait");
    }
}
