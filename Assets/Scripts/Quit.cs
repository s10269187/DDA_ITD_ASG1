/// <summary>
/// Quit.cs
/// This script handles end page going back to the 
/// login page when player is done with the game
/// Once player click, it will loop back to login page
/// </summary>
/// <author> Lee Jia Lu </author>
/// <date> 09/12/2025 </date>
/// <StudentID> S10269187E </StudentID>
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitButton : MonoBehaviour
{
    public void GoToNextScene()
    {
        SceneManager.LoadScene("login");
    }
}