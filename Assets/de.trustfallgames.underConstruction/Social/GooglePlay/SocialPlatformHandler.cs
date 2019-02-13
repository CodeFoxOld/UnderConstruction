using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class SocialPlatformHandler : MonoBehaviour
{
    private static SocialPlatformHandler _instance = null;
    
    [Header("The Main Menu")] [SerializeField]
    private GameObject _menu;

    private bool userIsAuthenticated;

    void Awake()
    {
        //Singleton instantiation
        if (_instance == null)
            _instance = this;
        else if (_instance != this) {
            Destroy(gameObject);
        }      
    }

    void Start()
    {
        //Keep the social platform handler alive
        DontDestroyOnLoad(this);
        
        //Enable Debug Log for Play Services and Activate the social Platform
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        
        //Prompt the user for OAuth on game start
        UserAuthentication();
    }
    
    /// <summary>
    /// Prompts the user for OAuthentication to Google Play Services. This is required for leaderboards and achievements.
    /// </summary>
    private void UserAuthentication()
    {
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                ((GooglePlayGames.PlayGamesPlatform)Social.Active).SetGravityForPopups(Gravity.BOTTOM);              
                userIsAuthenticated = true;
            }
            else
            {
                userIsAuthenticated = false;
            }           
        });       
    }

    /// <summary>
    /// Function to send a score to a specific leaderboard on Google Play
    /// </summary>
    /// <param name="identifier">The string identifier of the leaderboard</param>
    /// <param name="score">The new score to send</param>
    public void SendToLeaderboard(string identifier, int score)
    {
        if (userIsAuthenticated)
        {
            Social.ReportScore(score, identifier, (bool success) =>
            {
                //TODO: handle success or failure
            });
        }

    }
    
    /// <summary>
    /// Function to progress a specific achievement by percent
    /// </summary>
    /// <param name="identifier">The achievement to progress</param>
    /// <param name="progress">Progress in %</param>
    public void ProgressAchievement(string identifier, int progress)
    {
        if (userIsAuthenticated)
        {
            PlayGamesPlatform.Instance.IncrementAchievement(
                identifier, progress, (bool success) =>
                {
                    //TODO: handle success or failure
                });
        }
    }
    /// <summary>
    /// Function to complete a specific achievement
    /// </summary>
    /// <param name="identifier">The achievement to complete</param>
    public void CompleteAchievement(string identifier)
    {
        if (userIsAuthenticated)
        {
            Social.ReportProgress(identifier, 100.0f, (bool success) =>
            {
                //TODO handle success or failure
            });
        }
    }
}
