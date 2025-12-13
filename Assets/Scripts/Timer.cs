/// <summary>
/// Timer.cs
/// This script handles the lava obstacle in the final stage of our haunted house
/// When player steps onto the lava, the player automatically dies and respawn
/// at the spawn point set in Unity as an empty GameObject
/// </summary>
/// <author> Lee Jia Lu </author>
/// <date> 09/12+/2025 </date>
/// <StudentID> S10269187E </StudentID>
using UnityEngine;
using TMPro;


/// <summary>
/// Timer code in which is formatted nicely to minutes and seconds
/// </summary>
public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    private float elapsedTime;
    private bool isPaused;

    void Update()
    {
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    public void StopTimer()
    {
        enabled = false; // stops Update
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }
}
