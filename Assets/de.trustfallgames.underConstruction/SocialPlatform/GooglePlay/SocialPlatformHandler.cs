﻿using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using de.TrustfallGames.UnderConstruction.Core;

namespace de.TrustfallGames.UnderConstruction.SocialPlatform.GooglePlay
{
    public class SocialPlatformHandler : MonoBehaviour
    {
        private static SocialPlatformHandler _instance = null;
        //[SerializeField] private bool userIsAuthenticated;


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
            if (_instance != this) return;
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
        public void UserAuthentication()
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    //Check to resend a failed highscore send attempt
                    int scoreToCheck = PlayerPrefHandler.GetLastSentHighScore();

                    if (scoreToCheck != 0)
                        SendToLeaderboard(scoreToCheck);

                    //Check to resend a failed achievement complete attempt
                    string achievementToCheck = PlayerPrefHandler.GetLastSentAchievement();

                    if (achievementToCheck != "None")
                        CompleteAchievement(achievementToCheck);
                }
            });
        }

        /// <summary>
        /// Function to send a score to a specific leaderboard on Google Play
        /// </summary>
        /// <param name="identifier">The string identifier of the leaderboard</param>
        /// <param name="score">The new score to send</param>
        public void SendToLeaderboard(int score)
        {
            Social.ReportScore(score, _defaultLeaderboard, (bool success) =>
            {
                if (!success)
                {
                    PlayerPrefHandler.SetLastSentHighscore(score);
                }
                else
                {
                    PlayerPrefHandler.SetLastSentHighscore(0);
                }
            });
        }

        /// <summary>
        /// Function to progress a specific achievement by percent
        /// </summary>
        /// <param name="identifier">The achievement to progress</param>
        /// <param name="progress">Progress in %</param>
        public void ProgressAchievement(string identifier, int progress)
        {
            PlayGamesPlatform.Instance.IncrementAchievement(
                identifier, progress, (bool success) =>
                {
                    //TODO: handle success or failure
                });
        }

        /// <summary>
        /// Function to complete a specific achievement
        /// </summary>
        /// <param name="identifier">The achievement to complete</param>
        public void CompleteAchievement(string identifier)
        {
            Social.ReportProgress(identifier, 100.0f, (bool success) =>
            {
                if (!success)
                {
                    PlayerPrefHandler.SetLastSentAchievement(identifier);
                }
                else
                {
                    PlayerPrefHandler.SetLastSentAchievement("None");
                }
            });
        }
    }
}