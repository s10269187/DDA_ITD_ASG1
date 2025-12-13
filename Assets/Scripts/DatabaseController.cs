/// <summary>
/// DatabaseController.cs
/// This script handles the databased stored
/// When player logins, a data of their email and password
/// is stored as well as the time they took to complete game
/// </summary>
/// <author> Lee Jia Lu </author>
/// <date> 09/12/2025 </date>
/// <StudentID> S10269187E </StudentID>
using UnityEngine;
using Firebase.Database;
using System.Collections.Generic;
using Firebase.Extensions;
using TMPro;
using Firebase.Auth;
using System;

public class DatabaseController : MonoBehaviour
{
    /// <summary>
    /// Input field for the user's email
    /// </summary>
    public TMP_InputField EmailInput;
     /// <summary>
    /// Input field for the user's password
    /// </summary>
    public TMP_InputField PasswordInput;

    /// <summary>
    /// Signs the current Firebase user out.
    /// </summary>
    public void SignOut()
    {
        FirebaseAuth.DefaultInstance.SignOut();
    }

    /// <summary>
    /// Creates a new user account using email and password
    /// Logs an error if sign up requirement fails
    /// </summary>
    public void SignUp()
    {
        var signupTask = FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(EmailInput.text, PasswordInput.text);
        signupTask.ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log("Can't sign in due to error");
            }

            if (task.IsCompleted)
            {
                Debug.Log($"User logged in, id: {task.Result.User.UserId}");

                // Code to create user profile in database
            }
        });
    }

    /// <summary>
    /// Signs in an existing user using email and password
     /// Logs an error if login fails
    /// </summary>
    public void SignIn()
    {
        var signInTask = FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(EmailInput.text, PasswordInput.text);
        signInTask.ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log("Can't sign in due to error");
            }

            if (task.IsCompleted)
            {
                Debug.Log($"User logged in, id: {task.Result.User.UserId}");

                // Code to load the user profile
            }
        });

        Debug.Log("Task completed");
    }
    /// <summary>
    /// Handles Firebase authentication state changes
    /// This triggers whenever the user logs in or out
    /// </summary>
    // Auth event handling example
    private void OnAuthStateChanged(object sender, EventArgs e)
    {
        Debug.Log("Auth state changed!");

        if (FirebaseAuth.DefaultInstance.CurrentUser == null)
        {
            Debug.Log("Player is not logged in!");
        }
        else
        {
            Debug.Log($"Player logged in: {FirebaseAuth.DefaultInstance.CurrentUser.UserId}");
        }
    }

    /// <summary>
    /// Initialises Firebase event listeners and performs
    /// example operations: writing, updating, deleting,
    /// and retrieving data from Firebase Realtime Database
    /// </summary>
    void Start()
    {
        // Auth event handling example
        FirebaseAuth.DefaultInstance.StateChanged += OnAuthStateChanged;


        var db = FirebaseDatabase.DefaultInstance.RootReference;

        // Change a single value using SetValueAsync
        db.Child("players").Child("detach8").Child("score").SetValueAsync(9999);

        // Update using UpdateChildrenAsync
        // This is used to bulk update multiple values
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["name"] = "Some awesome guy";
        data["score"] = 1234;
        db.Child("players").Child("detach8").UpdateChildrenAsync(data);

        // Delete the player "detach8"
        db.Child("players").Child("detach8").RemoveValueAsync();

        // Retrieve
        var retrieveTask = db.Child("players").Child("somenonexistentplayer").GetValueAsync();

        retrieveTask.ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log("Error loading player!");
                return;
            }

            if (task.IsCompleted)
            {
                if (!task.Result.Exists)
                {
                    Debug.Log("Invalid player id!");
                    return;
                }

                string json = task.Result.GetRawJsonValue();
                Debug.Log(json);

                // Deserialize JSON data back to Player object
                Player p = JsonUtility.FromJson<Player>(json);
                Debug.Log($"Player loaded: {p.name}");
            }
        });

        Debug.Log("Successfully made player");


    
    }
    
    // Update is called once per frame
    void Update()
    {

    }
}

