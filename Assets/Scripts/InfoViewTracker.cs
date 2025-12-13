/// <summary>
/// InfoViewTracker.cs
/// This script handles the viewing of the infographic for each snack
/// once player has viewed all three infographic, they will be
/// redirected to the end screen
/// </summary>
/// <author> Leong Ming Hui </author>
/// <date> 13/12/2025 </date>
/// <StudentID> S10267664J </StudentID>
using UnityEngine;
using UnityEngine.SceneManagement;

public class InfoViewTracker : MonoBehaviour
{
    public static InfoViewTracker Instance;

    
    public GameObject biscuitInfoPanel;
    public GameObject bangkitInfoPanel;
    public GameObject hawflakesInfoPanel;

    bool viewedBiscuit;
    bool viewedBangkit;
    bool viewedHawflakes;

    void Awake()
    {
        Instance = this;  // allow static access
    }


    public void CloseBiscuitInfo()
    {
        Debug.Log("Biscuit Info closed!");
        viewedBiscuit = true;
        CheckIfPlayerIsDone();
    }

    public void CloseBangkitInfo()
    {
        Debug.Log("Bangkit Info closed!");
        viewedBangkit = true;
        CheckIfPlayerIsDone();
    }

    public void CloseHawflakesInfo()
    {
        Debug.Log("Hawflakes Info closed!");
        viewedHawflakes = true;
        CheckIfPlayerIsDone();
    }

    private void CheckIfPlayerIsDone() {
        Debug.Log($"CheckIfPlayerIsDone called. Biscuit={viewedBiscuit}, Bangkit={viewedBangkit}, Hawflakes={viewedHawflakes}");

        if (viewedBiscuit && viewedBangkit)
        {
            Debug.Log("ALL VIEWED, LOADING END SCREEN!");
            SceneManager.LoadScene("EndScreen");
        }
    }

}   
