using System;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.Core {
    public static class PlayerPrefHandler {
        //Sound
        private static string _musicVolume = "MusicVolume";
        private static string _sfxVolume = "SfxVolume";

        public static float GetMusicVolume() { return PlayerPrefs.GetFloat(_musicVolume); }

        public static void SetMusicVolume(float value) { PlayerPrefs.SetFloat(_musicVolume, Mathf.Clamp(value, 0, 1)); }

        public static float GetSfxVolume() { return PlayerPrefs.GetFloat(_sfxVolume); }

        public static void SetSfxVolume(float value) { PlayerPrefs.SetFloat(_sfxVolume, Mathf.Clamp(value,0,1));}
        
        //Highscore
        private static string _highscores = "Highscores";
        private static string _highscore = "Highscore";
        private static string _lastSentHighscore = "Last Sent HS";
        private static string _lastSentAchievement = "Last Sent AC";

        public static int[] GetHighscores() {
            return Array.ConvertAll(PlayerPrefs.GetString(_highscores).Split(','), s => int.Parse(s));
        }

        public static void SetHighscores(params int[] scores) {
            PlayerPrefs.SetString(_highscores,string.Join(",", scores));
        }

        public static int GetHighScore() { return PlayerPrefs.GetInt(_highscore); }

        public static void SetHighScore(int value)
        {
            if(value > GetHighScore())
                PlayerPrefs.SetInt(_highscore, value);
        }

        public static int GetLastSentHighScore() { return PlayerPrefs.GetInt(_lastSentHighscore); }
        
        public static void SetLastSentHighscore(int value){PlayerPrefs.SetInt(_lastSentHighscore, value);}

        public static string GetLastSentAchievement(){ return PlayerPrefs.GetString(_lastSentAchievement); }
        
        public static void SetLastSentAchievement(string value){PlayerPrefs.SetString(_lastSentAchievement, value);}
    }
}