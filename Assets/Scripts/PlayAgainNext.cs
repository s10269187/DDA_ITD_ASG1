using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayagainButton : MonoBehaviour
{
    public void GoToNextScene()
    {
        SceneManager.LoadScene("login");
    }
}