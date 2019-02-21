using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using de.TrustfallGames.UnderConstruction.Core;
using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using de.TrustfallGames.UnderConstruction.SoundManager;
using de.TrustfallGames.UnderConstruction.SocialPlatform.GooglePlay;
using TMPro;

public class MainMenuBehaviour : MonoBehaviour
{
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private bool isOptionsMenu;

    private void Start()
    {
        if (isOptionsMenu)
        {
            sfxSlider.value = GetGameSoundVolume();
            musicSlider.value = GetGameMusicVolume();
        }
    }
    
    public void Login()
    {
        SocialPlatformHandler.GetSocialHandler().UserAuthentication();
    }

    public void LogOut()
    {
        SocialPlatformHandler.GetSocialHandler().LogOut();
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
        float newValue = sfxSlider.value / 5;
        PlayerPrefHandler.SetSfxVolume(newValue);
        SoundHandler.GetInstance().Refresh();
        SoundHandler.GetInstance().PlaySound(SoundName.CharacterPickup);
    }

    public void SetGameMusicVolume()
    {
        float newValue = musicSlider.value / 5;
        PlayerPrefHandler.SetMusicVolume(newValue);
        SoundHandler.GetInstance().Refresh();
    }
}
