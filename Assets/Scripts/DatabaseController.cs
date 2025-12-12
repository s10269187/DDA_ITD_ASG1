/// <summary>
/// DatabaseController.cs
/// This script handles the lava obstacle in the final stage of our haunted house
/// When player steps onto the lava, the player automatically dies and respawn
/// at the spawn point set in Unity as an empty GameObject
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
                Debug.Log("Can't sign in due to error!!!");
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
    ///     /// Logs an error if login fails
    /// </summary>
    public void SignIn()
    {
        var signInTask = FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(EmailInput.text, PasswordInput.text);
        signInTask.ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log("Can't sign in due to error!!!");
            }

            if (task.IsCompleted)
            {
                Debug.Log($"User logged in, id: {task.Result.User.UserId}");

                // Code to load the user profile
            }
        });

        Debug.Log("Hahahahaha");
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
            Debug.Log("User is not logged in!");
        }
        else
        {
            Debug.Log($"User logged in: {FirebaseAuth.DefaultInstance.CurrentUser.UserId}");
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

        Debug.Log("Hehehehehehh");


        /***
         * BEFORE CRUD
         *
        Player justin = new Player("detach8", "Justin");
        justin.items.Add(new Item("sword", 2));

        Player steve = new Player("steviewonder", "Steve from Minecraft");
        steve.items.Add(new Item("pickaxe", 1));

        Player alex = new Player("alexinwonderland", "Alex from Minecraft");
        alex.items.Add(new Item("shovel", 1));

        string justinJson = JsonUtility.ToJson(justin, true);
        string steveJson = JsonUtility.ToJson(steve);

        Debug.Log(justinJson);
        Debug.Log(steveJson);

        db.Child("players").Child(justin.playerId).SetRawJsonValueAsync(justinJson);
        db.Child("players").Child(steve.playerId).SetRawJsonValueAsync(steveJson);


        var newReference = db.Child("players").Push();

        Debug.Log($"The key is: {newReference.Key}");

        alex.playerId = newReference.Key; // Store the new key
        string alexJson = JsonUtility.ToJson(alex);

        newReference.SetRawJsonValueAsync(alexJson);
        */
    }

    // Update is called once per frame
    void Update()
    {

    }
}

