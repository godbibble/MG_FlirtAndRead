using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        if (EventSystem.current != null)
        {
            EventSystem.current.enabled= false;
        }

        SceneManager.LoadScene("Webtoon1");
    }
}