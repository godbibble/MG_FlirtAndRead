using UnityEngine;
using UnityEngine.SceneManagement;

public class ProfileManager : MonoBehaviour
{
    public void OpenChatroom()
    {
        SceneManager.LoadScene("Chatroom");
    }
}