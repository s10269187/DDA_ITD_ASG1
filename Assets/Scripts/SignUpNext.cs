/// <summary>
/// SignUpNext.cs
/// This script handles login page doing into the playground page upon signing up
/// When player finish signing up, they will be press the sign up button
/// and they will be redirected to the next scene's page which is called Playground
/// </summary>
/// <author> Lee Jia Lu </author>
/// <date> 03/12/2025 </date>
/// <StudentID> S10269187E </StudentID>
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Going to the next scene which is called playground
/// </summary>
public class SignupButton : MonoBehaviour
{
    public void GoToNextScene()
    {
        SceneManager.LoadScene("Playground");
    }
}