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

    /// Singleton instance for global access
    public static InfoViewTracker Instance;

    
    /// Info panels for each snack
    public GameObject biscuitInfoPanel;
    public GameObject bangkitInfoPanel;
    public GameObject hawflakesInfoPanel;

    
    /// Flags to track if each infographic has been viewed
    bool viewedBiscuit;
    bool viewedBangkit;
    bool viewedHawflakes;

    
    /// Initializes the singleton instance
    void Awake()
    {
        Instance = this;  // allow static access
    }

    
    /// Closes the biscuit info panel and checks if all infographics have been viewed
    public void CloseBiscuitInfo()
    {
        Debug.Log("Biscuit Info closed!");
        viewedBiscuit = true;   // mark as viewed
        CheckIfPlayerIsDone();  // check if all viewed
    }

    /// Closes the bangkit info panel and checks if all infographics have been viewed
    public void CloseBangkitInfo()
    {
        Debug.Log("Bangkit Info closed!");
        viewedBangkit = true;
        CheckIfPlayerIsDone();
    }

    /// Closes the hawflakes info panel and checks if all infographics have been viewed
    public void CloseHawflakesInfo()
    {
        Debug.Log("Hawflakes Info closed!");
        viewedHawflakes = true;
        CheckIfPlayerIsDone();
    }

    /// Checks if all infographics have been viewed and loads the end screen if so
    private void CheckIfPlayerIsDone() {
        Debug.Log($"CheckIfPlayerIsDone called. Biscuit={viewedBiscuit}, Bangkit={viewedBangkit}, Hawflakes={viewedHawflakes}");

        if (viewedBiscuit && viewedBangkit)
        {
            Debug.Log("ALL VIEWED, LOADING END SCREEN!");   //debug log for testing
            SceneManager.LoadScene("EndScreen");    // load end screen
        }
    }

}   
