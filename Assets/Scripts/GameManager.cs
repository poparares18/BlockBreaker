using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using System;
using System.Linq;
using Firebase.Extensions;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI messageText;
    public static bool firstSceneLoaded = true;
    // Firebase variable
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;
    public DatabaseReference DBreference;

    // Start is called before the first frame update
    private void Awake()
    {
        if (firstSceneLoaded)
        {
            ShowMessage();
            firstSceneLoaded = false;
        }
        StartCoroutine(CheckAndFixDependenciesAsync());
    }

    private void ShowMessage()
    {
        messageText.text = string.Format("Welcome, {0} to our block breaker game! Have fun and break anything!", References.userName);
    }

    public void UpdateScore()
    {
        DBreference.Child("users").Child(user.UserId).Child("score").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.Exception != null)
            {
                // something bad happened, handle it
                Debug.LogWarning(message: $"Failed to register task with {task.Exception}");
            }
            else
            {
                DataSnapshot snapshot = task.Result;
                //Debug.Log(snapshot.Value);
                if (snapshot.Value != null)
                {
                    int result = int.Parse(References.currentScore);
                    int result2 = int.Parse(snapshot.Value.ToString());
                    if (result2 < result)
                    {
                        UpdateScoreDatabase();
                    }
                }
                else
                {
                    UpdateUsernameDatabase();
                    UpdateScoreDatabase();
                }
            }
        });
    }

    public void UpdateScoreDatabase()
    {
        Debug.Log(References.currentScore);
        string score = References.currentScore;
        Debug.Log(score);
        Debug.Log(user.UserId);
        DBreference.Child("users").Child(user.UserId).Child("score").SetValueAsync(score).ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.Log("error");
            }
            else if (task.IsCompleted)
            {
                //Database username is now updated
            }
        });
    }

    public void UpdateUsernameDatabase()
    {
        Debug.Log(References.userName);
        string userName = References.userName;
        Debug.Log(userName);
        Debug.Log(user.UserId);
        DBreference.Child("users").Child(user.UserId).Child("username").SetValueAsync(userName).ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.Log("error");
            }
            else if(task.IsCompleted)
            {
                //Database username is now updated
            }
        });
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("FirebaseLogin");
        FindObjectOfType<GameSession>().ResetGame();
    }

    public void BackHome()
    {
        SceneManager.LoadScene("FirebaseLogin");
    }

    private IEnumerator CheckAndFixDependenciesAsync()
    {
        var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();

        yield return new WaitUntil(() => dependencyTask.IsCompleted);

        dependencyStatus = dependencyTask.Result;

        if (dependencyStatus == DependencyStatus.Available)
        {
            InitializeFirebase();
            yield return new WaitForEndOfFrame();
        }
        else
        {
            Debug.LogError("Could not resolve all firebase dependencies: " + dependencyStatus);
        }
    }

    void InitializeFirebase()
    {
        //Set the default instance object
        auth = FirebaseAuth.DefaultInstance;

        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);

        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // Track state changes of the auth object.
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;

            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }

            user = auth.CurrentUser;

            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
            }
        }
    }

    private IEnumerator UpdateUsernameAuth(string _username)
    {
        //Create a user profile and set the username
        UserProfile profile = new UserProfile { DisplayName = _username };

        //Call the Firebase auth update user profile function passing the profile with the username
        var ProfileTask = user.UpdateUserProfileAsync(profile);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

        if (ProfileTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
        }
        else
        {
            //Auth username is now updated
        }
    }
}
