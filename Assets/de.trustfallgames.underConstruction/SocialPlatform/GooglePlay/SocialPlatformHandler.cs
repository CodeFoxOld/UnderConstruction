using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using de.TrustfallGames.UnderConstruction.Core;

namespace de.TrustfallGames.UnderConstruction.SocialPlatform.GooglePlay
{
    public class SocialPlatformHandler : MonoBehaviour
    {
        private static SocialPlatformHandler _instance = null;
        [SerializeField] private bool userIsAuthenticated;

        private const string _defaultLeaderboard = "CgkI3PuauqQdEAIQAA";

        void Awake()
        {
            //Singleton instantiation
            if (_instance == null)
                _instance = this;
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
        
        public static SocialPlatformHandler GetSocialHandler()
        {
            return _instance;
        }

        void Start()
        {
            //Keep the social platform handler alive
            DontDestroyOnLoad(this);

            //Enable Debug Log for Play Services and Activate the social Platform
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();

            //Prompt the user for OAuth on game start
            if (!PlayerPrefHandler.FirstStartPromptCheck())
            {
                UserAuthentication();
                PlayerPrefHandler.FirstStartPromptDone();
            }
        }

        /// <summary>
        /// Prompts the user for OAuthentication to Google Play Services. This is required for leaderboards and achievements.
        /// </summary>
        private void UserAuthentication()
        {
            if (!userIsAuthenticated)
            {
                Social.localUser.Authenticate((bool success) =>
                {
                    if (success)
                    {
                        userIsAuthenticated = true;
                        
                        //Check to resend a failed highscore send attempt
                        int scoreToCheck = PlayerPrefHandler.GetLastSentHighScore();

                        if (scoreToCheck != 0)
                            SendToLeaderboard(scoreToCheck);
                        
                        //Check to resend a failed achievement complete attempt
                        string achievementToCheck = PlayerPrefHandler.GetLastSentAchievement();

                        if (achievementToCheck != "None")
                            CompleteAchievement(achievementToCheck);
                    }
                    else
                    {
                        userIsAuthenticated = false;
                    }
                });
            }
        }

        /// <summary>
        /// Function to send a score to a specific leaderboard on Google Play
        /// </summary>
        /// <param name="identifier">The string identifier of the leaderboard</param>
        /// <param name="score">The new score to send</param>
        public void SendToLeaderboard(int score)
        {
            if (userIsAuthenticated)
            {
                Social.ReportScore(score, _defaultLeaderboard, (bool success) =>
                {
                    if (!success)
                    {
                        PlayerPrefHandler.SetLastSentHighscore(score);
                        userIsAuthenticated = false;
                    }
                    else
                    {
                        PlayerPrefHandler.SetLastSentHighscore(0);
                    }
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
                    if (!success)
                    {
                        PlayerPrefHandler.SetLastSentAchievement(identifier);
                        userIsAuthenticated = false;
                    }
                    else
                    {
                        PlayerPrefHandler.SetLastSentAchievement("None");
                    }
                });
            }
        }
    }
}
