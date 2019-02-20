using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;
using de.TrustfallGames.UnderConstruction.Core;
using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using de.TrustfallGames.UnderConstruction.SoundManager;
using de.TrustfallGames.UnderConstruction.SocialPlatform.GooglePlay;
using TMPro;

public class MainMenuBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Button loginButton;
    [SerializeField] private TextMeshProUGUI localHighscore;
    [SerializeField] private PostProcessingProfile defaultProfile;
    [SerializeField] private PostProcessingProfile blurProfile;
    
    private bool optionsOn;

    private void Start()
    {
        sfxSlider.value = GetGameSoundVolume();
        musicSlider.value = GetGameMusicVolume();

        optionsMenu.SetActive(false);
        localHighscore.SetText(PlayerPrefHandler.GetHighScore().ToString());
    }
    
    public void Login()
    {
        SocialPlatformHandler.GetSocialHandler().UserAuthentication();
    }
    
    public void LogOut()
    {
        SocialPlatformHandler.GetSocialHandler().LogOut();
    }

    public void SwapMenuDisplay()
    {
        mainMenu.SetActive(!mainMenu.activeSelf);
        optionsMenu.SetActive(!optionsMenu.activeSelf);
        optionsOn = !optionsOn;
        SoundHandler.GetInstance().PlaySound(SoundName.Click);

        if (!optionsMenu.activeSelf)
        {
            Camera.main.GetComponent<PostProcessingBehaviour>().profile = defaultProfile;
        }
        else
        {
            Camera.main.GetComponent<PostProcessingBehaviour>().profile = blurProfile;
        }
    }

    public int GetGameSoundVolume()
    {
        return (int)(Math.Round(PlayerPrefHandler.GetSfxVolume() * 5, 0));
    }

    public int GetGameMusicVolume()
    {
        return (int)(Math.Round(PlayerPrefHandler.GetMusicVolume() * 5, 0));
    }

    public void SetGameSoundVolume()
    {
        if (optionsOn)
        {
            float newValue = sfxSlider.value / 5;
            PlayerPrefHandler.SetSfxVolume(newValue);
            SoundHandler.GetInstance().Refresh();
            SoundHandler.GetInstance().PlaySound(SoundName.CharacterPickup);
        }
    }

    public void SetGameMusicVolume()
    {
        if (optionsOn)
        {
            float newValue = musicSlider.value / 5;
            PlayerPrefHandler.SetMusicVolume(newValue);
            SoundHandler.GetInstance().Refresh();
        }
    }
}
