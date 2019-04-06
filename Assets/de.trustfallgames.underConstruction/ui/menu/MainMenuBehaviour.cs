using System;
using de.TrustfallGames.UnderConstruction.Core.Util;
using de.TrustfallGames.UnderConstruction.SocialPlatform.GooglePlay;
using de.TrustfallGames.UnderConstruction.SoundManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace de.TrustfallGames.UnderConstruction.UI.menu {
    /// <summary>
    /// Class behaviour for the main menu
    /// </summary>
    public class MainMenuBehaviour : MonoBehaviour {
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Image musicIcon;
        [SerializeField] private Image sfxIcon;
        [SerializeField] private Sprite musicOn;
        [SerializeField] private Sprite musicOff;
        [SerializeField] private Sprite sfxOn;
        [SerializeField] private Sprite sfxOff;
        [SerializeField] private TextMeshProUGUI login;
        [SerializeField] private bool isOptionsMenu;
        private bool start;

        private void Start() {
            if (isOptionsMenu) {
                sfxSlider.value = GetGameSoundVolume();
                musicSlider.value = GetGameMusicVolume();
                sfxIcon.sprite = Math.Abs(sfxSlider.value / 5) < 0.01 ? sfxOff : sfxOn;
                musicIcon.sprite = Math.Abs(musicSlider.value / 5) < 0.01 ? musicOff : musicOn;
                start = true;
            }
        }

        private void FixedUpdate() {
            if (isOptionsMenu) {
                if (Social.localUser.authenticated) {
                    login.text = "Log Out";
                } else {
                    login.text = "Log In";
                }
            }
        }

        public void Login() {
#if UNITY_ANDROID
            if (Social.localUser.authenticated) {
                SocialPlatformHandler.GetSocialHandler().LogOut();
            } else {
                SocialPlatformHandler.GetSocialHandler().UserAuthentication();
            }
#endif
        }

        public int GetGameSoundVolume() {
            return (int) (Math.Round(PlayerPrefHandler.GetSfxVolume() * 5, 0));
        }

        public int GetGameMusicVolume() {
            return (int) (Math.Round(PlayerPrefHandler.GetMusicVolume() * 5, 0));
        }

        public void SetGameSoundVolume() {
            if (!start) return;
            float newValue = sfxSlider.value / 5;
            PlayerPrefHandler.SetSfxVolume(newValue);
            SoundHandler.GetInstance().Refresh();
            SoundHandler.GetInstance().PlaySound(SoundName.CharacterPickup);

            sfxIcon.sprite = Math.Abs(newValue) < 0.01 ? sfxOff : sfxOn;
        }

        public void SetGameMusicVolume() {
            if (!start) return;
            float newValue = musicSlider.value / 5;
            PlayerPrefHandler.SetMusicVolume(newValue);
            SoundHandler.GetInstance().Refresh();

            musicIcon.sprite = Math.Abs(newValue) < 0.01 ? musicOff : musicOn;
        }
    }
}