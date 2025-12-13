using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitButton : MonoBehaviour
{
    public void GoToNextScene()
    {
        SceneManager.LoadScene("login");
    }
}