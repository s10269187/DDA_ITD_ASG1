using UnityEngine;
using UnityEngine.SceneManagement;

public class SignupButton : MonoBehaviour
{
    public void GoToNextScene()
    {
        SceneManager.LoadScene("Playground");
    }
}